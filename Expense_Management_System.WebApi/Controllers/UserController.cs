using AutoMapper;
using Expense_Management_System.Application.DTOs.Requests;
using Expense_Management_System.Application.DTOs.Responses;
using Expense_Management_System.Application.Interfaces.Services;
using Expense_Management_System.Application.Services;
using Expense_Management_System.Domain.Entities;
using Expense_Management_System.Domain.Enums;
using Expense_Management_System.WebApi.ApiResponses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        var entity = await _userService.GetByIdAsync(id);
        if (entity is null)
            return Fail<UserResponse>("User not found", 404);

        var mappedEntity = _mapper.Map<UserResponse>(entity);
        return Success(mappedEntity);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("email/{email}")]
    public async Task<ApiResponse<UserResponse>> GetByEmail(string email)
    {
        var user = await _userService.GetUserByEmailAsync(email);
        if (user is null)
            return Fail<UserResponse>("User not found", 404);

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
        var createdEntity = await _userService.AddAsync(entity);
        var mappedEntity = _mapper.Map<UserResponse>(createdEntity);
        return Success(mappedEntity, "User successfully created.");
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<ApiResponse<UserResponse>> Update(Guid id, [FromBody] UserRequest userRequest)
    {
        var entity = await _userService.GetByIdAsync(id);
        if (entity is null)
            return Fail<UserResponse>("User not found", 404);

        if (await _userService.CheckUserExistsAsync(userRequest.Email) && entity.Email != userRequest.Email)
            return Fail<UserResponse>("Email already exists", 400);

        var updatedEntity = _mapper.Map(userRequest, entity);
        await _userService.UpdateAsync(id, updatedEntity);
        var mappedEntity = _mapper.Map<UserResponse>(updatedEntity);
        return Success(mappedEntity, "User successfully updated.");
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("role/{id}")]
    public async Task<ApiResponse> ChangeRole(Guid id, [FromBody] UserRole newRole)
    {
        var entity = await _userService.GetByIdAsync(id);
        if (entity is null)
            return Fail("User not found", 404);

        await _userService.ChangeUserRoleAsync(id, newRole);
        return Success("User role successfully updated.");
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<ApiResponse> Delete(Guid id)
    {
        var entity = await _userService.GetByIdAsync(id);
        if (entity is null)
            return Fail("User not found", 404);

        await _userService.DeleteAsync(entity.Id);
        return Success("User successfully deleted.");
    }

    [Authorize]
    [HttpGet("{id}/expenses")]
    public async Task<ApiResponse<IEnumerable<ExpenseResponse>>> GetUserExpenses(Guid id)
    {
        var entity = await _userService.GetUserByIdWithExpensesAsync(id);
        if (entity is null)
            return Fail<IEnumerable<ExpenseResponse>>("User not found", 404);

        var mappedExpenses = _mapper.Map<IEnumerable<ExpenseResponse>>(entity.Expenses);
        return Success(mappedExpenses);
    }
}

