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
public class ExpenseController : BaseController
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
        var userId = CurrentUserId;
        var expenses = await _expenseService.GetActiveExpensesByUserAsync(userId);
        var response = _mapper.Map<IEnumerable<ExpenseResponse>>(expenses);
        return Success(response);
    }

    [HttpGet("history")]
    [Authorize(Roles = "Personnel")]
    public async Task<ApiResponse<IEnumerable<ExpenseResponse>>> GetExpenseHistory()
    {
        var userId = CurrentUserId;
        var expenses = await _expenseService.GetExpensesHistoryByUserAsync(userId);
        var response = _mapper.Map<IEnumerable<ExpenseResponse>>(expenses);
        return Success(response);
    }

    [HttpGet("rejected")]
    [Authorize(Roles = "Personnel")]
    public async Task<ApiResponse<IEnumerable<ExpenseResponse>>> GetRejectedExpenses()
    {
        var userId = CurrentUserId;
        var expenses = await _expenseService.GetRejectedExpensesWithReasonAsync(userId);
        var response = _mapper.Map<IEnumerable<ExpenseResponse>>(expenses);
        return Success(response);
    }

    [HttpGet("filter")]
    [Authorize(Roles = "Personnel")]
    public async Task<ApiResponse<IEnumerable<ExpenseResponse>>> FilterMyExpenses([FromQuery] ExpenseFilterRequest filter)
    {
        var userId = CurrentUserId;
        var expenses = await _expenseService.GetFilteredExpensesForUserAsync(userId, filter.Status, filter.FromDate, filter.ToDate, filter.MinAmount, filter.MaxAmount);
        var response = _mapper.Map<IEnumerable<ExpenseResponse>>(expenses);
        return Success(response);
    }

    [HttpPost]
    [Authorize(Roles = "Personnel")]
    public async Task<ApiResponse<ExpenseResponse>> Create([FromBody] ExpenseRequest request)
    {
        var userId = CurrentUserId;

        if (request == null || request.Amount <= 0 || request.ExpenseCategoryId == Guid.Empty)
            return Fail<ExpenseResponse>("Invalid expense data", 400);

        var expense = _mapper.Map<Expense>(request);
        expense.UserId = userId;

        var created = await _expenseService.AddAsync(expense);
        var response = _mapper.Map<ExpenseResponse>(created);

        return Success(response, "Expense was successfully generated.");
    }

    [HttpGet("pending")]
    [Authorize(Roles = "Admin")]
    public async Task<ApiResponse<IEnumerable<ExpenseResponse>>> GetAllPending()
    {
        var expenses = await _expenseService.GetAllPendingExpensesAsync();
        var response = _mapper.Map<IEnumerable<ExpenseResponse>>(expenses);
        return Success(response);
    }

    [HttpPost("approve/{expenseId}")]
    [Authorize(Roles = "Admin")]
    public async Task<ApiResponse<ExpenseResponse>> Approve(Guid expenseId)
    {
        var adminId = CurrentUserId;
        var approved = await _expenseService.ApproveExpenseAsync(expenseId, adminId);
        var response = _mapper.Map<ExpenseResponse>(approved);
        return Success(response, "The expense is confirmed and the payment simulation is started.");
    }

    [HttpPost("reject/{expenseId}")]
    [Authorize(Roles = "Admin")]
    public async Task<ApiResponse<ExpenseResponse>> Reject(Guid expenseId, [FromBody] RejectExpenseRequest reject)
    {
        var adminId = CurrentUserId;
        var rejected = await _expenseService.RejectExpenseAsync(expenseId, adminId, reject.Reason);
        var response = _mapper.Map<ExpenseResponse>(rejected);
        return Success(response, "Expense denied.");
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<ApiResponse<ExpenseResponse>> GetById(Guid id)
    {
        var expense = await _expenseService.GetByIdAsync(id);
        if (expense == null)
            return Fail<ExpenseResponse>("Expense not found.", 404);

        var response = _mapper.Map<ExpenseResponse>(expense);
        return Success(response);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<ApiResponse<string>> Delete(Guid id)
    {
        try
        {
            await _expenseService.DeleteAsync(id);
            return ApiResponse<string>.Success("Deletion successful.");
        }
        catch (Exception ex)
        {
            return ApiResponse<string>.Fail($"Deletion failed: {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<ApiResponse<ExpenseResponse>> Update(Guid id, [FromBody] ExpenseRequest request)
    {
        var userId = CurrentUserId;
        var existing = await _expenseService.GetByIdAsync(id);
        if (existing == null || existing.UserId != userId)
            return Fail<ExpenseResponse>("No charges found or unauthorised access", 403);

        _mapper.Map(request, existing);
        await _expenseService.UpdateAsync(id, existing);

        var mappedEntity = _mapper.Map<ExpenseResponse>(existing);
        return Success(mappedEntity, "Expense updated");
    }
}
