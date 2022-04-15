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
    public partial class DebtorsViewPage : ContentPage
    {
        public DebtorsViewPage()
        {
            InitializeComponent();
        }

        

        protected override async void OnAppearing()
        {
            await InitPageAsync();
            base.OnAppearing();
        }
        public async Task InitPageAsync()
        {
            var mydebtors = App.Debtors;
            mydebtors.AddRange(App.LocalDebtors);

            var response = await Requests.GetDebtorsAsync();

            if (Connectivity.NetworkAccess.HasInternet()) {
                if (ResponseModel.TryParse(response, out List<DebtorType> debtors))
                {
                    mydebtors = debtors;
                }
            }

            ListOfDebtors.ItemsSource = mydebtors;
        }
    }
}