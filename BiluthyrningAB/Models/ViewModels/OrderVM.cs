using BiluthyrningAB.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BiluthyrningAB.Models.ViewModels
{
    public class OrderVM
    {
        public int BookingNr { get; set; }
        public int CustomerId { get; set; }
        public string CarType { get; set; }

        [StringLength(6, ErrorMessage = "Mata in ett giltigt RegNR ex. ABC123"), MinLength(6)]
        public string RegNr { get; set; }

        [Range(typeof(DateTime), "1/5/2019", "1/1/2021", ErrorMessage = "Välj ett giltigt bokningsdatum 1/5/2019 - 1/1/2021")]
        public DateTime? PickUpDate { get; set; }

        public DateTime ReturnDate { get; set; }
        public int CurrentMileage { get; set; }
        public int MileageOnReturn { get; set; }
        public int DrivenMiles { get; set; }
        public bool IsReturned { get; set; }

        public CustomerVM[] Customers { get; set; }
    }
}
