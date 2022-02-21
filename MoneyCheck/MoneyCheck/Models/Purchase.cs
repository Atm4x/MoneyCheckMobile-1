using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

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

        [JsonIgnore]
        public bool IsPurchase => CategoryName != "Зачисление";

        [JsonIgnore]
        public string PurchasePhrase => IsPurchase ? $"Покупка на {Amount.ToString("f")} руб." : $"Зачисление на {Amount.ToString("f")} руб.";
    }
}
