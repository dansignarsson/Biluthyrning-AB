using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BiluthyrningAB.Models.ViewModels
{
    public class CustomerVM
    {
        public int CustomerId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Mata in förnamn")]
        public string FirstName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Mata in efternamn")]
        public string LastName { get; set; }

        [StringLength(12, ErrorMessage = "PersonNR måste anges i rätt format: 195510129999 och innehålla {1} tecken.", MinimumLength = 12)]
        public string Ssn { get; set; }

        public OrderVM[] CustomerOrders { get; set; }
    }
}
