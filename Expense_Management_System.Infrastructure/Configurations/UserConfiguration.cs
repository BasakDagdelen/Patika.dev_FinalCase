using Expense_Management_System.Domain.Models.Entities;
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
    }
}
