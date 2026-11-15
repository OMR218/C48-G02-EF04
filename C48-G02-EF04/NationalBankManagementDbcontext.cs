using C48_G02_EF04.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C48_G02_EF04
{
    public class BankDbContext : DbContext
    {
        protected override void OnConfiguring(
            DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    "Server=.;Database=NationalBankManagementDb;Trusted_Connection=True;TrustServerCertificate=True;"
                );
            }
        }

        public DbSet<Branch> Branches => Set<Branch>();

        public DbSet<Manager> Managers => Set<Manager>();

        public DbSet<Customer> Customers => Set<Customer>();

        public DbSet<Account> Accounts => Set<Account>();

        public DbSet<CustomerAccount> CustomerAccounts
            => Set<CustomerAccount>();

        public DbSet<BankTransaction> Transactions
            => Set<BankTransaction>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(BankDbContext).Assembly
            );
        }
    }
}
