using BiluthyrningAB.Models.Entities;
using Microsoft.AspNetCore.Mvc;
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

        [Remote("IsRegistered", "Customers", HttpMethod = "POST",ErrorMessage ="Kunden är ej registrerad vänligen skapa en ny kund")]
        [StringLength(12, ErrorMessage ="PersonNR måste anges i rätt format: 195510129999", MinimumLength = 12)]
        public string Ssn { get; set; }
        public int CustomerId { get; set; }
        public int CarId { get; set; }
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
        public Customers Customer { get; set; }
        public CarVM[] Cars { get; set; }

        public Cars Car { get; set; }
    }
}
