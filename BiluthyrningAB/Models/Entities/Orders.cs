using System;
using System.Collections.Generic;

namespace BiluthyrningAB.Models.Entities
{
    public partial class Orders
    {
        public Orders()
        {
            HistoryLog = new HashSet<HistoryLog>();
        }

        public int BookingNr { get; set; }
        public int CustomerId { get; set; }
        public int? CarId { get; set; }
        public DateTime BookingDate { get; set; }
        public DateTime? PickUpDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public int CurrentMileage { get; set; }
        public int MileageOnReturn { get; set; }
        public int DrivenMiles { get; set; }
        public bool IsReturned { get; set; }

        public Cars Car { get; set; }
        public Customers Customer { get; set; }
        public ICollection<HistoryLog> HistoryLog { get; set; }
    }
}
