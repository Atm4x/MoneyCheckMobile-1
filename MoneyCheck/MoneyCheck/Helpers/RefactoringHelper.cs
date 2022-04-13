using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace MoneyCheck.Helpers
{
    public static class RefactoringHelper
    {
        public static bool HasInternet(this NetworkAccess access)
        {
            if (access == NetworkAccess.Internet || access == NetworkAccess.ConstrainedInternet)
                return true;
            else return false;
        }
    }
}
