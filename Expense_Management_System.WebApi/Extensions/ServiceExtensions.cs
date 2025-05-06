using AutoMapper;
using Expense_Management_System.Application.Interfaces.Services;
using Expense_Management_System.Application.Mapper;
using Expense_Management_System.Application.Services;
using Expense_Management_System.Application.Validation;
using Expense_Management_System.Domain.Interfaces.Repositories;
using Expense_Management_System.Domain.Interfaces.UnitOfWorks;
using Expense_Management_System.Infrastructure;
using Expense_Management_System.Infrastructure.Repositories;
using Expense_Management_System.Infrastructure.Token;
using Expense_Management_System.Infrastructure.UnitofWorks;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System;
using System.Text;

namespace Expense_Management_System.WebApi.Extensions;

public static class ServiceExtensions
{
    // 1. JWT ve Authentication Ayarları
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("JwtSettings");
        services.Configure<JwtSettings>(jwtSettings);
        services.AddScoped<IJwtTokenService, JwtTokenService>();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]))
            };
        });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
        });

        services.AddAntiforgery();

        return services;
    }


    // 2. Database ve Entity Framework Ayarları
    public static IServiceCollection AddDatabaseContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(
                configuration.GetConnectionString("SqlServer"),
                x => x.MigrationsAssembly("Expense_Management_System.Infrastructure")
            );
        });

        return services;
    }

    // 3. FluentValidation Ayarı 
    public static IServiceCollection AddValidationAndMapping(this IServiceCollection services)
    {
        services.AddControllers()
            .AddFluentValidation(x =>
            {
                x.RegisterValidatorsFromAssemblyContaining<UserValidator>();
            });  

        return services;
    }

    // 4. Repository ve Service Kayıtları
    public static IServiceCollection AddRepositoriesAndServices(this IServiceCollection services)
    {

        services.Scan(scan => scan
            // Infrastructure katmanındaki tüm Repository'leri bul
            .FromAssemblies(typeof(GenericRepository<>).Assembly)
            .AddClasses(classes => classes.Where(t => t.Name.EndsWith("Repository")))
            .AsImplementedInterfaces()
            .WithScopedLifetime()

            // Application katmanındaki tüm Service'leri bul
            .AddClasses(classes => classes.Where(t => t.Name.EndsWith("Service")))
            .AsImplementedInterfaces()
            .WithScopedLifetime()
        );

        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped(typeof(IGenericService<>), typeof(GenericService<>));
        services.AddScoped<IUnitOfWork, UnitofWork>();

        return services;
    }


    //5. AutoMapper Ayarı
    public static IServiceCollection AddMapperConfiguration(this IServiceCollection services)
    {
        services.AddSingleton(new MapperConfiguration(x => x.AddProfile(new MappingConfig())).CreateMapper());
        return services;
    }
}
