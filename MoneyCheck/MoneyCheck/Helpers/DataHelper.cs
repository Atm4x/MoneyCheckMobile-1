using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace MoneyCheck.Helpers
{
    public partial class DataHelper
    {
        public class Data
        {
            private string _token;
            private string _login;
            private DateTime? _expiresAt;
            public string Token
            {
                get { return _token; }
                set
                {
                    _token = value;
                    UpdateData(Token: value, Login: null, ExpiresAt: default);
                }
            }
            public string Login
            {
                get { return _login; }
                set
                {
                    _login = value;
                    UpdateData(Login: value, Token: null, ExpiresAt: default);
                }
            }
            public DateTime? ExpiresAt
            {
                get { return _expiresAt; }
                set
                {
                    _expiresAt = value;
                    UpdateData(ExpiresAt: value, Login: null, Token: null);
                }
            }
        }

        public static Data GetData()
        {
            Data def = new Data() { ExpiresAt = null, Login = null, Token = null };
            Data data  = new Data() { ExpiresAt = null, Login = null, Token = null};
            if (Preferences.ContainsKey("user_token"))
            {
                data.Token = Preferences.Get("user_token", "");
            }
                
            if(Preferences.ContainsKey("user_login")) 
            {
                data.Login = Preferences.Get("user_login", "");
            }

           if (Preferences.ContainsKey("user_tokenExpire"))
            {
                data.ExpiresAt = Preferences.Get("user_tokenExpire", default(DateTime));
            }
            if (def.ExpiresAt == data.ExpiresAt && def.Login == data.Login && def.Token == data.Token) return null;
            return data;
        }

        public static void UpdateData(string Login, string Token, DateTime? ExpiresAt)
        {

            if (!String.IsNullOrWhiteSpace(Login))
            {
                Preferences.Set("user_login", Login);
                return;
            }
            if (!String.IsNullOrWhiteSpace(Token))
            {
                Preferences.Set("user_token", Token);
                return;
            }
            if((ExpiresAt != default) && (ExpiresAt != null))
            {
                Preferences.Set("user_tokenExpire", ExpiresAt.Value);
                return;
            }
        }

        public static void ClearAll()
        {
            Preferences.Set("user_token", "");
            Preferences.Set("user_login", "");
            Preferences.Set("user_tokenExpire", default(DateTime));
        }

        public static void ClearToken()
        {
            Preferences.Set("user_token", "");
            Preferences.Set("user_tokenExpire", default(DateTime));
        }
    }
}
