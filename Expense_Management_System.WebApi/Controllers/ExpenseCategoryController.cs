using AutoMapper;
using Expense_Management_System.Application.DTOs.Requests;
using Expense_Management_System.Application.DTOs.Responses;
using Expense_Management_System.Application.Interfaces.Services;
using Expense_Management_System.Domain.Entities;
using Expense_Management_System.WebApi.ApiResponses;
using Microsoft.AspNetCore.Mvc;

namespace Expense_Management_System.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExpenseCategoryController : ControllerBase
{
    private readonly IExpenseCategoryService _expenseCategoryService;
    private readonly IMapper _mapper;

    public ExpenseCategoryController(IExpenseCategoryService expenseCategoryService, IMapper mapper)
    {
        _expenseCategoryService = expenseCategoryService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ApiResponse<IEnumerable<ExpenseCategoryResponse>>> GetAll()
    {
        var entities = await _expenseCategoryService.GetAllAsync();
        var mappedEntity = _mapper.Map<IEnumerable<ExpenseCategoryResponse>>(entities);
        return ApiResponse<IEnumerable<ExpenseCategoryResponse>>.Success(mappedEntity);
    }

    [HttpGet("{id}")]
    public async Task<ApiResponse<ExpenseCategoryResponse>> GetById(Guid id)
    {
        var entity = await _expenseCategoryService.GetByIdAsync(id);
        if (entity is null)
            return ApiResponse<ExpenseCategoryResponse>.Fail("Category not found", 404);

        var mappedEntity = _mapper.Map<ExpenseCategoryResponse>(entity);
        return ApiResponse<ExpenseCategoryResponse>.Success(mappedEntity);
    }

    [HttpPost]
    public async Task<ApiResponse<ExpenseCategoryResponse>> Create([FromBody] ExpenseCategoryRequest categoryRequest)
    {
        if (await _expenseCategoryService.IsCategoryNameExistsAsync(categoryRequest.Name))
            return ApiResponse<ExpenseCategoryResponse>.Fail("Category name already exists", 400);

        var entity = _mapper.Map<ExpenseCategory>(categoryRequest);
        var createdEntity = await _expenseCategoryService.AddAsync(entity);
        var mappedEntity = _mapper.Map<ExpenseCategoryResponse>(createdEntity);
        return ApiResponse<ExpenseCategoryResponse>.Success(mappedEntity, "Category name successfully created.");
    }

    [HttpPut("{id}")]
    public async Task<ApiResponse<ExpenseCategoryResponse>> Update(Guid id, [FromBody] ExpenseCategoryRequest categoryRequest)
    {
        var existingEntity = await _expenseCategoryService.GetByIdAsync(id);
        if (existingEntity is null)
            return ApiResponse<ExpenseCategoryResponse>.Fail("Category not found", 404);

        if (await _expenseCategoryService.IsCategoryNameExistsAsync(categoryRequest.Name, id))
            return ApiResponse<ExpenseCategoryResponse>.Fail("Category name already exists");

        var updatedEntity = _mapper.Map(categoryRequest, existingEntity);
        await _expenseCategoryService.UpdateAsync(id, updatedEntity);

        var mappedEntity = _mapper.Map<ExpenseCategoryResponse>(updatedEntity);
        return ApiResponse<ExpenseCategoryResponse>.Success(mappedEntity, "Category successfully updated.");
    }

    [HttpDelete("{id}")]
    public async Task<ApiResponse> Delete(Guid id)
    {
        var entity = await _expenseCategoryService.GetByIdAsync(id);
        if (entity is null)
            return ApiResponse.Fail("Category not found", 404);

        var canDelete = await _expenseCategoryService.CanDeleteCategoryAsync(id);
        if (!canDelete)
            return ApiResponse.Fail("Cannot be deleted as there are active expense records for this category.");

        await _expenseCategoryService.DeleteAsync(entity.Id);
        return ApiResponse.Success("Category name successfully deleted.");
    }
}

