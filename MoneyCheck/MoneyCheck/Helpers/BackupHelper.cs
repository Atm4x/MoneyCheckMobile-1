using MoneyCheck.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace MoneyCheck.Helpers
{
    public class BackupHelper
    {
        public static bool RewriteBackup(string path)
        {
            try
            {
                File.WriteAllText(App.BackupFilePath, JsonSerializer.Serialize(new BackupModel() { balance = App.UBalance, purchases = App.Transactions, categories = App.Categories }));
                return true;
            }
            catch 
            {
                return false;
            }
        }

        public static BackupModel ReadBackup(string path)
        {
            var result = File.ReadAllText(App.BackupFilePath);
            if (!String.IsNullOrWhiteSpace(result))
            {
                var backupModel = JsonSerializer.Deserialize<BackupModel>(result, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                return backupModel;
            }
            return null;
        }
    }
}
