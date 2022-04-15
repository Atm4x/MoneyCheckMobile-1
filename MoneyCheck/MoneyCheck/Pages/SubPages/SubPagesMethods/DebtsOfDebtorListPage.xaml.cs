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
    public partial class DebtsOfDebtorListPage : ContentPage
    {
        private DebtorType _debtor;

        public DebtsOfDebtorListPage(DebtorType debtor)
        {
            InitializeComponent();

            _debtor = debtor;
        }

        protected override void OnAppearing()
        {
            Refresh();
            base.OnAppearing();

        }

        public void Refresh()
        {
            if (Connectivity.NetworkAccess.HasInternet())
            {
                ListOfDebts.ItemsSource = _debtor.Debts;
            }
        }


        private async void AddDebt(object sender, EventArgs e)
        {
            await App.Tbp.Navigation.PushAsync(new AddDebtPage(_debtor, this));
        }
    }
}