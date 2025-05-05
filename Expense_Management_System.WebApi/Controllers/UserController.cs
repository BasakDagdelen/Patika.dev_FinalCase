using AutoMapper;
using Expense_Management_System.Application.DTOs.Requests;
using Expense_Management_System.Application.DTOs.Responses;
using Expense_Management_System.Application.Interfaces.Services;
using Expense_Management_System.Application.Services;
using Expense_Management_System.Domain.Entities;
using Expense_Management_System.Domain.Enums;
using Expense_Management_System.WebApi.ApiResponses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Expense_Management_System.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{

    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public UserController(IUserService userService, IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ApiResponse<IEnumerable<UserResponse>>> GetAll()
    {
        var entities = await _userService.GetAllAsync();
        var mappedEntity = _mapper.Map<IEnumerable<UserResponse>>(entities);
        return ApiResponse<IEnumerable<UserResponse>>.Success(mappedEntity);
    }

    [HttpGet("{id}")]
    public async Task<ApiResponse<UserResponse>> GetById(Guid id)
    {
        var entity = await _userService.GetByIdAsync(id);
        if (entity is null)
            return ApiResponse<UserResponse>.Fail("User not found", 404);

        var mappedEntity = _mapper.Map<UserResponse>(entity);
        return ApiResponse<UserResponse>.Success(mappedEntity);
    }

    [HttpGet("email/{email}")]
    public async Task<ApiResponse<UserResponse>> GetByEmail(string email)
    {
        var user = await _userService.GetUserByEmailAsync(email);
        if (user == null)
            return ApiResponse<UserResponse>.Fail("User not found", 404);

        var mappedUser = _mapper.Map<UserResponse>(user);
        return ApiResponse<UserResponse>.Success(mappedUser);
    }

    [HttpGet("role/{role}")]
    public async Task<ApiResponse<IEnumerable<UserResponse>>> GetByRole(UserRole role)
    {
        var users = await _userService.GetUsersByRoleAsync(role);
        var mappedUsers = _mapper.Map<IEnumerable<UserResponse>>(users);
        return ApiResponse<IEnumerable<UserResponse>>.Success(mappedUsers);
    }

    [HttpPost]
    public async Task<ApiResponse<UserResponse>> Create([FromBody] UserRequest userRequest)
    {

        if (await _userService.CheckUserExistsAsync(userRequest.Email))
            return ApiResponse<UserResponse>.Fail("User with this email already exists", 400);

        var entity = _mapper.Map<User>(userRequest); 
        var createdEntity = await _userService.AddAsync(entity);
        var mappedEntity = _mapper.Map<UserResponse>(createdEntity);
        return ApiResponse<UserResponse>.Success(mappedEntity, "User successfully created.");
    }

    [HttpPut("{id}")]
    public async Task<ApiResponse<UserResponse>> Update(Guid id, [FromBody] UserRequest userRequest)
    {
        var entity = await _userService.GetByIdAsync(id);
        if (entity is null)
            return ApiResponse<UserResponse>.Fail("User not found", 404);

        if (await _userService.CheckUserExistsAsync(userRequest.Email) && entity.Email != userRequest.Email)
            return ApiResponse<UserResponse>.Fail("Email already exists", 400);

        var updatedEntity = _mapper.Map(userRequest, entity); 
        await _userService.UpdateAsync(id, updatedEntity);
        var mappedEntity = _mapper.Map<UserResponse>(updatedEntity);
        return ApiResponse<UserResponse>.Success(mappedEntity, "User successfully updated.");
    }

    [HttpPut("role/{id}")]
    public async Task<ApiResponse> ChangeRole(Guid id, [FromBody] UserRole newRole)
    {
        var entity = await _userService.GetByIdAsync(id);
        if (entity is null)
            return ApiResponse.Fail("User not found", 404);

        await _userService.ChangeUserRoleAsync(id, newRole);
        return ApiResponse.Success("User role successfully updated.");
    }

    [HttpDelete("{id}")]
    public async Task<ApiResponse> Delete(Guid id)
    {
        var entity = await _userService.GetByIdAsync(id);
        if (entity == null)
            return ApiResponse.Fail("User not found", 404);

        await _userService.DeleteAsync(entity.Id); 
        return ApiResponse.Success("User successfully deleted.");
    }

    [HttpGet("{id}/expenses")]
    public async Task<ApiResponse<IEnumerable<ExpenseResponse>>> GetUserExpenses(Guid id)
    {
        var entity = await _userService.GetUserByIdWithExpensesAsync(id);
        if (entity == null)
            return ApiResponse<IEnumerable<ExpenseResponse>>.Fail("User not found", 404);

        var mappedExpenses = _mapper.Map<IEnumerable<ExpenseResponse>>(entity.Expenses);
        return ApiResponse<IEnumerable<ExpenseResponse>>.Success(mappedExpenses);
    }
}
