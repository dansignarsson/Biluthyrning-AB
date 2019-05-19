using System;
using System.Collections.Generic;

namespace BiluthyrningAB.Models.Entities
{
    public partial class Orders
    {
        public int BookingNr { get; set; }
        public int CustomerId { get; set; }
        public DateTime BookingDate { get; set; }
        public string CarType { get; set; }
        public string RegNr { get; set; }
        public DateTime? PickUpDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public int CurrentMileage { get; set; }
        public int MileageOnReturn { get; set; }
        public int DrivenMiles { get; set; }
        public bool? IsReturned { get; set; }

        public Customers Customer { get; set; }
    }
}
