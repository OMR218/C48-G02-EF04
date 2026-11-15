using C48_G02_EF04.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C48_G02_EF04.Configurations
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.ToTable("Accounts");

            builder.HasKey(a => a.AccountNumber);

            builder.Property(a => a.AccountNumber)
                .HasMaxLength(30)
                .IsRequired();

            builder.Property(a => a.CurrentBalance)
                .HasPrecision(18, 2)
                .HasDefaultValue(0m);

            builder.Property(a => a.AccountType)
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(a => a.OpeningDate)
                .IsRequired();

            builder.Property(a => a.BranchCode)
                .HasMaxLength(10)
                .IsRequired();

            builder.HasOne(a => a.Branch)
                .WithMany(b => b.Accounts)
                .HasForeignKey(a => a.BranchCode)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}