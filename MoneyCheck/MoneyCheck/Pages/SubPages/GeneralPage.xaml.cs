using MoneyCheck.Controls;
using MoneyCheck.Helpers;
using MoneyCheck.Methods;
using MoneyCheck.Models;
using MoneyCheck.Pages.SubPages.SubPagesMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        //public Grid Purchase(Purchase purchase, Category category)
        //{
        //    RowDefinition row()
        //    {
        //        RowDefinition rowDefinition = new RowDefinition();
        //        rowDefinition.Height = new GridLength(1, GridUnitType.Star);
        //        return rowDefinition;
        //    }
        //    decimal amount = purchase.Amount;
        //    Grid grid = new Grid();
        //    grid.RowDefinitions.Add(row());
        //    grid.RowDefinitions.Add(row());
        //    bool isPurchase = category.Name != "Зачисление";
        //    Label transaction = new Label()
        //    {
        //        Text = !isPurchase ? $"Зачисление на {amount.ToString("f")} руб." : $"Покупка на {amount.ToString("f")} руб.",
        //        TextColor = isPurchase ? Color.FromHex("#DE3842") : Color.FromHex("#2EC321"),
        //        FontSize = 16,
        //        HorizontalOptions = LayoutOptions.Start,
        //        VerticalOptions = LayoutOptions.End
        //    };
        //    Label categoryAndDate = new Label()
        //    {
        //        Text = $"{category.Name}, {purchase.BoughtAt.ToString("f")}",
        //        TextColor = Color.Gray,
        //        FontSize = 12,
        //        HorizontalOptions = LayoutOptions.Start,
        //        VerticalOptions = LayoutOptions.Start
        //    };

        //    grid.Children.Add(categoryAndDate);
        //    Grid.SetRow(categoryAndDate, 1);
        //    grid.Children.Add(transaction);
        //    return grid;
        //}

        public Frame Purchase(Purchase purchase)
        {
            StackLayoutControl stackLayout = new StackLayoutControl
            {
                BindingContext = purchase
            };
            return stackLayout;
        }
        public Frame Debtor(DebtorType debtor)
        {
            DebtorControl stackLayout = new DebtorControl
            {
                BindingContext = debtor
            };
            return stackLayout;
        }
        public GeneralPage()
        {
            InitializeComponent();
            App.UBalance.AmountChanged += BalanceChanged;
        }

        private void BalanceChanged(AmountChangedEventArgs amount)
        {
            //Дениc, это не удаляй >:(
        }

        private async void AddPurchase(object sender, EventArgs e)
        {
            await GoAddPurchase();
        }

        private async void AddDebtor(object sender, EventArgs e)
        {
            await GoAddDebtor();
        }

        public async Task GoAddDebtor() => await Navigation.PushModalAsync(new NavigationPage(new SubPagesMethods.AddDebtorPage()));
        public async Task GoAddPurchase() => await Navigation.PushModalAsync(new NavigationPage(new SubPagesMethods.AddPurchasePage()));

        public async Task RefreshFromLocal(bool local = false)
        {
            if (App.LocalPurchases.Count > 0)
            {
                foreach (var purchase in App.LocalPurchases)
                {
                    if (!local) await Requests.AddPurchaseAsync(purchase);
                    App.Transactions.Add(purchase);
                }
                if (!local) App.LocalPurchases.Clear();
            }
            if (App.LocalDebtors.Count > 0)
            {
                foreach (var debtor in App.LocalDebtors)
                {
                    if (!local) await Requests.AddDebtorAsync(debtor);
                    App.Debtors.Add(debtor);
                }
                if (!local) App.LocalPurchases.Clear();
            }
        }

        public void LocalBalanceRecount()
        {
            decimal balance = 0;
            decimal spentToday = 0;
            foreach (var purchase in App.Transactions)
            {
                balance += purchase?.CategoryName == "Зачисление" ? purchase.Amount : -purchase.Amount;
                spentToday += purchase?.CategoryName != "Зачисление" ? purchase.Amount : 0;
            }
            App.UBalance.Balance = balance;
            App.UBalance.TodaySpent = spentToday;
        }

        private async void Refreshing(object sender, EventArgs e)
        {
            await Refresh();
            ((RefreshView)sender).IsRefreshing = false;
        }

        public async Task Refresh(bool useLocal = false)
        {
            if (!useLocal && (Connectivity.NetworkAccess.HasInternet()))
            {
                var status = await Requests.GetStatusAsync();
                if (!useLocal && status.statusCode == HttpStatusCode.OK)
                {
                    var purchaseResponse = await Requests.GetPurchasesAsync();
                    if (ResponseModel.TryParse(purchaseResponse, out List<Purchase> purchases))
                    {
                        App.Transactions = purchases;
                    }

                    var debtorsResponse = await Requests.GetDebtorsAsync();
                    if (ResponseModel.TryParse(debtorsResponse, out List<DebtorType> debtors))
                    {
                        App.Debtors = debtors;
                    }

                    var categoriesResponse = await Requests.GetCategoriesAsync();
                    if (ResponseModel.TryParse(categoriesResponse, out List<Category> categories))
                    {
                        App.Categories = categories;
                    }

                    var balanceResponse = await Requests.GetBalanceAsync();
                    if (ResponseModel.TryParse(balanceResponse, out UserBalance balance))
                    {
                        App.UBalance.SetBalance(balance);
                    }
                    if (!BackupHelper.RewriteBackup(App.BackupFilePath))
                    {
                        Console.WriteLine("Неудача при переписывании Backup файла");
                    }
                }


                await RefreshFromLocal(true);

                Balance.Text = (App.UBalance?.Balance.ToString("f") ?? "0") + " рублей";
                Predication.Text = (App.UBalance?.FutureCash.ToString("f") ?? "0") + " рублей";
                Spent.Text = (App.UBalance?.TodaySpent.ToString("f") ?? "0") + " рублей";
            }
            else 
            {
                var backupModel = BackupHelper.ReadBackup(App.BackupFilePath);

                if (backupModel != null) {
                    if (backupModel?.purchases?.Count != 0)
                        App.Transactions = backupModel.purchases.ToList();
                    if (backupModel?.categories?.Count != 0)
                        App.Categories = backupModel.categories.ToList();

                    await RefreshFromLocal(true);
                    if (App.Transactions.Count <= backupModel?.purchases.Count)
                    {
                        if (backupModel?.balance != null)
                            App.UBalance = backupModel.balance;

                        Balance.Text = (App.UBalance?.Balance.ToString("f") ?? "0") + " рублей";
                        Predication.Text = (App.UBalance?.FutureCash.ToString("f") ?? "0") + " рублей";
                        Spent.Text = (App.UBalance?.TodaySpent.ToString("f") ?? "0") + " рублей";
                    }
                    else
                    {
                        LocalBalanceRecount();

                        Balance.Text = (App.UBalance?.Balance.ToString("f") ?? "0") + " рублей";
                        Predication.Text = "Данные не зафиксированы";
                        Spent.Text = (App.UBalance?.TodaySpent.ToString("f") ?? "0") + " рублей";
                    }
                } else
                {
                    Balance.Text = "0" + " рублей";
                    Predication.Text = "0" + " рублей";
                    Spent.Text = "0" + " рублей";
                }
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
            }
            else
            {
                var list = App.Debtors.OrderByDescending(x => x.Name).Take(5).ToList();

                foreach (DebtorType debtor in list)
                {
                    MyDebtors.Children.Add(Debtor(debtor));
                }

                if (App.Debtors.Count > 5)
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
                    button.Clicked += (sender, e) => { Navigation.PushAsync(new DebtorsViewPage()); };
                    showMore.Content = button;
                    MyDebtors.Children.Add(showMore);
                }
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

                foreach (Purchase purchase in list)
                {
                    MyTransactions.Children.Add(Purchase(purchase));
                }
                if (App.Transactions.Count > 5)
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
                    button.Clicked += (sender, e) => { Navigation.PushAsync(new PurchasesListPage()); };
                    showMore.Content = button;
                    MyTransactions.Children.Add(showMore);
                }
            }
        }

    }
}