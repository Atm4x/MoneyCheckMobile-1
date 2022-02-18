using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyCheck.Models
{
    public class TokenModel
    {
        public string token { get; set; }
        public DateTime expiresAt { get; set; }
    }
}
