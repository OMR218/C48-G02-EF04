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
    public class ManagerConfiguration : IEntityTypeConfiguration<Manager>
    {
        public void Configure(EntityTypeBuilder<Manager> builder)
        {
            builder.ToTable("Managers");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.FullName)
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(m => m.Email)
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(m => m.PhoneNumber)
                .HasMaxLength(20)
                .IsRequired();

            builder.HasIndex(m => m.Email)
                .IsUnique();

            builder.HasData(
                new Manager
                {
                    Id = 1,
                    FullName = "Ahmed Hassan",
                    Email = "ahmed.hassan@nationalbank.local",
                    PhoneNumber = "01010000001",
                    HireDate = new DateTime(2018, 1, 15)
                },

                new Manager
                {
                    Id = 2,
                    FullName = "Mona Ali",
                    Email = "mona.ali@nationalbank.local",
                    PhoneNumber = "01010000002",
                    HireDate = new DateTime(2019, 6, 1)
                }
            );
        }
    }
}
