using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;

namespace MoneyCheck.Models
{
    public class DebtorType
    {
        public string Name { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public long? Id { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IEnumerable<DebtType> Debts { get; set; }

        [JsonIgnore]
        public decimal FullAmount => Debts != null ? Debts.ToList().Sum(x => x.Amount) : 0;
        
    }
}
