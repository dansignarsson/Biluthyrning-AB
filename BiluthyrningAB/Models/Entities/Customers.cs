using System;
using System.Collections.Generic;

namespace BiluthyrningAB.Models.Entities
{
    public partial class Customers
    {
        public Customers()
        {
            HistoryLog = new HashSet<HistoryLog>();
            Orders = new HashSet<Orders>();
        }

        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Ssn { get; set; }

        public ICollection<HistoryLog> HistoryLog { get; set; }
        public ICollection<Orders> Orders { get; set; }
    }
}
