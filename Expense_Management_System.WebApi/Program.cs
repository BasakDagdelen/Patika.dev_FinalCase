using AutoMapper;
using Expense_Management_System.Application.Interfaces.Services;
using Expense_Management_System.Application.Services;
using Expense_Management_System.Application.Validation;
using Expense_Management_System.Domain.Interfaces.Repositories;
using Expense_Management_System.Domain.Interfaces.UnitOfWorks;
using Expense_Management_System.Infrastructure;
using Expense_Management_System.Infrastructure.Repositories;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Expense_Management_System.Application.Mapper;
using Expense_Management_System.Infrastructure.UnitofWorks;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddFluentValidation(x =>
{
    x.RegisterValidatorsFromAssemblyContaining<UserValidator>();
});

builder.Services.AddDbContext<ApplicationDbContext>(config =>
{
    config.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"),
                        x => x.MigrationsAssembly("Expense_Management_System.Infrastructure"));
});

builder.Services.AddSingleton(new MapperConfiguration(x => x.AddProfile(new MappingConfig())).CreateMapper());

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped(typeof(IGenericService<>), typeof(GenericService<>));
builder.Services.AddScoped<IUnitOfWork, UnitofWork>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
