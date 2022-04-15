using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyCheck.Models
{
    public class DebtType
    {
        public long? DebtId { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public long? PurchaseId { get; set; }
        public DebtorType Debtor { get; set; }
    }

}
