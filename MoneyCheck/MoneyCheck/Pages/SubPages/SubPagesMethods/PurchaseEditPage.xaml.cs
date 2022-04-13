using MoneyCheck.Helpers;
using MoneyCheck.Methods;
using MoneyCheck.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MoneyCheck.Pages.SubPages.SubPagesMethods
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PurchaseEditPage : ContentPage
    {
        private Purchase _currentPurchase;
        public PurchaseEditPage(Purchase purchase)
        {
            InitializePageAsync(purchase);
            InitializeComponent();
        }

        private async void InitializePageAsync(Purchase purchase)
        {
            _currentPurchase = purchase;
            List<Category> allCategories = App.Categories;
            if (Connectivity.NetworkAccess.HasInternet())
            {
                if (ResponseModel.TryParse(await Requests.GetCategoriesAsync(), out List<Category> categories))
                {
                    if (categories.Count != 0)
                        allCategories = categories;
                }
            }
            CurrentAmount.Text = purchase.Amount.ToString();
            CurrentCategory.ItemsSource = allCategories;
            CurrentCategory.SelectedItem = allCategories.FirstOrDefault(x => x.Id == purchase.CategoryId);
        }

        private async void RemoveClicked(object sender, EventArgs e)
        {
            var answer = await DisplayAlert("Вы уверены, что хотите удалить эту транзакцию?", "Предупреждение", "ОК", "Отмена");
            if(answer)
            {
                if (Connectivity.NetworkAccess.HasInternet())
                {
                    var response = await Requests.RemovePurchaseAsync(_currentPurchase);
                    if (response.statusCode == HttpStatusCode.OK)
                    {
                        await ((GeneralPage)App.ListPages.FirstOrDefault(x => x is GeneralPage)).Refresh();
                        await App.Tbp.Navigation.PopAsync();
                    }
                    else if (response.statusCode == HttpStatusCode.BadRequest)
                    {
                        await DisplayAlert("Данной транзакции не существует", "Ошибка", "ОК");
                        return;
                    }
                }
            }
        }

        private async void SaveClicked(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(CurrentAmount.Text))
            {
                await DisplayAlert("Не заполнены данные", "Не указана цена", "Ок");
                return;
            }
            if (CurrentCategory.SelectedItem == null)
            {
                await DisplayAlert("Не заполнены данные", "Не выбрана категория", "Ок");
                return;
            }
            decimal amount = 0.0m;

            if (!decimal.TryParse(CurrentAmount.Text, out amount))
            {
                await DisplayAlert("Не заполнены данные", "Неправильно указана цена", "Ок");
                return;
            }
            if (amount <= 0.0m)
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

            var selecteditem = (Category)CurrentCategory.SelectedItem;

            purchase.Id = _currentPurchase.Id;
            purchase.Amount = amount;
            purchase.BoughtAt = DateTime.Now;
            purchase.CategoryId = selecteditem.Id;
            purchase.CategoryName = selecteditem.Name;

            if (Connectivity.NetworkAccess.HasInternet())
            {
                var response = await Requests.EditPurchaseAsync(purchase);

                if(response.statusCode == HttpStatusCode.OK)
                {
                    await ((GeneralPage)App.ListPages.FirstOrDefault(x => x is GeneralPage)).Refresh();
                    await App.Tbp.Navigation.PopAsync();
                }
                else if (response.statusCode == HttpStatusCode.BadRequest)
                {
                    await DisplayAlert("Данной транзакции не существует", "Ошибка", "ОК");
                    return;
                }
            }
        }
    }
}