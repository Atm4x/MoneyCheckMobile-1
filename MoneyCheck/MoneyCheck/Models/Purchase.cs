using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyCheck.Models
{
    public class Purchase
    {
        public long? Id { get; set; }
        public DateTime BoughtAt { get; set; }
        public decimal Amount { get; set; }
        public long CategoryId { get; set; }
        public string CategoryName { get; set; }
        public decimal? Longitude { get; set; }
        public decimal? Latitude { get; set; }
    }
}
