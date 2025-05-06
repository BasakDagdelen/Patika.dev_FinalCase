using Expense_Management_System.Application.DTOs.Requests;
using FluentValidation;

namespace Expense_Management_System.Application.Validation;

public class PaymentValidator: AbstractValidator<PaymentRequest>
{
    public PaymentValidator()
    {
        RuleFor(x => x.ExpenseId)
            .NotEmpty().WithMessage("Expense claim is required...");

        RuleFor(x => x.BankAccountNumber)
           .NotEmpty().WithMessage("Account number is required...");

        RuleFor(x => x.PaymentMethod)
            .IsInEnum().WithMessage("Invalid payment method...");
    }
}
