using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BiluthyrningAB.Models.ViewModels
{
    public class CustomerDetailsVM
    {
        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Ssn { get; set; }
      

        public OrderVM[] Orders { get; set; }
        public HistoryLogVM[] History { get; set; }
    }
}
