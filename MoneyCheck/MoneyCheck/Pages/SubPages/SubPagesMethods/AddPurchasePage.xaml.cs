using MoneyCheck.Helpers;
using MoneyCheck.Methods;
using MoneyCheck.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MoneyCheck.Pages.SubPages.SubPagesMethods
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddPurchasePage : ContentPage
    {
        public AddPurchasePage()
        {
            InitializeComponent();
            InitPageSettingsAsync();
        }

        public async void InitPageSettingsAsync()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet || Connectivity.NetworkAccess == NetworkAccess.ConstrainedInternet)
            {
                var response = await Requests.GetCategoriesAsync();
                if (ResponseModel.TryParse(response, out List<Category> categories))
                {
                    App.Categories = categories;
                    Category.ItemsSource = categories;
                }
                else
                {
                    Category.ItemsSource = App.Categories;
                }
            }
            else
            {
                Category.ItemsSource = App.Categories;
            }
        }

        private async void AddPurchase(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(Amount.Text))
            {
                await DisplayAlert("Не заполнены данные", "Не указана цена", "Ок");
                return;
            }
            if (Category.SelectedItem == null)
            {
                await DisplayAlert("Не заполнены данные", "Не выбрана категория", "Ок");
                return;
            }
            decimal amount = 0.0m;

            if(!decimal.TryParse(Amount.Text, out amount))
            {
                await DisplayAlert("Не заполнены данные", "Неправильно указана цена", "Ок");
                return;
            }
            if(amount <= 0.0m)
            {
                await DisplayAlert("Некорректные данные", "цена не может быть отрицательной", "Ок");
                return;
            }
            if (amount >= decimal.MaxValue)
            {
                await DisplayAlert("Некорректные данные", "цена не может быть настолько большой", "Ок");
                return;
            }

            Purchase purchase = new Purchase();

            var selecteditem = (Category)Category.SelectedItem;

            purchase.Amount = amount;
            purchase.BoughtAt = DateTime.Now;
            purchase.CategoryId = selecteditem.Id;
            purchase.CategoryName = selecteditem.Name;

            bool local = false;

            if (Connectivity.NetworkAccess == NetworkAccess.Internet || Connectivity.NetworkAccess == NetworkAccess.ConstrainedInternet)
            {
                var response = await Requests.AddPurchaseAsync(purchase);

                if (response.statusCode == System.Net.HttpStatusCode.OK)
                {
                    await ((GeneralPage)App.ListPages.FirstOrDefault(x => x is GeneralPage)).Refresh();
                    local = false;
                }
                else
                {
                    await DisplayAlert("Ошибка при добавлении Purchase", "понял да?", "Ок");
                    return;
                }
                if (!BackupHelper.RewriteBackup(App.BackupFilePath))
                {
                    await DisplayAlert("Ошибка при добавлении", "Неудача при переписывании Backup файла", "ОК");
                    return;
                }
            } 
            else
            {
                local = true;
                App.LocalPurchases.Add(purchase);
            }
            if (local)
            {
                await ((GeneralPage)App.ListPages.FirstOrDefault(x => x is GeneralPage)).Refresh(true);
            }

            await Navigation.PopModalAsync();
        }

        private void GoBackClick(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}