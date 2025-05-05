using Expense_Management_System.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expense_Management_System.Infrastructure.Configurations;

public class BankAccountConfiguration : IEntityTypeConfiguration<BankAccount>
{
    public void Configure(EntityTypeBuilder<BankAccount> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.InsertedDate).IsRequired();
        builder.Property(x => x.InsertedUser).IsRequired().HasMaxLength(250);
        builder.Property(x => x.UpdatedDate).IsRequired(false);
        builder.Property(x => x.UpdatedUser).HasMaxLength(250);
        builder.Property(x => x.IsActive).HasDefaultValue(true);

        builder.Property(x => x.IBAN).IsRequired().HasMaxLength(34);
        builder.Property(x => x.AccountNumber).IsRequired().HasMaxLength(20);

        builder.HasOne(x => x.User)
               .WithOne(u => u.BankAccount)
               .HasForeignKey<BankAccount>(x => x.UserId)
               .OnDelete(DeleteBehavior.Cascade);
 
    }
}
