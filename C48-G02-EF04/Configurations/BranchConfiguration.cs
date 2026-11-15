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
    public class BranchConfiguration : IEntityTypeConfiguration<Branch>
    {
        public void Configure(EntityTypeBuilder<Branch> builder)
        {
            builder.ToTable("Branches");

            builder.HasKey(b => b.Code);

            builder.Property(b => b.Code)
                .HasMaxLength(10)
                .IsRequired();

            builder.Property(b => b.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(b => b.Address)
                .HasMaxLength(250)
                .IsRequired();

            builder.Property(b => b.PhoneNumber)
                .HasMaxLength(20)
                .IsRequired();

            // One-to-One: Branch - Manager

            builder.HasIndex(b => b.ManagerId)
                .IsUnique();

            builder.HasOne(b => b.Manager)
                .WithOne(m => m.Branch)
                .HasForeignKey<Branch>(b => b.ManagerId)
                .OnDelete(DeleteBehavior.Restrict);


            builder.HasData(
                new Branch
                {
                    Code = "CAI-01",
                    Name = "Cairo Main Branch",
                    Address = "Downtown, Cairo",
                    PhoneNumber = "0223456789",
                    ManagerId = 1
                },

                new Branch
                {
                    Code = "ALX-01",
                    Name = "Alexandria Branch",
                    Address = "Raml Station, Alexandria",
                    PhoneNumber = "0334567890",
                    ManagerId = 2
                }
            );
        }
    }
}
