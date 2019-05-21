using System;
using System.Collections.Generic;

namespace BiluthyrningAB.Models.Entities
{
    public partial class HistoryLog
    {
        public int Id { get; set; }
        public int? OrderId { get; set; }
        public int? CustomerId { get; set; }
        public int? CarId { get; set; }
        public string Activity { get; set; }

        public Cars Car { get; set; }
        public Customers Customer { get; set; }
        public Orders Order { get; set; }
    }
}
