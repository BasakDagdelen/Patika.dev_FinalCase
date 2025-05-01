using Expense_Management_System.Application.DTOs.entitys;
using FluentValidation;

namespace Expense_Management_System.Application.Validation;

public class PaymentValidator: AbstractValidator<PaymenTEntity>
{
    public PaymentValidator()
    {
        RuleFor(x => x.ExpenseClaimId)
            .NotEmpty().WithMessage("Expense claim is required.");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than 0.");

        RuleFor(x => x.Method)
            .IsInEnum().WithMessage("Invalid payment method.");
    }
}
