using System;
using System.Collections.Generic;

namespace BiluthyrningAB.Models.Entities
{
    public partial class Customers
    {
        public Customers()
        {
            Orders = new HashSet<Orders>();
        }

        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Ssn { get; set; }

        public ICollection<Orders> Orders { get; set; }
    }
}
