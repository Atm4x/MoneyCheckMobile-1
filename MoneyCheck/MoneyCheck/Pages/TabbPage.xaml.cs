using MoneyCheck.Helpers;
using MoneyCheck.Methods;
using MoneyCheck.Models;
using System;
using System.Threading.Tasks;
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


        public async void CheckToken()
        {
            if (App.Data == null)
            {
                await DisplayAlert("Ошибка", "Токен не актуален", "Назад");
                await SendToAuthPage("");
            }
            if (Connectivity.NetworkAccess == NetworkAccess.Internet || Connectivity.NetworkAccess == NetworkAccess.ConstrainedInternet)
            {
                if ((await Requests.GetStatus()).statusCode == System.Net.HttpStatusCode.OK)
                {
                    await General.RefreshFromLocal();
                    await General.Refresh();
                    CurrentPageChanged += CurrentPageHasChanged;
                    DisplayTitle.Text = "Главная";
                }
                else
                {
                    await General.RefreshFromLocal(true);
                    DataHelper.ClearToken();
                    App.Data = DataHelper.GetData();
                    await DisplayAlert("Токен истёк", "Токен не актуален", "Назад");
                    await SendToAuthPage("");
                }
            }
        }
        private async Task SendToAuthPage(string login) => await Navigation.PushModalAsync(new Pages.AuthPage(login));

        private void CurrentPageHasChanged(object sender, EventArgs e)
        {
            DisplayTitle.Text = CurrentPage.Title;
        }

    }
}