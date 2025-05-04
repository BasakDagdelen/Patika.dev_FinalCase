namespace Expense_Management_System.WebApi.ApiResponses;

public class ApiResponse
{
    public bool IsSuccess { get; protected set; }
    public string Message { get; protected set; }
    public int StatusCode { get; protected set; }
    public static ApiResponse Success(string message = "Success", int statusCode = 200)
    {
        return new ApiResponse
        {
            IsSuccess = true,
            Message = message,
            StatusCode = statusCode
        };
    }
    public static ApiResponse Fail(string message = "Error", int statusCode = 400)
    {
        return new ApiResponse
        {
            IsSuccess = false,
            Message = message,
            StatusCode = statusCode
        };
    }
}



public class ApiResponse<TEntity> : ApiResponse
{
    public TEntity Data { get; set; }


    public static ApiResponse<TEntity> Success(TEntity data, string message = "Success", int statusCode = 200)
    {
        return new ApiResponse<TEntity>
        {
            IsSuccess = true,
            Message = message,
            StatusCode = statusCode,
            Data = data,
        };
    }
    public static ApiResponse<TEntity> Fail(string message = "Error", int statusCode = 400)
    {
        return new ApiResponse<TEntity>
        {
            IsSuccess = false,
            Message = message,
            StatusCode = statusCode,
            Data = default
        };
    }
}
