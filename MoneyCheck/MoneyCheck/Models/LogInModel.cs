using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyCheck.Models
{
    public class LogInModel
    {
        public string Username { get; internal set; }
        public string PasswordHash { get; internal set; }
    }
}
