using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BiluthyrningAB.Models.ViewModels
{
    public class CarVM
    {
        public int Id { get; set; }

        [StringLength(6, ErrorMessage = "Mata in ett giltigt RegNR ex. ABC123"), MinLength(6)]
        public string RegNr { get; set; }
        public string CarType { get; set; }
        public int Mileage { get; set; }
        public bool? IsAvailable { get; set; }
        public int TimesRented { get; set; }
        public bool ToBeCleaned { get; set; }
        public bool ToBeRemoved { get; set; }
        public bool NeedService { get; set; }
        public bool Success { get; set; }
        public HistoryLogVM[] History { get; set; }
    }
}
