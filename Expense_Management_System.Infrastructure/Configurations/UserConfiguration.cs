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

        builder.Property(x => x.FirstName).IsRequired().HasMaxLength(50);
        builder.Property(x => x.LastName).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Email).IsRequired().HasMaxLength(100);
        builder.HasIndex(x => x.Email).IsUnique();
        builder.Property(x => x.PasswordHash).IsRequired().HasMaxLength(255);
        builder.Property(x => x.PhoneNumber).IsRequired().HasMaxLength(15);
        builder.Property(x => x.WorkPhoneNumber).HasMaxLength(15);
        builder.Property(x => x.Address).IsRequired().HasMaxLength(200);
        builder.Property(x => x.IBAN).IsRequired().HasMaxLength(34);
        builder.Property(x => x.Role).IsRequired();

        builder.HasMany(x => x.Expenses)
               .WithOne(x => x.User)
               .HasForeignKey(x => x.UserId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Payments)
               .WithOne(x => x.User)
               .HasForeignKey(x => x.UserId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasData(
            builder.Property(x => x.Id == Guid.NewGuid()),
            builder.Property(x => x.FirstName == "Ahmet"),
            builder.Property(x => x.LastName == "Çınar"),
            builder.Property(x => x.Email == "ahmet_cinar@company.com"),
            builder.Property(x => x.PasswordHash == "$2a$11$Cm93CrScW2tSjqAo3AcEYO0kgVtZvcqLsPu1smj6/MuKiKuVzOaT2"),// Admin123!
            builder.Property(x => x.PhoneNumber == "05000000000"),
            builder.Property(x => x.WorkPhoneNumber == "02120000000"),
            builder.Property(x => x.Address == "Beşiktaş/İstanbul"),
            builder.Property(x => x.IBAN == "TR000000000000000000000000"),
            builder.Property(x => x.Role == UserRole.Admin),
            builder.Property(x => x.InsertedUser == "system"),
            builder.Property(x => x.InsertedDate == DateTime.Now),
            builder.Property(x => x.IsActive == true));

        builder.HasData(
            builder.Property(x => x.Id == Guid.NewGuid()),
            builder.Property(x => x.FirstName == "Selma"),
            builder.Property(x => x.LastName == "Deniz"),
            builder.Property(x => x.Email == "selma_deniz@company.com"),
            builder.Property(x => x.PasswordHash == "$2a$11$xNlpsCUq8HrfTFSip5R6HOpNIdfq7fZf6AYqQUwE9ZKDYgsFuE9Fe"),// Employee123!
            builder.Property(x => x.PhoneNumber == "05001112233"),
            builder.Property(x => x.WorkPhoneNumber == "02120000123"),
            builder.Property(x => x.Address == "Kartal/İstanbul"),
            builder.Property(x => x.IBAN == "TR111111111111111111111111"),
            builder.Property(x => x.Role == UserRole.Employee),
            builder.Property(x => x.InsertedUser == "system"),
            builder.Property(x => x.InsertedDate == DateTime.Now),
            builder.Property(x => x.IsActive == true));
    }
}

 