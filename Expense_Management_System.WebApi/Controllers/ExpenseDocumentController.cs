using AutoMapper;
using Expense_Management_System.Application.DTOs.Requests;
using Expense_Management_System.Application.DTOs.Responses;
using Expense_Management_System.Application.Interfaces.Services;
using Expense_Management_System.Application.Services;
using Expense_Management_System.Domain.Entities;
using Expense_Management_System.WebApi.ApiResponses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Expense_Management_System.WebApi.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ExpenseDocumentController : BaseController
{
    private readonly IExpenseDocumentService _expenseDocumentService;
    private readonly IMapper _mapper;
    public ExpenseDocumentController(IExpenseDocumentService expenseDocumentService, IMapper mapper)
    {
        _expenseDocumentService = expenseDocumentService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ApiResponse<IEnumerable<ExpenseDocumentResponse>>> GetAll()
    {
        var role = CurrentUserRole;
        IEnumerable<ExpenseDocument> entities;

        if (role == "Admin")
            entities = await _expenseDocumentService.GetAllAsync();
        else
        {
            var userId = CurrentUserId;
            entities = await _expenseDocumentService.GetDocumentsByUserIdAsync(userId);
        }

        var mappedEntity = _mapper.Map<IEnumerable<ExpenseDocumentResponse>>(entities);
        return Success(mappedEntity);
    }

    [HttpGet("{id}")]
    public async Task<ApiResponse<ExpenseDocumentResponse>> GetById(Guid id)
    {
        var entity = await _expenseDocumentService.GetByIdAsync(id);
        if (entity is null)
            return Fail<ExpenseDocumentResponse>("Expense document not found", 404);

        var role = CurrentUserRole;
        if (role != "Admin" && entity.Expenses.UserId != CurrentUserId)
            return Fail<ExpenseDocumentResponse>("Access denied", 403);

        var mappedEntity = _mapper.Map<ExpenseDocumentResponse>(entity);
        return Success(mappedEntity);
    }

    [HttpPost]
    [Authorize(Roles = "Personnel,Admin")]
    [Consumes("multipart/form-data")]
    public async Task<ApiResponse<ExpenseDocumentResponse>> Create([FromForm] ExpenseDocumentRequest documentRequest)
    {
        if (documentRequest is null)
            return Fail<ExpenseDocumentResponse>("Invalid data", 400);

        var expenseDocument = _mapper.Map<ExpenseDocument>(documentRequest);
        expenseDocument.Expenses ??= new Expense(); 
        expenseDocument.Expenses.UserId = CurrentUserId;
        expenseDocument.InsertedUser = "system";
        expenseDocument.InsertedDate = DateTime.UtcNow;
        expenseDocument.IsActive = true;
        expenseDocument.UploadDate = DateTime.Now;

        var createdDocument = await _expenseDocumentService.AddAsync(expenseDocument);
        var mappedEntity = _mapper.Map<ExpenseDocumentResponse>(createdDocument);
        return Success(mappedEntity, "Expense document successfully created.");
    }


    [HttpPut("{id}")]
    [Consumes("multipart/form-data")]
    [Authorize(Roles = "Personnel,Admin")]
    public async Task<ApiResponse<ExpenseDocumentResponse>> Update(Guid id, [FromForm] ExpenseDocumentRequest documentRequest)
    {
        //if (documentRequest is null)
        //    return Fail<ExpenseDocumentResponse>("Invalid data", 400);

        var existingDocument = await _expenseDocumentService.GetByIdAsync(id);
        if (existingDocument is null)
            return Fail<ExpenseDocumentResponse>("Expense document not found", 404);

        var updatedDocument = _mapper.Map(documentRequest, existingDocument);
        await _expenseDocumentService.UpdateAsync(id, updatedDocument);
        var mappedEntity = _mapper.Map<ExpenseDocumentResponse>(updatedDocument);
        return Success(mappedEntity, "Expense document successfully updated.");
    }


    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ApiResponse> Delete(Guid id)
    {
        var existingDocument = await _expenseDocumentService.GetByIdAsync(id);
        if (existingDocument is null)
            return Fail("Expense document not found", 404);

        var role = CurrentUserRole;
        if (role != "Admin")
            return Fail("Access denied", 403);

        await _expenseDocumentService.DeleteAsync(existingDocument.Id);
        return Success("Expense document successfully deleted.");
    }


}
