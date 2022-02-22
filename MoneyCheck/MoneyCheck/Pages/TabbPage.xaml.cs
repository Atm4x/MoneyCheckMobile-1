using MoneyCheck.Helpers;
using MoneyCheck.Methods;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MoneyCheck.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabbPage : TabbedPage
    {
        public TabbPage()
        {
            InitializeComponent();
        }

        public void CheckToken()
        {
            if (App.Data == null)
            {
                DisplayAlert("Ошибка", "Токен не актуален", "Назад");
                SendToAuthPage("");
            }
            if (Connectivity.NetworkAccess == NetworkAccess.Internet || Connectivity.NetworkAccess == NetworkAccess.ConstrainedInternet)
            {
                if (Requests.GetStatus().statusCode == System.Net.HttpStatusCode.OK)
                {
                    General.RefreshFromLocal();
                    General.Refresh();
                    CurrentPageChanged += CurrentPageHasChanged;
                    DisplayTitle.Text = "Главная";
                }
                else
                {
                    General.RefreshFromLocal(true);
                    DataHelper.ClearToken();
                    App.Data = DataHelper.GetData();
                    DisplayAlert("Токен истёк", "Токен не актуален", "Назад");
                    SendToAuthPage("");
                }
            }
        }
        private async void SendToAuthPage(string login) => await Navigation.PushModalAsync(new Pages.AuthPage(login));

        private void CurrentPageHasChanged(object sender, EventArgs e)
        {
            DisplayTitle.Text = CurrentPage.Title;
        }

    }
}