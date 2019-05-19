using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BiluthyrningAB.Models.ViewModels
{
    public class RecieptVM
    {
        public int BookingNr { get; set; }
        public string CarType { get; set; }
        public string RegNr { get; set; }
        public DateTime PickUpDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public int DaysRented { get; set; }
        public int MileageOnPickup { get; set; }
        public int MileageOnReturn { get; set; }
        public int DrivenMiles { get; set; }
        public double TotalPrice { get; set; }
    }
}
