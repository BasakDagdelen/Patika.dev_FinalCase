using Expense_Management_System.WebApi.ApiResponses;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Expense_Management_System.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BaseController : ControllerBase
{
     protected Guid CurrentUserId => 
        Guid.TryParse(User.FindFirst("id")?.Value, out var id)
            ? id
            : throw new UnauthorizedAccessException("User ID not found.");

    protected string CurrentUserRole => 
        User.FindFirst(ClaimTypes.Role)?.Value 
        ?? throw new UnauthorizedAccessException("User role not found.");

    protected ApiResponse<T> Success<T>(T data, string? message = null)
        => ApiResponse<T>.Success(data, message);

    protected ApiResponse Success(string message = "")
        => ApiResponse.Success(message);

    protected ApiResponse<T> Fail<T>(string message, int statusCode = 400)
        => ApiResponse<T>.Fail(message, statusCode);

    protected ApiResponse Fail(string message, int statusCode = 400)
        => ApiResponse.Fail(message, statusCode);
}
