using AutoMapper;
using Expense_Management_System.Application.DTOs.Requests;
using Expense_Management_System.Application.DTOs.Responses;
using Expense_Management_System.Application.Interfaces.Services;
using Expense_Management_System.Domain.Entities;
using Expense_Management_System.WebApi.ApiResponses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Expense_Management_System.WebApi.Controllers;


[Authorize]
[Route("api/[controller]")]
[ApiController]
public class PaymentController : BaseController
{
    private readonly IPaymentService _paymentService;
    private readonly IMapper _mapper;

    public PaymentController(IPaymentService paymentService, IMapper mapper)
    {
        _paymentService = paymentService;
        _mapper = mapper;
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<ApiResponse<IEnumerable<PaymentResponse>>> GetAll()
    {
        var payments = await _paymentService.GetAllAsync();
        var mappedPayments = _mapper.Map<IEnumerable<PaymentResponse>>(payments);
        return Success(mappedPayments);
    }

    [HttpGet("{id}")]
    public async Task<ApiResponse<PaymentResponse>> GetById(Guid id)
    {
        var payment = await _paymentService.GetByIdAsync(id);
        if (payment is null)
            return Fail<PaymentResponse>("Payment not found", 404);

        var userRole = User.FindFirstValue(ClaimTypes.Role);
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userRole == "Personel" && payment.Expense.UserId.ToString() != userId)
            return Fail<PaymentResponse>("Unauthorized access", 403);

        var mappedPayment = _mapper.Map<PaymentResponse>(payment);
        return Success(mappedPayment);
    }


    [HttpGet("expense/{expenseId}")]
    public async Task<ApiResponse<PaymentResponse>> GetByExpenseId(Guid expenseId)
    {
        var payment = await _paymentService.GetPaymentByExpenseIdAsync(expenseId);
        if (payment is null)
            return Fail<PaymentResponse>("Payment for the given expense not found", 404);

        var userRole = User.FindFirstValue(ClaimTypes.Role);
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userRole == "Personel" && payment.Expense.UserId.ToString() != userId)
            return Fail<PaymentResponse>("Unauthorized access", 403);

        var mappedPayment = _mapper.Map<PaymentResponse>(payment);
        return Success(mappedPayment);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("user/{userId}")]
    public async Task<ApiResponse<IEnumerable<PaymentResponse>>> GetByUserId(Guid userId)
    {
        var payments = await _paymentService.GetPaymentsByUserIdAsync(userId);
        var mappedPayments = _mapper.Map<IEnumerable<PaymentResponse>>(payments);
        return Success(mappedPayments);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ApiResponse<PaymentResponse>> Create([FromBody] PaymentRequest paymentRequest)
    {
        var payment = _mapper.Map<Payment>(paymentRequest);
        var createdPayment = await _paymentService.AddAsync(payment);

        var mappedPayment = _mapper.Map<PaymentResponse>(createdPayment);
        return Success(mappedPayment, "Payment successfully created.");
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<ApiResponse<PaymentResponse>> Update(Guid id, [FromBody] PaymentRequest paymentRequest)
    {
        var payment = await _paymentService.GetByIdAsync(id);
        if (payment == null)
            return Fail<PaymentResponse>("Payment not found", 404);

        var updatedPayment = _mapper.Map(paymentRequest, payment);
        await _paymentService.UpdateAsync(id, updatedPayment);

        var mappedPayment = _mapper.Map<PaymentResponse>(updatedPayment);
        return Success(mappedPayment, "Payment successfully updated.");
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<ApiResponse> Delete(Guid id)
    {
        var payment = await _paymentService.GetByIdAsync(id);
        if (payment == null)
            return Fail("Payment not found", 404);

        await _paymentService.DeleteAsync(payment.Id);
        return Success("Payment successfully deleted.");
    }
}