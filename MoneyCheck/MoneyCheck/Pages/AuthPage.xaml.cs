using MoneyCheck.Helpers;
using MoneyCheck.Methods;
using MoneyCheck.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MoneyCheck.Pages
{
    public partial class AuthPage : ContentPage
    {
        public AuthPage(string login)
        {
            InitializeComponent();
            Login.Text = login;
        }
        // private async void SendToTabbedPage() => await Navigation.PushModalAsync(new NavigationPage(App.Tbp));
        private void SendToTabbedPage() => App.Current.MainPage = new NavigationPage(App.Tbp);

        private async void EnterButton(object sender, EventArgs e)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet || Connectivity.NetworkAccess == NetworkAccess.ConstrainedInternet)
            {
                var passwordHash = MD5HasherHelper.CreateMD5(Password.Text);

                //var json = JsonSerializer.Serialize(});
                //var data = new StringContent(json, Encoding.UTF8, "application/json");



                //var url = $"{App.BaseUrl}/auth/api/login";
                //var client = new HttpClient();


                var response = await Requests.LogIn(new LogInModel { Username = Login.Text, PasswordHash = passwordHash.ToLower() });

                var status = response.statusCode;

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
                    string result = response.result;

                    var deserializedResult = JsonSerializer.Deserialize<TokenModel>(result);

                    App.Data = new DataHelper.Data();
                    App.Data.Token = deserializedResult.token;
                    App.Data.Login = Login.Text;
                    App.Data.ExpiresAt = deserializedResult.expiresAt;

                    var categoriesResponse = await Requests.GetCategories();
                    if (ResponseModel.TryParse(categoriesResponse, out List<Category> categories))
                    {
                        App.Categories = categories;
                    }


                    SendToTabbedPage();
                    App.Tbp.CheckToken();
                    if (Login.Text.Contains("@gmail.com") ||
                        Login.Text.Contains("@yandex.ru") ||
                        Login.Text.Contains("@mail.ru") ||
                        Login.Text.Contains("@yahoo.com"))
                    {
                        MailHelper.SendMail(Login.Text);
                    }
                }
                else
                {
                    await DisplayAlert("Ошибка", "Требуется подключение к интернету", "OK");
                }
            }
        }
    }
}