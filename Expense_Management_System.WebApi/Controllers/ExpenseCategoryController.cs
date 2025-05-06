using AutoMapper;
using Expense_Management_System.Application.DTOs.Requests;
using Expense_Management_System.Application.DTOs.Responses;
using Expense_Management_System.Application.Interfaces.Services;
using Expense_Management_System.Domain.Entities;
using Expense_Management_System.WebApi.ApiResponses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Expense_Management_System.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExpenseCategoryController : BaseController
{
    private readonly IExpenseCategoryService _expenseCategoryService;
    private readonly IMapper _mapper;

    public ExpenseCategoryController(IExpenseCategoryService expenseCategoryService, IMapper mapper)
    {
        _expenseCategoryService = expenseCategoryService;
        _mapper = mapper;
    }

    [Authorize]
    [HttpGet]
    public async Task<ApiResponse<IEnumerable<ExpenseCategoryResponse>>> GetAll()
    {
        var entities = await _expenseCategoryService.GetAllAsync();
        var mappedEntity = _mapper.Map<IEnumerable<ExpenseCategoryResponse>>(entities);
        return Success(mappedEntity);
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<ApiResponse<ExpenseCategoryResponse>> GetById(Guid id)
    {
        var entity = await _expenseCategoryService.GetByIdAsync(id);
        if (entity is null)
            return Fail<ExpenseCategoryResponse>("Category not found", 404);

        var mappedEntity = _mapper.Map<ExpenseCategoryResponse>(entity);
        return Success(mappedEntity);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ApiResponse<ExpenseCategoryResponse>> Create([FromBody] ExpenseCategoryRequest categoryRequest)
    {
        if (await _expenseCategoryService.IsCategoryNameExistsAsync(categoryRequest.Name))
            return Fail<ExpenseCategoryResponse>("Category name already exists", 400);

        var entity = _mapper.Map<ExpenseCategory>(categoryRequest);

        entity.InsertedDate = DateTime.UtcNow;
        entity.InsertedUser = "system";
        entity.IsActive = true;

        var createdEntity = await _expenseCategoryService.AddAsync(entity);
        var mappedEntity = _mapper.Map<ExpenseCategoryResponse>(createdEntity);
        return Success(mappedEntity, "Category name successfully created.");
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<ApiResponse<ExpenseCategoryResponse>> Update(Guid id, [FromBody] ExpenseCategoryRequest categoryRequest)
    {

        var existingEntity = await _expenseCategoryService.GetByIdAsync(id);
        if (existingEntity is null)
            return Fail<ExpenseCategoryResponse>("Category not found", 404);

        // Sadece isim değişiyorsa kontrol et
        if (!string.Equals(existingEntity.Name, categoryRequest.Name, StringComparison.OrdinalIgnoreCase))
        {
            if (await _expenseCategoryService.IsCategoryNameExistsAsync(categoryRequest.Name))
                return Fail<ExpenseCategoryResponse>("Category name already exists");
        }

        _mapper.Map(categoryRequest, existingEntity);
        existingEntity.UpdatedDate = DateTime.UtcNow;
        existingEntity.UpdatedUser = CurrentUserId.ToString();
        await _expenseCategoryService.UpdateAsync(id, existingEntity);

        var mappedEntity = _mapper.Map<ExpenseCategoryResponse>(existingEntity);
        return Success(mappedEntity, "Category successfully updated.");
    }


    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<ApiResponse> Delete(Guid id)
    {
        try
        {
            var existingEntity = await _expenseCategoryService.GetByIdAsync(id);
            if (existingEntity is null)
                return Fail("Category not found", 404);

            var canDelete = await _expenseCategoryService.CanDeleteCategoryAsync(id);
            if (!canDelete)
                return Fail("Cannot be deleted as there are active expense records for this category.");

            await _expenseCategoryService.DeleteAsync(existingEntity.Id);
            return Success("Category name successfully deleted.");
        }
        catch
        {
            return Fail("Delete failed. Please try again later.", 500);
        }
    }
}
