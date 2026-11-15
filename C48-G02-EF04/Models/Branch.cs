using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace C48_G02_EF04.Models
{
    public class Branch
    {
        public string Code { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Address { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public int ManagerId { get; set; }

        public Manager Manager { get; set; } = null!;

        public List<Account> Accounts { get; set; } = new();
    }
}
