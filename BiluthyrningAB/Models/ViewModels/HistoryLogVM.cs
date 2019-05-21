using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BiluthyrningAB.Models.ViewModels
{
    public class HistoryLogVM
    {
        public int? OrderId { get; set; }
        public int? CustomerId { get; set; }
        public int? CarId { get; set; }
        public string Activity { get; set; }
    }
}
