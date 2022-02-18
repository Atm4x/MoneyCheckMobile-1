using MoneyCheck.Pages.SubPages;
using System;
using System.Linq;
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
            else
            {
                CurrentPageChanged += CurrentPageHasChanged;
                DisplayTitle.Text = "Главная";
            }
        }
        private async void SendToAuthPage(string login) => await Navigation.PushModalAsync(new Pages.AuthPage(login));

        private void CurrentPageHasChanged(object sender, EventArgs e) {
            DisplayTitle.Text = CurrentPage.Title; 
        }

    }
}