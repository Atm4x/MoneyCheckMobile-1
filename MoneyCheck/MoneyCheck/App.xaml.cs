using MoneyCheck.Helpers;
using MoneyCheck.Models;
using MoneyCheck.Pages;
using MoneyCheck.Pages.SubPages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MoneyCheck
{
    public partial class App : Application
    {
        public static string baseUrl = "https://moneycheck.gym1551.ru"; 
        public static List<Purchase> Transactions = new List<Purchase>();
        public static List<object> Debtors = new List<object>();
        public static DataHelper.Data Data;
        public static UserBalance UBalance = new UserBalance();
        public static TabbPage tbp;
        public static List<ContentPage> ListPages = new List<ContentPage>();
        public static string backupFilePath = String.Empty;


        public App()
        {
            InitializeComponent();
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NTY0NDI5QDMxMzkyZTM0MmUzMFU0eG9iMVp3NXdNMksyM0F2WEFHc0JFZmY4ajJxTGtvMWwxU0RncXBnMTQ9;NTY0NDMwQDMxMzkyZTM0MmUzMFY2ZVBDY291bU9naU41czh2UlRiZXI4RVVhdjB0Q2JiY1lYdjV5MEZGbGM9;NTY0NDMxQDMxMzkyZTM0MmUzMFlCdFhFTmN0TTB2dFgzWU51QUkxeXBISDZPRTBXNDd1OEcyWHdRdC9naWM9;NTY0NDMyQDMxMzkyZTM0MmUzMGtVR2xrZk95djJaNEpvaEx0ZE0zWmRkT1UrN3hKeVJxaEZIdGh6T2FCeTg9");


            tbp = new Pages.TabbPage();

            foreach (ContentPage page in tbp.Children)
            {
                ListPages.Add(page);
            }

            var mainPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            backupFilePath = Path.Combine(mainPath, "Backup.json");
            if (!File.Exists(backupFilePath))
            {
                File.Create(backupFilePath);
            }

            var connection = Connectivity.NetworkAccess;
            if (connection == NetworkAccess.Local || connection == NetworkAccess.None)
            {
                Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            }
            else
            {
                Data = DataHelper.GetData();

                string login = "";

                if (App.Data != null)
                {
                    if (!String.IsNullOrWhiteSpace(App.Data.Login))
                    {
                        login = App.Data.Login;
                    }
                    if (App.Data.ExpiresAt > DateTime.Now)
                    {
                        var response = Methods.Methods.GetPurchasesResponse();
                        var code = response.statusCode;
                        if (code == HttpStatusCode.OK)
                        {
                            var result = response.result;
                            var purchases = JsonSerializer.Deserialize<Purchase[]>(result, new JsonSerializerOptions
                            {
                                PropertyNameCaseInsensitive = true
                            }).ToList();
                            if (purchases.Count != 0)
                                App.Transactions = purchases;

                            ((GeneralPage)ListPages.FirstOrDefault(x => x is GeneralPage)).Refresh();

                            MainPage = new NavigationPage(tbp);
                            App.tbp.CheckToken();
                        }
                        else
                        {
                            MainPage = new Pages.AuthPage(login);
                        }
                    }
                    else
                    {
                        MainPage = new Pages.AuthPage("");
                    }
                }
                else
                {
                    MainPage = new Pages.AuthPage("");
                }
            }
        }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {

        }

        protected override void OnStart()
        {
            
            
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
