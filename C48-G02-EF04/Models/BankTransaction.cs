using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C48_G02_EF04.Models
{
    public class BankTransaction
    {
        public long TransactionNumber { get; set; }

        public DateTime TransactionDate { get; set; }

        public decimal Amount { get; set; }

        public TransactionType TransactionType { get; set; }

        public string? Note { get; set; }

        public string AccountNumber { get; set; } = null!;

        public Account Account { get; set; } = null!;
    }
}
