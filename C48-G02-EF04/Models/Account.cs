using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C48_G02_EF04.Models
{
    public class Account
    {
        public string AccountNumber { get; set; } = null!;

        public decimal CurrentBalance { get; set; }

        public AccountType AccountType { get; set; }

        public DateTime OpeningDate { get; set; }

        public string BranchCode { get; set; } = null!;

        public Branch Branch { get; set; } = null!;

        public List<CustomerAccount> CustomerAccounts { get; set; } = new();

        public List<BankTransaction> Transactions { get; set; } = new();
    }
}
