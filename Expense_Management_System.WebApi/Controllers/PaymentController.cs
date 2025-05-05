using AutoMapper;
using Expense_Management_System.Application.DTOs.Requests;
using Expense_Management_System.Application.DTOs.Responses;
using Expense_Management_System.Application.Interfaces.Services;
using Expense_Management_System.Domain.Entities;
using Expense_Management_System.WebApi.ApiResponses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Expense_Management_System.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IMapper _mapper;

        public PaymentController(IPaymentService paymentService, IMapper mapper)
        {
            _paymentService = paymentService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ApiResponse<IEnumerable<PaymentResponse>>> GetAll()
        {
            var payments = await _paymentService.GetAllAsync();
            var mappedPayments = _mapper.Map<IEnumerable<PaymentResponse>>(payments);
            return ApiResponse<IEnumerable<PaymentResponse>>.Success(mappedPayments);
        }

        [HttpGet("{id}")]
        public async Task<ApiResponse<PaymentResponse>> GetById(Guid id)
        {
            var payment = await _paymentService.GetByIdAsync(id);
            if (payment == null)
                return ApiResponse<PaymentResponse>.Fail("Payment not found", 404);

            var mappedPayment = _mapper.Map<PaymentResponse>(payment); 
            return ApiResponse<PaymentResponse>.Success(mappedPayment);
        }

       
        [HttpGet("expense/{expenseId}")]
        public async Task<ApiResponse<PaymentResponse>> GetByExpenseId(Guid expenseId)
        {
            var payment = await _paymentService.GetPaymentByExpenseIdAsync(expenseId);
            if (payment == null)
                return ApiResponse<PaymentResponse>.Fail("Payment for the given expense not found", 404);

            var mappedPayment = _mapper.Map<PaymentResponse>(payment);
            return ApiResponse<PaymentResponse>.Success(mappedPayment);
        }

        [HttpGet("user/{userId}")]
        public async Task<ApiResponse<IEnumerable<PaymentResponse>>> GetByUserId(Guid userId)
        {
            var payments = await _paymentService.GetPaymentsByUserIdAsync(userId);
            var mappedPayments = _mapper.Map<IEnumerable<PaymentResponse>>(payments);
            return ApiResponse<IEnumerable<PaymentResponse>>.Success(mappedPayments);
        }

        [HttpPost]
        public async Task<ApiResponse<PaymentResponse>> Create([FromBody] PaymentRequest paymentRequest)
        {
            var payment = _mapper.Map<Payment>(paymentRequest); 
            var createdPayment = await _paymentService.AddAsync(payment);

            var mappedPayment = _mapper.Map<PaymentResponse>(createdPayment);
            return ApiResponse<PaymentResponse>.Success(mappedPayment, "Payment successfully created.");
        }

        [HttpPut("{id}")]
        public async Task<ApiResponse<PaymentResponse>> Update(Guid id, [FromBody] PaymentRequest paymentRequest)
        {
            var payment = await _paymentService.GetByIdAsync(id);
            if (payment == null)
                return ApiResponse<PaymentResponse>.Fail("Payment not found", 404);

            var updatedPayment = _mapper.Map(paymentRequest, payment); 
            await _paymentService.UpdateAsync(id, updatedPayment);

            var mappedPayment = _mapper.Map<PaymentResponse>(updatedPayment);
            return ApiResponse<PaymentResponse>.Success(mappedPayment, "Payment successfully updated.");
        }

        [HttpDelete("{id}")]
        public async Task<ApiResponse> Delete(Guid id)
        {
            var payment = await _paymentService.GetByIdAsync(id);
            if (payment == null)
                return ApiResponse.Fail("Payment not found", 404);

            await _paymentService.DeleteAsync(payment.Id); 
            return ApiResponse.Success("Payment successfully deleted.");
        }
    }
}
