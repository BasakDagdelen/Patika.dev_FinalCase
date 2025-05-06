using AutoMapper;
using Expense_Management_System.Application.DTOs.Requests;
using Expense_Management_System.Application.DTOs.Responses;
using Expense_Management_System.Application.Interfaces.Services;
using Expense_Management_System.Application.Services;
using Expense_Management_System.Domain.Entities;
using Expense_Management_System.Domain.Interfaces.UnitOfWorks;
using Expense_Management_System.WebApi.ApiResponses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Expense_Management_System.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExpenseController : BaseController
{
    private readonly IExpenseService _expenseService;
    private readonly IExpenseDocumentService _expenseDocumentService;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public ExpenseController(IExpenseService expenseService, IMapper mapper, IExpenseDocumentService expenseDocumentService, IUnitOfWork unitOfWork)
    {
        _expenseService = expenseService;
        _mapper = mapper;
        _expenseDocumentService = expenseDocumentService;
        _unitOfWork = unitOfWork;
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


    [HttpPost]
    [Consumes("multipart/form-data")]
    [Authorize(Roles = "Personnel")]
    public async Task<ApiResponse<ExpenseResponse>> Create([FromForm] ExpenseRequest request)
    {
        var userId = CurrentUserId;

        if (request is null || request.Amount <= 0 || request.ExpenseCategoryId == Guid.Empty)
            return Fail<ExpenseResponse>("Invalid expense data", 400);

        var expense = _mapper.Map<Expense>(request);
        expense.UserId = userId;
        expense.InsertedDate = DateTime.Now;
        expense.InsertedUser = "system";
        expense.IsActive = true;

        var created = await _expenseService.AddAsync(expense);

        if (request.Documents != null && request.Documents.Any())
        {
            foreach (var document in request.Documents)
            {
                if (document.Length > 0)
                {
                    var fakeFileName = Guid.NewGuid() + Path.GetExtension(document.FileName);
                    var fakeFilePath = $"/documents/expenses/{created.Id}/{fakeFileName}";

                    var expenseDocument = new ExpenseDocument
                    {
                        ExpenseId = created.Id,
                        FilePath = fakeFilePath,
                        FileName = document.FileName,
                        UploadDate = DateTime.Now,
                        InsertedDate = DateTime.Now,
                        InsertedUser = userId.ToString(),
                        IsActive = true
                    };
                    await _expenseDocumentService.AddAsync(expenseDocument);
                }
            }
        }


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
        var approvedExpense = await _expenseService.ApproveExpenseAndPayAsync(expenseId, adminId);
        var response = _mapper.Map<ExpenseResponse>(approvedExpense);
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
    [Consumes("multipart/form-data")]
    public async Task<ApiResponse<ExpenseResponse>> Update(Guid id, [FromForm] ExpenseRequest request)
    {
        var userId = CurrentUserId;

        var existing = await _expenseService.GetByIdAsync(id);
        if (existing is null)
            return Fail<ExpenseResponse>("Expense document found.");

        _mapper.Map(request, existing);
        existing.UpdatedDate = DateTime.Now;
        existing.UpdatedUser = userId.ToString();
        await _expenseService.UpdateAsync(id, existing);

        if (request.Documents != null && request.Documents.Any())
        {
            foreach (var document in request.Documents)
            {
                if (document.Length > 0)
                {
                    var fakeFileName = Guid.NewGuid() + Path.GetExtension(document.FileName);
                    var fakeFilePath = $"/documents/expenses/{id}/{fakeFileName}";

                    var expenseDocument = new ExpenseDocument
                    {
                        ExpenseId = id,
                        FilePath = fakeFilePath,
                        FileName = document.FileName,
                        UploadDate = DateTime.Now,
                        UpdatedUser = userId.ToString(),
                        UpdatedDate = DateTime.Now,
                        IsActive = true
                    };
                    await _expenseDocumentService.AddAsync(expenseDocument);
                }
            }
        }
        var mappedEntity = _mapper.Map<ExpenseResponse>(existing);
        return Success(mappedEntity, "Expense updated");

    }

}