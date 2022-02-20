using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyCheck.Models
{
    public class AmountChangedEventArgs
    {
        public decimal OldBalance { get; set; }
        public decimal NewBalance { get; set; }
        public decimal Count { get; set; }
    }

    public class UserBalance
    {
        public UserBalance Clone()
        {
            return new UserBalance(_Balance, _TodaySpent, _FutureCash);
        }

        internal delegate void AmountHandler(AmountChangedEventArgs amount);
        internal event AmountHandler AmountChanged;

        private decimal _Balance = default;
        private decimal _TodaySpent = default;
        private decimal _FutureCash = default;

        public UserBalance userBalance { get; set; }

        public decimal Balance { get => _Balance; set
            {
                UserBalance balance = Clone();
                _Balance = value;
                AmountChanged?.Invoke(new AmountChangedEventArgs() { OldBalance = balance.Balance, NewBalance = _Balance, Count = _Balance-balance.Balance });
            } 
        }

        public decimal TodaySpent { get => _TodaySpent; set
            {
                _TodaySpent = value;
            }
        }

        public decimal FutureCash { get => _FutureCash; set
            {
                _FutureCash = value;
            }
        }

        public UserBalance(decimal balance = default, decimal todaySpent = default, decimal futureCash = default)
        {
            _Balance = balance;
            _TodaySpent = todaySpent;
            _FutureCash = futureCash;
        }

        public void SetBalance(UserBalance balance)
        {
            FutureCash = balance.FutureCash;
            TodaySpent = balance.TodaySpent;
            Balance = balance.Balance;
        }
    }
}
