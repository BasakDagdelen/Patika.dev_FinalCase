using Expense_Management_System.Domain.Entities;
using Expense_Management_System.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;


namespace Expense_Management_System.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<ExpenseCategory> ExpenseCategories { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<ExpenseDocument> ExpenseDocuments { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<User> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //var adminUserId = Guid.NewGuid();
            //var employeeUserId = Guid.NewGuid();

            //modelBuilder.Entity<User>().HasData(
            //    new User
            //    {
            //        Id = adminUserId,
            //        FirstName = "Admin",
            //        LastName = "User",
            //        Email = "admin@company.com",
            //        PasswordHash = "$2a$11$Cm93CrScW2tSjqAo3AcEYO0kgVtZvcqLsPu1smj6/MuKiKuVzOaT2",   // Admin123!
            //        PhoneNumber = "05000000000",
            //        WorkPhoneNumber = "02120000000",
            //        Address = "Beşiktaş/İstanbul",
            //        IBAN = "TR000000000000000000000000",
            //        Role = UserRole.Admin,
            //        InsertedUser = "system",
            //        InsertedDate = DateTime.Now,
            //        IsActive = true
            //    },
            //     new User
            //     {
            //         Id = employeeUserId,
            //         FirstName = "Field",
            //         LastName = "Worker",
            //         Email = "employee@company.com",
            //         PasswordHash = "$2a$11$xNlpsCUq8HrfTFSip5R6HOpNIdfq7fZf6AYqQUwE9ZKDYgsFuE9Fe",  // Employee123!
            //         PhoneNumber = "05001112233",
            //         WorkPhoneNumber = null,
            //         Address = "Kartal/İstanbul",
            //         IBAN = "TR111111111111111111111111",
            //         Role = UserRole.Employee,
            //         InsertedUser = "system",
            //         InsertedDate = DateTime.Now,
            //         IsActive = true
            //     });

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
