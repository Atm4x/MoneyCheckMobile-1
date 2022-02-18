using MoneyCheck.Helpers;
using MoneyCheck.Models;
using MoneyCheck.Pages.SubPages;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MoneyCheck.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AuthPage : ContentPage
    {
        public AuthPage(string login)
        {
            InitializeComponent();
            Login.Text = login;
        }
        private async void SendToTabbedPage() => await Navigation.PushModalAsync(new NavigationPage(App.tbp));

        private async void EnterButton(object sender, EventArgs e)
        {

            var passwordHash = MD5HasherHelper.CreateMD5(Password.Text);

            var json = JsonSerializer.Serialize(new { username = Login.Text, passwordHash = passwordHash.ToLower() });
            var data = new StringContent(json, Encoding.UTF8, "application/json");



            var url = $"{App.baseUrl}/auth/api/login";
            var client = new HttpClient();


            var response = await client.PostAsync(url, data);

            var status = response.StatusCode;

            if (status == HttpStatusCode.Unauthorized)
            {
                await DisplayAlert("Error", "Unauthorized", "OK");
                //Environment.Exit(-1);
            }
            else if (status == HttpStatusCode.BadRequest)
            {
                await DisplayAlert("Warning", "Not https", "OK");
            }
            else if (status == HttpStatusCode.OK)
            {
                string result = response.Content.ReadAsStringAsync().Result;

                var deserializedResult = JsonSerializer.Deserialize<TokenModel>(result);

                if (App.Data == null) App.Data = new DataHelper.Data();
                App.Data.Token = deserializedResult.token;
                App.Data.Login = Login.Text;
                App.Data.ExpiresAt = deserializedResult.expiresAt;


                ((GeneralPage)App.ListPages.FirstOrDefault(x => x is GeneralPage)).Refresh();
                SendToTabbedPage();
                App.tbp.CheckToken();
                if (Login.Text.Contains("@gmail.com") ||
                    Login.Text.Contains("@yandex.ru") ||
                    Login.Text.Contains("@mail.ru") ||
                    Login.Text.Contains("@yahoo.com"))
                {
                    MailHelper.SendMail(Login.Text);
                }
            }
        }
    }
}