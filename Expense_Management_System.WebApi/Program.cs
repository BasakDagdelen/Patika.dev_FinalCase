using AutoMapper;
using Expense_Management_System.Application.Interfaces.Services;
using Expense_Management_System.Application.Services;
using Expense_Management_System.Application.Validation;
using Expense_Management_System.Domain.Interfaces.Repositories;
using Expense_Management_System.Domain.Interfaces.UnitOfWorks;
using Expense_Management_System.Infrastructure;
using Expense_Management_System.Infrastructure.Repositories;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Expense_Management_System.Application.Mapper;
using Expense_Management_System.Infrastructure.UnitofWorks;
using Expense_Management_System.Infrastructure.Token;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Serilog;
using Expense_Management_System.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAntiforgery();

// Tüm servis kayýtlarýný tek yerden yönetir
builder.Services
    .AddJwtAuthentication(builder.Configuration)
    .AddDatabaseContext(builder.Configuration)
    .AddValidationAndMapping()
    .AddRepositoriesAndServices()
    .AddMapperConfiguration();


Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .CreateLogger();

builder.Host.UseSerilog();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseAntiforgery();

app.UseAuthentication();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
