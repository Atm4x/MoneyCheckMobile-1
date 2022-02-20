using MoneyCheck.Helpers;
using MoneyCheck.Models;
using System;
using System.Linq;
using System.Text.Json;
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
            if (Connectivity.NetworkAccess == NetworkAccess.Internet || Connectivity.NetworkAccess == NetworkAccess.ConstrainedInternet)
            {
                var response = App.Categories.Count > 0 ? null : Methods.Methods.GetCategories();

                if (response != null)
                {
                    if (response.statusCode == System.Net.HttpStatusCode.OK)
                    {
                        var result = JsonSerializer.Deserialize<Models.Category[]>(response.result, new JsonSerializerOptions()
                        {
                            PropertyNameCaseInsensitive = true
                        }).ToList();

                        App.Categories = result;

                        Category.ItemsSource = result;
                    }
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

        private void AddPurchase(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(Amount.Text))
            {
                DisplayAlert("Не заполнены данные", "Не указана цена", "Ок");
                return;
            }
            if (Category.SelectedItem == null)
            {
                DisplayAlert("Не заполнены данные", "Не выбрана категория", "Ок");
                return;
            }
            decimal amount = 0.0m;

            if(!decimal.TryParse(Amount.Text, out amount))
            {
                DisplayAlert("Не заполнены данные", "Неправильно указана цена", "Ок");
                return;
            }
            if(amount <= 0.0m)
            {
                DisplayAlert("Некорректные данные", "цена не может быть отрицательной", "Ок");
                return;
            }
            if (amount >= decimal.MaxValue)
            {
                DisplayAlert("Некорректные данные", "цена не может быть настолько большой", "Ок");
                return;
            }

            Purchase purchase = new Purchase();

            var selecteditem = ((Models.Category)Category.SelectedItem);

            purchase.Amount = amount;
            purchase.BoughtAt = DateTime.Now;
            purchase.CategoryId = selecteditem.Id;
            purchase.CategoryName = selecteditem.Name;

            bool local = false;

            if (Connectivity.NetworkAccess == NetworkAccess.Internet || Connectivity.NetworkAccess == NetworkAccess.ConstrainedInternet)
            {
                var response = Methods.Methods.AddPurchase(purchase);

                if (response.statusCode == System.Net.HttpStatusCode.OK)
                {
                    ((GeneralPage)App.ListPages.FirstOrDefault(x => x is GeneralPage)).Refresh();
                    local = false;
                }
                else
                {
                    DisplayAlert("Ошибка при добавлении Purchase", "понял да?", "Ок");
                    return;
                }
                if (!BackupHelper.RewriteBackup(App.BackupFilePath))
                {
                    DisplayAlert("Ошибка при добавлении", "Неудача при переписывании Backup файла", "ОК");
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
                ((GeneralPage)App.ListPages.FirstOrDefault(x => x is GeneralPage)).Refresh(true);
            }

            Navigation.PopModalAsync();
        }

        private void GoBackClick(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}