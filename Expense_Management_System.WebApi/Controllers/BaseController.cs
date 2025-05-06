using Expense_Management_System.WebApi.ApiResponses;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Expense_Management_System.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BaseController : ControllerBase
{
    protected Guid CurrentUserId =>
     Guid.TryParse(
         User.FindFirstValue(ClaimTypes.NameIdentifier) ??
         User.FindFirstValue("nameid") ??
         User.FindFirstValue("nameId") ??
         User.FindFirstValue("id") ??
         User.FindFirstValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"),
         out var id)
         ? id
         : throw new UnauthorizedAccessException("User ID not found.");

    protected string CurrentUserRole =>
        User.FindFirstValue(ClaimTypes.Role) ??               // Önce standart claim
        User.FindFirstValue("role") ??                        // Sonra kısa ad
        User.FindFirstValue("http://schemas.microsoft.com/ws/2008/06/identity/claims/role") ?? // Sonra uzun URI
        throw new UnauthorizedAccessException("User role not found.");

    protected ApiResponse<T> Success<T>(T data, string? message = null)
        => ApiResponse<T>.Success(data, message);

    protected ApiResponse Success(string message = "")
        => ApiResponse.Success(message);

    protected ApiResponse<T> Fail<T>(string message, int statusCode = 400)
        => ApiResponse<T>.Fail(message, statusCode);

    protected ApiResponse Fail(string message, int statusCode = 400)
        => ApiResponse.Fail(message, statusCode);
}
