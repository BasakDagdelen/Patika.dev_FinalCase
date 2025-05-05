using AutoMapper;
using Expense_Management_System.Application.DTOs.Requests;
using Expense_Management_System.Application.DTOs.Responses;
using Expense_Management_System.Application.Interfaces.Services;
using Expense_Management_System.Domain.Entities;
using Expense_Management_System.WebApi.ApiResponses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Expense_Management_System.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExpenseController : ControllerBase
{
    private readonly IExpenseService _expenseService;
    private readonly IMapper _mapper;

    public ExpenseController(IExpenseService expenseService, IMapper mapper)
    {
        _expenseService = expenseService;
        _mapper = mapper;
    }

    [HttpGet("active")]
    [Authorize(Roles = "Personnel")]
    public async Task<ApiResponse<IEnumerable<ExpenseResponse>>> GetActiveExpenses()
    {
        var userId = GetUserId();
        var expenses = await _expenseService.GetActiveExpensesByUserAsync(userId);
        var response = _mapper.Map<IEnumerable<ExpenseResponse>>(expenses);
        return ApiResponse<IEnumerable<ExpenseResponse>>.Success(response);
    }

    [HttpGet("history")]
    [Authorize(Roles = "Personnel")]
    public async Task<ApiResponse<IEnumerable<ExpenseResponse>>> GetExpenseHistory()
    {
        var userId = GetUserId();
        var expenses = await _expenseService.GetExpensesHistoryByUserAsync(userId);
        var response = _mapper.Map<IEnumerable<ExpenseResponse>>(expenses);
        return ApiResponse<IEnumerable<ExpenseResponse>>.Success(response);
    }

    [HttpGet("rejected")]
    [Authorize(Roles = "Personnel")]
    public async Task<ApiResponse<IEnumerable<ExpenseResponse>>> GetRejectedExpenses()
    {
        var userId = GetUserId();
        var expenses = await _expenseService.GetRejectedExpensesWithReasonAsync(userId);
        var response = _mapper.Map<IEnumerable<ExpenseResponse>>(expenses);
        return ApiResponse<IEnumerable<ExpenseResponse>>.Success(response);
    }

    // Personelin filtreli arama
    [HttpGet("filter")]
    [Authorize(Roles = "Personnel")]
    public async Task<ApiResponse<IEnumerable<ExpenseResponse>>> FilterMyExpenses([FromQuery] ExpenseFilterRequest filter)
    {
        var userId = GetUserId();
        var expenses = await _expenseService.GetFilteredExpensesForUserAsync(userId, filter.Status, filter.FromDate, filter.ToDate, filter.MinAmount, filter.MaxAmount);
        var response = _mapper.Map<IEnumerable<ExpenseResponse>>(expenses);
        return ApiResponse<IEnumerable<ExpenseResponse>>.Success(response);
    }

    // Yeni masraf oluşturma
    [HttpPost]
    [Authorize(Roles = "Personnel")]
    public async Task<ApiResponse<ExpenseResponse>> Create([FromBody] ExpenseRequest request)
    {
        var userId = GetUserId();

        if (request == null || request.Amount <= 0 || request.ExpenseCategoryId == Guid.Empty)
            return ApiResponse<ExpenseResponse>.Fail("Geçersiz masraf verisi", 400);

        var expense = _mapper.Map<Expense>(request);
        expense.UserId = userId;

        var created = await _expenseService.AddAsync(expense);
        var response = _mapper.Map<ExpenseResponse>(created);

        return ApiResponse<ExpenseResponse>.Success(response, "Masraf başarıyla oluşturuldu.");
    }

    // Admin - tüm bekleyen masraflar
    [HttpGet("pending")]
    [Authorize(Roles = "Admin")]
    public async Task<ApiResponse<IEnumerable<ExpenseResponse>>> GetAllPending()
    {
        var expenses = await _expenseService.GetAllPendingExpensesAsync();
        var response = _mapper.Map<IEnumerable<ExpenseResponse>>(expenses);
        return ApiResponse<IEnumerable<ExpenseResponse>>.Success(response);
    }

    // Admin - masraf onaylama
    [HttpPost("approve/{expenseId}")]
    [Authorize(Roles = "Admin")]
    public async Task<ApiResponse<ExpenseResponse>> Approve(Guid expenseId)
    {
        var adminId = GetUserId();
        var approved = await _expenseService.ApproveExpenseAsync(expenseId, adminId);
        var response = _mapper.Map<ExpenseResponse>(approved);
        return ApiResponse<ExpenseResponse>.Success(response, "Masraf onaylandı ve ödeme simülasyonu başlatıldı.");
    }

    // Admin - masraf reddetme
    [HttpPost("reject/{expenseId}")]
    [Authorize(Roles = "Admin")]
    public async Task<ApiResponse<ExpenseResponse>> Reject(Guid expenseId, [FromBody] RejectExpenseRequest reject)
    {
        var adminId = GetUserId();
        var rejected = await _expenseService.RejectExpenseAsync(expenseId, adminId, reject.Reason);
        var response = _mapper.Map<ExpenseResponse>(rejected);
        return ApiResponse<ExpenseResponse>.Success(response, "Masraf reddedildi.");
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<ApiResponse<ExpenseResponse>> GetById(Guid id)
    {
        var expense = await _expenseService.GetByIdAsync(id);
        if (expense == null)
            return ApiResponse<ExpenseResponse>.Fail("Masraf bulunamadı", 404);

        var response = _mapper.Map<ExpenseResponse>(expense);
        return ApiResponse<ExpenseResponse>.Success(response);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<ApiResponse<string>> Delete(Guid id)
    {
        try
        {
            await _expenseService.DeleteAsync(id); 
            return ApiResponse<string>.Success("Silme işlemi başarılı.");
        }
        catch (Exception ex) 
        {
            return ApiResponse<string>.Fail($"Silme işlemi başarısız: {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<ApiResponse<ExpenseResponse>> Update(Guid id, [FromBody] ExpenseRequest request)
    {
        var userId = GetUserId();
        var existing = await _expenseService.GetByIdAsync(id);
        if (existing == null || existing.UserId != userId)
            return ApiResponse<ExpenseResponse>.Fail("Masraf bulunamadı veya yetkisiz erişim", 403);

        _mapper.Map(request, existing);
        await _expenseService.UpdateAsync(id, existing);

        var mappedEntity = _mapper.Map<ExpenseResponse>(existing);
        return ApiResponse<ExpenseResponse>.Success(mappedEntity, "Masraf güncellendi");
    }


    private Guid GetUserId()
    {
        return Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
    }
}
