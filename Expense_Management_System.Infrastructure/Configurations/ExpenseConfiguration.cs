using Expense_Management_System.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expense_Management_System.Infrastructure.Configurations;

public class ExpenseConfiguration : IEntityTypeConfiguration<Expense>
{
    public void Configure(EntityTypeBuilder<Expense> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd(); 

        builder.Property(x => x.InsertedDate).IsRequired(true);
        builder.Property(x => x.UpdatedDate).IsRequired(false);
        builder.Property(x => x.InsertedUser).IsRequired(true).HasMaxLength(250);
        builder.Property(x => x.UpdatedUser).IsRequired(false).HasMaxLength(250);
        builder.Property(x => x.IsActive).IsRequired(true).HasDefaultValue(true);

        builder.Property(x => x.PaymentLocation).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Description).IsRequired(false).HasMaxLength(500);
        builder.Property(x => x.Amount).IsRequired().HasColumnType("decimal(18,2)").HasPrecision(18, 2);
        builder.Property(x => x.Status).IsRequired();
        builder.Property(x => x.PaymentMethod).IsRequired();
        builder.Property(x => x.RejectionReason).IsRequired(false).HasMaxLength(300);
      
        builder.HasOne(x => x.User)
               .WithMany(u => u.Expenses)
               .HasForeignKey(x => x.UserId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ExpenseCategory)
               .WithMany(c => c.Expenses)
               .HasForeignKey(x => x.ExpenseCategoryId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ApprovedByUser)
               .WithMany()
               .HasForeignKey(x => x.ApprovedByUserId)
               .OnDelete(DeleteBehavior.SetNull); // Yönetici silinse bile talep kalabilir

        builder.HasMany(x => x.ExpenseDocuments)
               .WithOne(d => d.Expenses)
               .HasForeignKey(d => d.ExpenseId)
               .OnDelete(DeleteBehavior.Cascade); // Talep silinirse belgeler de silinsin
    }
}
