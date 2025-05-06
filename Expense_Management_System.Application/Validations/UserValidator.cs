using Expense_Management_System.Application.DTOs.Requests;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Expense_Management_System.Application.Validation;

public class UserValidator : AbstractValidator<UserRequest>
{
    public UserValidator()
    {
        RuleFor(x => x.FirstName)
           .NotEmpty().WithMessage("First name is required...")
           .MaximumLength(50).WithMessage("First name must contain a maximum of 50 characters...");

        RuleFor(x => x.LastName)
        .NotEmpty().WithMessage("Last name is required...")
        .MaximumLength(50).WithMessage("Last name must contain a maximum of 50 characters...");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email address is required...")
            .EmailAddress().WithMessage("Email address format is invalid...");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required...")
            .MaximumLength(6).WithMessage("Password must be at most 6 characters long...")
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9\W]).{1,6}$")
            .WithMessage("Password must include at least one uppercase letter, one lowercase letter, and one number or special character...");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required.")
            .Must(phone =>
            {
                var cleanedPhone = phone?.Replace(" ", "").Replace("-", "");
                return Regex.IsMatch(cleanedPhone, @"^(\+90|0)?5\d{9}$");
            })
            .WithMessage("Invalid format. Examples: +905551234567 or 05551234567");

        RuleFor(x => x.WorkPhoneNumber)
              .Must(phone =>
              {
                  if (string.IsNullOrEmpty(phone)) return true;
                  var cleanedPhone = Regex.Replace(phone, @"[\s\-()]", "");
                  return Regex.IsMatch(cleanedPhone, @"^(\+?[1-9]\d{1,14}|0\d{10,14})$");
              })
               .WithMessage("Invalid format. Examples: +902121234567, 02121234567 veya 05551234567");

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Address is required...")
            .MaximumLength(250).WithMessage("Address must contain a maximum of 50 characters...");

        RuleFor(x => x.IBAN)
            .NotEmpty().WithMessage("IBAN is required...")
            .Length(26).WithMessage("IBAN must be exactly 26 characters long...")
            .Matches(@"^TR\d{24}$").WithMessage("IBAN format is invalid. It must start with 'TR' followed by 24 digits...");

        RuleFor(x => x.AccountNumber)
           .NotEmpty().WithMessage("Account number cannot be empty.")
           .Matches(@"^\d{8}$").WithMessage("Account number must be exactly 8 digits long.");

        RuleFor(x => x.UserRole)
            .IsInEnum().WithMessage("Invalid user role...");
    }
}
