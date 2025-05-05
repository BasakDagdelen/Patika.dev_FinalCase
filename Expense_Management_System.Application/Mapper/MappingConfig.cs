using AutoMapper;
using Expense_Management_System.Application.DTOs.Requests;
using Expense_Management_System.Application.DTOs.Responses;
using Expense_Management_System.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expense_Management_System.Application.Mapper;

public class MappingConfig: Profile
{
    public MappingConfig()
    {
        CreateMap<ExpenseRequest, Expense>().ReverseMap();
        CreateMap<Expense, ExpenseResponse>().ReverseMap();

        CreateMap<ExpenseCategoryRequest, ExpenseCategory>().ReverseMap();
        CreateMap<ExpenseCategory, ExpenseCategoryResponse>().ReverseMap();

        CreateMap<ExpenseDocumentRequest, ExpenseDocument>().ReverseMap();
        CreateMap<ExpenseDocument, ExpenseDocumentResponse>().ReverseMap();

        CreateMap<PaymentRequest, Payment>().ReverseMap();
        CreateMap<Payment, PaymentResponse>().ReverseMap();

        CreateMap<UserRequest, User>().ReverseMap();
        CreateMap<User, UserResponse>().ReverseMap();

        CreateMap<RegisterRequest, User>()
        .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());

    }
}
