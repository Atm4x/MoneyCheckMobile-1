using MoneyCheck.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var dialogResult = await DisplayAlert("Предупреждение","Вы уверены, что хотите выйти?","Да","Отмена");
            if(dialogResult)
            {
                DataHelper.ClearToken();
                App.Data = DataHelper.GetData();
                await App.Tbp.SendToAuthPage(App.Data?.Login);
            }
        }
    }
}