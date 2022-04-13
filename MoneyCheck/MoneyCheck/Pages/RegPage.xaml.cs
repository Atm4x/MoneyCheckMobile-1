using MoneyCheck.Helpers;
using MoneyCheck.Methods;
using MoneyCheck.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MoneyCheck.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegPage : ContentPage
    {
        public RegPage()
        {
            InitializeComponent();
        }

        private void SendToTabbedPage() => App.Current.MainPage = new NavigationPage(App.Tbp);

        private async void RegistrationClicked(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(Login.Text))
            {
                await DisplayAlert("Не заполнены данные", "Не заполнен логин", "Ок");
                return;
            }
            if (String.IsNullOrWhiteSpace(Password.Text))
            {
                await DisplayAlert("Не заполнены данные", "Не заполнен пароль", "Ок");
                return;
            }
            if (String.IsNullOrWhiteSpace(RepeatPassword.Text))
            {
                await DisplayAlert("Не заполнены данные", "Не заполнен повторный пароль", "Ок");
                return;
            }

            if (RepeatPassword.Text != Password.Text)
            {
                await DisplayAlert("Внимание", "Пароли не совпадают", "Ок");
                return;
            }
            if (Connectivity.NetworkAccess == NetworkAccess.Internet || Connectivity.NetworkAccess == NetworkAccess.ConstrainedInternet)
            {


                var response = await Requests.LogUpAsync(new LogUpModel { Username = Login.Text ?? "", Password = Password.Text ?? "" });

                var status = response.statusCode;

                if (status == System.Net.HttpStatusCode.BadRequest)
                {
                    await DisplayAlert("Ошибка", "Пользователь с таким логином уже существует", "OK");
                    return;
                }
                else if (status == System.Net.HttpStatusCode.OK) {

                    string result = response.result;

                    var deserializedResult = JsonSerializer.Deserialize<TokenModel>(result);

                    App.Data = new DataHelper.Data();
                    App.Data.Token = deserializedResult.token;
                    App.Data.Login = Login.Text;
                    App.Data.ExpiresAt = deserializedResult.expiresAt;

                    var categoriesResponse = await Requests.GetCategoriesAsync();
                    if (ResponseModel.TryParse(categoriesResponse, out List<Category> categories))
                    {
                        App.Categories = categories;
                    }

                    SendToTabbedPage();
                }
            }
            else
            {
                await DisplayAlert("Ошибка", "Требуется подключение к интернету", "OK");
            }
        }
    }
}