﻿using System;
using System.Collections.Generic;

namespace BiluthyrningAB.Models.Entities
{
    public partial class Cars
    {
        public Cars()
        {
            HistoryLog = new HashSet<HistoryLog>();
            Orders = new HashSet<Orders>();
        }

        public int Id { get; set; }
        public string CarType { get; set; }
        public string RegNr { get; set; }
        public int Mileage { get; set; }
        public int TimesRented { get; set; }
        public bool IsAvailable { get; set; }
        public bool ToBeCleaned { get; set; }
        public bool ToBeRemoved { get; set; }
        public bool NeedService { get; set; }

        public ICollection<HistoryLog> HistoryLog { get; set; }
        public ICollection<Orders> Orders { get; set; }
    }
}
