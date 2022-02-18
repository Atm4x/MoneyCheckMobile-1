using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyCheck.Models
{
    public class UserBalance
    {
        public decimal Balance { get; set; }
        public decimal TodaySpent { get; set; }
        public decimal FutureCash { get; set; }
    }
}
