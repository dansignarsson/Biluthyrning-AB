using System;
using System.Collections.Generic;

namespace BiluthyrningAB.Models.Entities
{
    public partial class Cars
    {
        public Cars()
        {
            Orders = new HashSet<Orders>();
        }

        public int Id { get; set; }
        public string CarType { get; set; }
        public string RegNr { get; set; }
        public int Mileage { get; set; }
        public bool? IsAvailable { get; set; }

        public ICollection<Orders> Orders { get; set; }
    }
}
