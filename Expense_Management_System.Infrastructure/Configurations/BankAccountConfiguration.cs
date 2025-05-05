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

        builder.HasData(
        new BankAccount
        {
            Id = Guid.Parse("d290f1ee-6c54-4b01-90e6-d701748f0855"),
            IBAN = "TR330006100519786457841326",
            AccountNumber = "12345678",
            UserId = Guid.Parse("d290f1ee-6c54-4b01-90e6-d701748f0851"),
            InsertedUser = "system",
            InsertedDate = new DateTime(2024, 05, 06, 12, 0, 0),
            IsActive = true
        },
        new BankAccount
        {
            Id = Guid.Parse("a6b1c9e2-37c4-49e8-b1af-10b1546e529f\r\n"),
            IBAN = "TR330006100519786457841327",
            AccountNumber = "87654321",
            UserId = Guid.Parse("f5a3c4b7-7d23-4c6e-91f0-9e0fa54b8f32"),
            InsertedUser = "system",
            InsertedDate = new DateTime(2024, 05, 06, 12, 0, 0),
            IsActive = true
        }
        );

    }
}
