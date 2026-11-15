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
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customers");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.NationalId)
                .HasMaxLength(20)
                .IsRequired();

            builder.HasIndex(c => c.NationalId)
                .IsUnique();

            builder.Property(c => c.FullName)
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(c => c.Email)
                .HasMaxLength(150)
                .IsRequired();

            builder.HasIndex(c => c.Email)
                .IsUnique();

            builder.Property(c => c.PhoneNumber)
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(c => c.Address)
                .HasMaxLength(250)
                .IsRequired();

            builder.Property(c => c.CustomerType)
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();
        }
    }
}
