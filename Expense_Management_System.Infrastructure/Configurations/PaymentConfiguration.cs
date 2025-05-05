using Expense_Management_System.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expense_Management_System.Infrastructure.Configurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.InsertedDate).IsRequired(true);
        builder.Property(x => x.UpdatedDate).IsRequired(false);
        builder.Property(x => x.InsertedUser).IsRequired(true).HasMaxLength(250);
        builder.Property(x => x.UpdatedUser).IsRequired(false).HasMaxLength(250);
        builder.Property(x => x.IsActive).IsRequired(true).HasDefaultValue(true);

        builder.Property(x => x.Amount).IsRequired().HasColumnType("decimal(18,2)").HasPrecision(18, 2);
        builder.Property(x => x.PaymentDate).IsRequired();
        builder.Property(x => x.TransactionReference).IsRequired().HasMaxLength(100);
        builder.Property(x => x.PaymentMethod).IsRequired();

        builder.HasOne(x => x.User)
               .WithMany(x => x.Payments)
               .HasForeignKey(x => x.UserId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Expense)
               .WithMany(x => x.Payments)
               .HasForeignKey(x => x.ExpenseId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
