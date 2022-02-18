using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

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
            var response = Methods.Methods.GetCategories();

            if(response.statusCode == System.Net.HttpStatusCode.OK)
            {
                var result = JsonSerializer.Deserialize<Models.Category[]>(response.result, new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                }).ToList();

               Category.ItemsSource = result;
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

            Models.Purchase purchase = new Models.Purchase();

            var selecteditem = ((Models.Category)Category.SelectedItem);

            purchase.Amount = amount;
            purchase.BoughtAt = DateTime.Now;
            purchase.CategoryId = selecteditem.Id;
            purchase.CategoryName = selecteditem.Name; 

            var response = Methods.Methods.AddPurchase(purchase);

            if (response.statusCode == System.Net.HttpStatusCode.OK)
            {
                ((GeneralPage)App.ListPages.FirstOrDefault(x => x is GeneralPage)).Refresh();
                Navigation.PopModalAsync();
            }
            else
            {
                DisplayAlert("Ошибка при добавлении Purchase", "понял да?", "Ок");
                return;
            }
        }
    }
}