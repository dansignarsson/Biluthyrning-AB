using BiluthyrningAB.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BiluthyrningAB.Models.Data
{
    public class AvailableCarsData
    {
        public int Id { get; set; }
        public DateTime PickUpDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public string RegNr { get; internal set; }
        public string CarType { get; internal set; }
        public CarVM[] AvailableCars { get; set; }
    }
}
