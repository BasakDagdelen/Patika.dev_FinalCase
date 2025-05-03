using AutoMapper;
using Expense_Management_System.Application.DTOs.entitys;
using Expense_Management_System.Application.DTOs.Responses;
using Expense_Management_System.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expense_Management_System.Application.Mappings;

public class MapperConfiguration: Profile
{
    public MapperConfiguration()
    {
        CreateMap<Expenseentity, Expense>().ReverseMap();
        CreateMap<Expense, ExpenseResponse>().ReverseMap();

        CreateMap<ExpenseCategoryentity, ExpenseCategory>().ReverseMap();
        CreateMap<ExpenseCategory, ExpenseCategoryResponse>().ReverseMap();

        CreateMap<ExpenseDocumenTEntity, ExpenseDocument>().ReverseMap();
        CreateMap<ExpenseDocument, ExpenseDocumenTEntity>().ReverseMap();

        CreateMap<PaymenTEntity, Payment>().ReverseMap();
        CreateMap<Payment, PaymenTEntity>().ReverseMap();

        CreateMap<Userentity, User>().ReverseMap();
        CreateMap<User, UserResponse>().ReverseMap();

    }
}
