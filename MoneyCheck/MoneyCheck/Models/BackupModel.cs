﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyCheck.Models
{
    public class BackupModel
    {
        public List<Purchase> purchases { get; set; }
        public List<Category> categories { get; set; }
        public UserBalance balance { get; set; }
    }
}
