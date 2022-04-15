using MoneyCheck.Helpers;
using MoneyCheck.Methods;
using MoneyCheck.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MoneyCheck.Pages.SubPages.SubPagesMethods
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddDebtorPage : ContentPage
    {
        public AddDebtorPage()
        {
            InitializeComponent();
        }

        private void GoBackClick(object sender, EventArgs e)
        {
            App.Tbp.Navigation.PopModalAsync();
        }

        private async void AddDebtor(object sender, EventArgs e)
        {
            if(String.IsNullOrWhiteSpace(Name.Text))
            {
                await DisplayAlert("Не заполнены данные", "Не указано имя", "Ок");
                return;
            }

            DebtorType debtor = new DebtorType();

            debtor.Debts = new List<DebtType>();
            debtor.Name = Name.Text;

            if (Connectivity.NetworkAccess.HasInternet())
            {
                var response = await Requests.AddDebtorAsync(debtor);

                if (response.statusCode == System.Net.HttpStatusCode.OK)
                {
                    await ((GeneralPage)App.ListPages.FirstOrDefault(x => x is GeneralPage)).Refresh();
                }
                else
                {
                    await DisplayAlert("Ошибка при добавлении Debtor", "понял да?", "Ок");
                    return;
                }

                await Navigation.PopModalAsync();
            } else
            {
                await DisplayAlert("Ошибка", "Нет подключения к интернету", "Ок");
                return;
            }
        }
    }
}