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
    public class CustomerAccountConfiguration : IEntityTypeConfiguration<CustomerAccount>
    {
        public void Configure(EntityTypeBuilder<CustomerAccount> builder)
        {
            builder.ToTable("CustomerAccounts");

            builder.HasKey(ca => new
            {
                ca.CustomerId,
                ca.AccountNumber
            });

            builder.Property(ca => ca.AccountNumber)
                .HasMaxLength(30)
                .IsRequired();

            builder.Property(ca => ca.OwnershipType)
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(ca => ca.AccountStatus)
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(ca => ca.OwnershipStartDate)
                .IsRequired();

            builder.HasOne(ca => ca.Customer)
                .WithMany(c => c.CustomerAccounts)
                .HasForeignKey(ca => ca.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ca => ca.Account)
                .WithMany(a => a.CustomerAccounts)
                .HasForeignKey(ca => ca.AccountNumber)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
