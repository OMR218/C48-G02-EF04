using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C48_G02_EF04.Models
{
    public class Customer
    {
        public int Id { get; set; }

        public string NationalId { get; set; } = null!;

        public DateTime DateOfBirth { get; set; }

        public string FullName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public string Address { get; set; } = null!;

        public CustomerType CustomerType { get; set; }

        // Navigation Property
        public List<CustomerAccount> CustomerAccounts { get; set; } = new();
    }
}
