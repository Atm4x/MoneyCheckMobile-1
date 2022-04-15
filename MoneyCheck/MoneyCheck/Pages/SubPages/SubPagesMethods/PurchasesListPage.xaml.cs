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
    public partial class PurchasesListPage : ContentPage
    {
        public PurchasesListPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            await InitializePageAsync();
            base.OnAppearing();
        }

        private async Task InitializePageAsync()
        {
            var fullList = App.Transactions.ToList();
            fullList.AddRange(App.LocalPurchases);

            if (Connectivity.NetworkAccess.HasInternet())
            {
                if (ResponseModel.TryParse(await Requests.GetPurchasesAsync(), out List<Purchase> purchases))
                {
                    fullList = purchases;
                }
            }

            ListOfTransactions.ItemsSource = fullList.OrderByDescending(x => x.BoughtAt);
        }

        private void GoBackClick(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}