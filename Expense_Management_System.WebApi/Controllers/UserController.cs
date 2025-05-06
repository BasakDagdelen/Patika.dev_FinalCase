using AutoMapper;
using Azure.Core;
using Expense_Management_System.Application.DTOs.Requests;
using Expense_Management_System.Application.DTOs.Responses;
using Expense_Management_System.Application.Interfaces.Services;
using Expense_Management_System.Domain.Entities;
using Expense_Management_System.Domain.Enums;
using Expense_Management_System.WebApi.ApiResponses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Expense_Management_System.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : BaseController
{
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public UserController(IUserService userService, IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<ApiResponse<IEnumerable<UserResponse>>> GetAll()
    {
        var entities = await _userService.GetAllAsync();
        var mappedEntity = _mapper.Map<IEnumerable<UserResponse>>(entities);
        return Success(mappedEntity);
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<ApiResponse<UserResponse>> GetById(Guid id)
    {
        try
        {
            var entity = await _userService.GetByIdAsync(id);
            if (entity is null)
                return Fail<UserResponse>("User not found", 404);

            var mappedEntity = _mapper.Map<UserResponse>(entity);
            return Success(mappedEntity);
        }
        catch
        {
            return Fail<UserResponse>("User not found", 404);
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("email/{email}")]
    public async Task<ApiResponse<UserResponse>> GetByEmail(string email)
    {
        var user = await _userService.GetUserByEmailAsync(email);
        var mappedUser = _mapper.Map<UserResponse>(user);
        return Success(mappedUser);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("role/{role}")]
    public async Task<ApiResponse<IEnumerable<UserResponse>>> GetByRole(UserRole role)
    {
        var users = await _userService.GetUsersByRoleAsync(role);
        var mappedUsers = _mapper.Map<IEnumerable<UserResponse>>(users);
        return Success(mappedUsers);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ApiResponse<UserResponse>> Create([FromBody] UserRequest userRequest)
    {
        if (await _userService.CheckUserExistsAsync(userRequest.Email))
            return Fail<UserResponse>("User with this email already exists", 400);

        var entity = _mapper.Map<User>(userRequest);
        var createdEntity = await _userService.CreateUserAsync(userRequest);
        var mappedEntity = _mapper.Map<UserResponse>(createdEntity);
        return Success(mappedEntity, "User successfully created.");
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<ApiResponse<UserResponse>> Update(Guid id, [FromBody] UserRequest userRequest)
    {
        var entity = await _userService.GetByIdAsync(id);

        if (await _userService.CheckUserExistsAsync(userRequest.Email) && entity.Email != userRequest.Email)
            return Fail<UserResponse>("Email already exists", 400);

        if (!string.IsNullOrEmpty(userRequest.Password))
        {
            entity.PasswordHash = BCrypt.Net.BCrypt.HashPassword(userRequest.Password); 
        }

        _mapper.Map(userRequest, entity);
        entity.UpdatedDate = DateTime.Now;
        entity.UpdatedUser = CurrentUserId.ToString();

        if (entity.BankAccount != null)
        {
            entity.BankAccount.UpdatedDate = DateTime.Now;
            entity.BankAccount.UpdatedUser = CurrentUserId.ToString();
        }

        await _userService.UpdateAsync(id, entity);
        var mappedEntity = _mapper.Map<UserResponse>(entity);
        return Success(mappedEntity, "User successfully updated.");

    }

    [Authorize(Roles = "Admin")]
    [HttpPut("role/{id}")]
    public async Task<ApiResponse> ChangeRole(Guid id, [FromBody] UserRole newRole)
    {
        var entity = await _userService.GetByIdAsync(id);
        entity.UpdatedDate = DateTime.Now;
        entity.UpdatedUser = CurrentUserId.ToString(); 
        await _userService.ChangeUserRoleAsync(id, newRole);
        return Success("User role successfully updated.");
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<ApiResponse> Delete(Guid id)
    {
        var entity = await _userService.GetByIdAsync(id);
        if (entity is null || !entity.IsActive)
            return Fail("User not found", 404);

        await _userService.DeleteAsync(entity.Id);
        return Success("User successfully deleted.");
    }

    [Authorize]
    [HttpGet("{id}/expenses")]
    public async Task<ApiResponse<IEnumerable<ExpenseResponse>>> GetUserExpenses(Guid id)
    {
        var entity = await _userService.GetUserByIdWithExpensesAsync(id);
        var mappedExpenses = _mapper.Map<IEnumerable<ExpenseResponse>>(entity.Expenses);
        return Success(mappedExpenses);
    }
}
