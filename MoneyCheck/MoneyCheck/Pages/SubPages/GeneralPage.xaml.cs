using MoneyCheck.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xamarin.Essentials;
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
            App.UBalance.AmountChanged += BalanceChanged;
        }

        private void BalanceChanged(UserBalance.AmountChangedEventArgs amount)
        {

        }

        private void AddPurchase(object sender, EventArgs e)
        {
            GoAddPurchase();
        }

        public async void GoAddPurchase() => await Navigation.PushModalAsync(new Pages.SubPages.SubPagesMethods.AddPurchasePage());

        public void RefreshFromLocal(bool local = false)
        {
            if (App.LocalPurchases.Count > 0)
            {
                foreach (var purchase in App.LocalPurchases)
                {
                    if(!local) Methods.Methods.AddPurchase(purchase);
                    App.Transactions.Add(purchase);
                }
                if (!local) App.LocalPurchases.Clear();
            }
        }
        public void Refresh(bool useLocal = false)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet || Connectivity.NetworkAccess == NetworkAccess.ConstrainedInternet) {
                var status = Methods.Methods.GetStatus();
                if (!useLocal && status.statusCode == HttpStatusCode.OK)
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
                        App.UBalance.SetBalance(balance);
                    }

                    try
                    {
                        File.WriteAllText(App.BackupFilePath, JsonSerializer.Serialize(new BackupModel() { balance = App.UBalance, purchases = App.Transactions, categories = App.Categories }));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Неудача при переписывании Backup файла: " + ex.Message);
                    }
                }

                RefreshFromLocal(true);
            } else
            {
                var result = File.ReadAllText(App.BackupFilePath);
                if (!String.IsNullOrWhiteSpace(result))
                {
                    var backupModel = JsonSerializer.Deserialize<BackupModel>(result, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (backupModel.purchases?.Count != 0)
                        App.Transactions = backupModel.purchases;
                    if (backupModel.balance != null)
                        App.UBalance = backupModel.balance;
                    if (backupModel.categories?.Count != 0)
                        App.Categories = backupModel.categories;
                }
            }


            Balance.Text = (App.UBalance.Balance.ToString("f") ?? "0") + " рублей";
            Predication.Text = (App.UBalance.FutureCash.ToString("f") ?? "0") + " рублей";
            Spent.Text = (App.UBalance.TodaySpent.ToString("f") ?? "0") + " рублей";

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
                var list = App.Transactions.OrderByDescending(x => x.BoughtAt).Take(5).ToList();
                if (Connectivity.NetworkAccess == NetworkAccess.Internet || Connectivity.NetworkAccess == NetworkAccess.ConstrainedInternet)
                {
                    foreach (Purchase purchase in list)
                    {
                        var purchases = Purchase(purchase.Amount, Methods.Methods.GetCategory(purchase.CategoryId));
                        MyTransactions.Children.Add(purchases);
                    }
                } 
                else
                {
                    foreach (Purchase purchase in list)
                    {
                        var purchases = Purchase(purchase.Amount, App.Categories.Find(x => x.Id == purchase.CategoryId));
                        MyTransactions.Children.Add(purchases);
                    }
                }
                if(App.Transactions.Count>5)
                {
                    Frame showMore = new Frame();
                    showMore.Style = (Style)Application.Current.Resources["funcButton"];
                    showMore.HorizontalOptions = LayoutOptions.Center;
                    showMore.VerticalOptions = LayoutOptions.Center;
                    showMore.CornerRadius = 3;
                    showMore.Padding = 2.5;
                    Button button = new Button();
                    button.Text = "Смотреть все";
                    button.FontSize = 20;
                    button.TextColor = Color.White;
                    button.FontFamily = "Verdana";
                    button.Background = Brush.Transparent;
                    button.TextTransform = TextTransform.None;
                    showMore.Content = button;
                    MyTransactions.Children.Add(showMore);
                }
            }
        }

    }
}