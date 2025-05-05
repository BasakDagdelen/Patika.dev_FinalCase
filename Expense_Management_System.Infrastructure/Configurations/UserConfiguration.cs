using Expense_Management_System.Domain.Entities;
using Expense_Management_System.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expense_Management_System.Infrastructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.InsertedDate).IsRequired(true);
        builder.Property(x => x.UpdatedDate).IsRequired(false);
        builder.Property(x => x.InsertedUser).IsRequired(true).HasMaxLength(250);
        builder.Property(x => x.UpdatedUser).IsRequired(false).HasMaxLength(250);
        builder.Property(x => x.IsActive).IsRequired(true).HasDefaultValue(true);

        builder.Property(x => x.FirstName).IsRequired(true).HasMaxLength(50);
        builder.Property(x => x.LastName).IsRequired(true).HasMaxLength(50);
        builder.Property(x => x.Email).IsRequired(true).HasMaxLength(100);
        builder.HasIndex(x => x.Email).IsUnique();
        builder.Property(x => x.PasswordHash).IsRequired(true).HasMaxLength(255);
        builder.Property(x => x.PhoneNumber).IsRequired(true).HasMaxLength(15);
        builder.Property(x => x.WorkPhoneNumber).HasMaxLength(15);
        builder.Property(x => x.Address).IsRequired(false).HasMaxLength(200);
        //builder.Property(x => x.IBAN).IsRequired().HasMaxLength(34);
        builder.Property(x => x.Role).IsRequired().HasConversion<string>();

        builder.HasMany(x => x.Expenses)
               .WithOne(x => x.User)
               .HasForeignKey(x => x.UserId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Payments)
               .WithOne(x => x.User)
               .HasForeignKey(x => x.UserId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasData(
            new User
            {
                Id = Guid.Parse("d290f1ee-6c54-4b01-90e6-d701748f0851"),
                FirstName = "Ahmet",
                LastName = "Cinar",
                Email = "ahmet.cinar@company.com",
                PasswordHash = "$2a$11$5Cm9c0sCwZt5jqAo3AcEYO8vgVfZycgLsPuIsmj6/MuxiNuvzDaI2",  // Admin123!
                PhoneNumber = "905300000000",
                WorkPhoneNumber = "02120000000",
                Address = "Besiktas/Istanbul",
                Role = UserRole.Admin,
                InsertedUser = "system",
                InsertedDate = new DateTime(2024, 05, 06, 12, 0, 0),
                IsActive = true,

            },
        new User
        {
            Id = Guid.Parse("f5a3c4b7-7d23-4c6e-91f0-9e0fa54b8f32"),
            FirstName = "Selim",
            LastName = "Deniz",
            Email = "selim.deniz@company.com",
            PasswordHash = "$2a$11$5Cm9c0sCwZt5jqAo3AcEYO8vgVfZycgLsPuIsmj6/MuxiNuvzDaI2", // Employee123!
            PhoneNumber = "90501112233",
            WorkPhoneNumber = "02120000123",
            Address = "Kartal/Istanbul",
            Role = UserRole.Personnel,
            InsertedUser = "system",
            InsertedDate = new DateTime(2024, 05, 06, 12, 0, 0),
            IsActive = true
        });

    }
}

