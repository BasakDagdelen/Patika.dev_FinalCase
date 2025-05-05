using Expense_Management_System.Application.DTOs.Requests;
using FluentValidation;

namespace Expense_Management_System.Application.Validation;

public class PaymentValidator: AbstractValidator<PaymentRequest>
{
    public PaymentValidator()
    {
        RuleFor(x => x.ExpenseClaimId)
            .NotEmpty().WithMessage("Expense claim is required.");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than 0.");

        RuleFor(x => x.PaymnetMethod)
            .IsInEnum().WithMessage("Invalid payment method.");
    }
}
