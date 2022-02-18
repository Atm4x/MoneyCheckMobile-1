using MoneyCheck.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MoneyCheck.Pages.SubPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GeneralPage : ContentPage
    {
        public Label NotFoundLabel;
        public Grid Purchase(decimal amount, Category category)
        {
            Grid grid = new Grid();
            bool isPurchase = category.Name != "Зачисление";
            Label transaction = new Label() 
            { 
                Text = !isPurchase ? $"Зачисление на {amount.ToString("f")} руб." : $"Покупка на {amount.ToString("f")} руб.",
                TextColor = isPurchase ? Color.FromHex("#DE3842") : Color.FromHex("#2EC321"),
                FontSize = 18,
                HorizontalOptions = LayoutOptions.Start,
            };
            grid.Children.Add(transaction);
            return grid;
        }
        public GeneralPage()
        {
            InitializeComponent();
        }


        private void AddPurchase(object sender, EventArgs e)
        {
            GoAddPurchase();
        }

        public async void GoAddPurchase() => await Navigation.PushModalAsync(new Pages.SubPages.SubPagesMethods.AddPurchasePage());

        public void Refresh()
        {
            var purchaseResponse = Methods.Methods.GetPurchasesResponse();
            var code = purchaseResponse.statusCode;
            if (code == HttpStatusCode.OK)
            {
                var result = purchaseResponse.result;
                var purchases = JsonSerializer.Deserialize<Purchase[]>(result, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }).ToList();
                if (purchases.Count != 0)
                    App.Transactions = purchases;
            }


            var balanceResponse = Methods.Methods.GetBalanceResponse();
            if (code == HttpStatusCode.OK)
            {
                var result = balanceResponse.result;
                var balance = JsonSerializer.Deserialize<UserBalance>(result, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                Balance.Text = (balance.Balance.ToString("f") ?? "0") + " рублей";
                Predication.Text = (balance.FutureCash.ToString("f") ?? "0") + " рублей";
                Spent.Text = (balance.TodaySpent.ToString("f") ?? "0") + " рублей";
            }


            MyDebtors.Children.Clear();
            MyTransactions.Children.Clear();
            if (App.Debtors.Count <= 0)
            {
                NotFoundLabel = new Label()
                {
                    TextColor = Color.FromHex("#B122F4"),
                    FontSize = 20,
                    HorizontalOptions = LayoutOptions.Center,
                    Text = "Здесь ничего нет"
                };

                MyDebtors.Children.Add(NotFoundLabel);
            } else
            {
                
            }
            if (App.Transactions.Count <= 0)
            {
                NotFoundLabel = new Label()
                {
                    TextColor = Color.FromHex("#B122F4"),
                    FontSize = 20,
                    HorizontalOptions = LayoutOptions.Center,
                    Text = "Здесь ничего нет"

                };

                MyTransactions.Children.Add(NotFoundLabel);
            }
            else
            {
                foreach (Purchase purchase in App.Transactions)
                {
                    //Purchase = new Label()
                    //{
                    //    TextColor = Color.FromHex("#DE3842"),
                    //    FontSize = 18,
                    //    HorizontalOptions = LayoutOptions.Start,
                    //    Text = $"Покупка на {purchase.Amount.ToString("f")} руб."
                    //};
                    var a = Purchase(purchase.Amount, Methods.Methods.GetCategory(purchase.CategoryId));
                    MyTransactions.Children.Add(a);
                }
            }
        }

    }
}