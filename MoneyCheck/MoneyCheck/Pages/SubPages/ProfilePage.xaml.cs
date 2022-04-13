using MoneyCheck.Helpers;
using MoneyCheck.Methods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MoneyCheck.Pages.SubPages
{
    public partial class ProfilePage : ContentPage
    {
        public ProfilePage()
        {
            InitializeComponent();
        }

        private async void Exit(object sender, EventArgs e)
        {
            var dialogResult = await DisplayAlert("Предупреждение", "Вы уверены, что хотите выйти?", "Да", "Отмена");
            if(dialogResult)
            {
                if (Connectivity.NetworkAccess.HasInternet()) await Requests.LogOutAsync();

                DataHelper.ClearToken();
                App.Data = DataHelper.GetData();
                BackupHelper.ClearBackup(App.BackupFilePath);
                await App.Tbp.SendToAuthPage(App.Data?.Login);
            }
        }
    }
}