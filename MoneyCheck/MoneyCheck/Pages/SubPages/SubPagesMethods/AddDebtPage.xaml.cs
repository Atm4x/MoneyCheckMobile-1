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
    public partial class AddDebtPage : ContentPage
    {
        private DebtorType _debtor;
        private DebtsOfDebtorListPage _root;
        public AddDebtPage(DebtorType debtor, DebtsOfDebtorListPage root)
        {
            InitializeComponent();
            _debtor = debtor;
            _root = root;
        }

        private async void AddDebt(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(Amount.Text))
            {
                await DisplayAlert("Не заполнены данные", "Не указана цена", "Ок");
                return;
            }
            decimal amount = 0.0m;

            if (!decimal.TryParse(Amount.Text, out amount))
            {
                await DisplayAlert("Не заполнены данные", "Неправильно указана цена", "Ок");
                return;
            }
            if (amount <= 0.0m)
            {
                await DisplayAlert("Некорректные данные", "Цена не может быть отрицательной", "Ок");
                return;
            }
            if (amount >= decimal.MaxValue)
            {
                await DisplayAlert("Некорректные данные", "Цена не может быть настолько большой", "Ок");
                return;
            }

            DebtType debt = new DebtType();
            debt.Amount = amount;
            debt.Description = Description.Text ?? "";

            if (Connectivity.NetworkAccess.HasInternet())
            {
                var response = await Requests.AddDebtAsync(new { Amount = debt.Amount, Description = debt.Description, DebtorId = _debtor.Id});

                if (response.statusCode == System.Net.HttpStatusCode.OK)
                {
                    await ((GeneralPage)App.ListPages.FirstOrDefault(x => x is GeneralPage)).Refresh();
                    _root.Refresh();
                    await DisplayAlert("Успех", $"Долг для {_debtor.Name} добавлен", "Ок");
                }
                else
                {
                    await DisplayAlert("Ошибка при добавлении Debt", "понял да?", "Ок");
                    return;
                }
            } 
            else
            {
                await DisplayAlert("Ошибка", "Нет подключения к интернету", "Ок");
                return;
            }
        }
    }
}