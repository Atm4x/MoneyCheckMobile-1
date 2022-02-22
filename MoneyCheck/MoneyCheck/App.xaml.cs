using MoneyCheck.Helpers;
using MoneyCheck.Methods;
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
        public static string BaseUrl = "https://moneycheck.gym1551.ru";
        public static List<Purchase> Transactions = new List<Purchase>();
        public static List<object> Debtors = new List<object>();
        public static DataHelper.Data Data;
        public static UserBalance UBalance = new UserBalance();
        public static List<Category> Categories = new List<Category>();
        public static TabbPage Tbp;
        public static List<ContentPage> ListPages = new List<ContentPage>();

        public static List<Purchase> LocalPurchases = new List<Purchase>();
        public static string BackupFilePath = String.Empty;

        public App()
        {
            InitializeComponent();
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NTY0NDI5QDMxMzkyZTM0MmUzMFU0eG9iMVp3NXdNMksyM0F2WEFHc0JFZmY4ajJxTGtvMWwxU0RncXBnMTQ9;NTY0NDMwQDMxMzkyZTM0MmUzMFY2ZVBDY291bU9naU41czh2UlRiZXI4RVVhdjB0Q2JiY1lYdjV5MEZGbGM9;NTY0NDMxQDMxMzkyZTM0MmUzMFlCdFhFTmN0TTB2dFgzWU51QUkxeXBISDZPRTBXNDd1OEcyWHdRdC9naWM9;NTY0NDMyQDMxMzkyZTM0MmUzMGtVR2xrZk95djJaNEpvaEx0ZE0zWmRkT1UrN3hKeVJxaEZIdGh6T2FCeTg9");


            Tbp = new TabbPage();

            foreach (ContentPage page in Tbp.Children)
            {
                ListPages.Add(page);
            }

            var mainPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            BackupFilePath = Path.Combine(mainPath, "Backup.json");
            if (!File.Exists(BackupFilePath))
            {
                File.Create(BackupFilePath).Close();
            }

            InitApp();

        }

        public void InitApp()
        {
            Data = DataHelper.GetData();

            string login = "";

            if (App.Data != null)
            {
                var connection = Connectivity.NetworkAccess;
                Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;

                if (connection == NetworkAccess.Local || connection == NetworkAccess.None)
                {
                    var backupModel = BackupHelper.ReadBackup(App.BackupFilePath);
                    if (backupModel != null)
                    {

                        if (backupModel?.purchases?.Count != 0)
                            App.Transactions = backupModel.purchases;
                        if (backupModel?.balance != null)
                            App.UBalance = backupModel.balance;
                        if (backupModel?.categories?.Count != 0)
                            App.Categories = backupModel.categories;

                        ((GeneralPage)ListPages.FirstOrDefault(x => x is GeneralPage)).Refresh(true);

                        MainPage = new NavigationPage(Tbp);
                    }
                    else
                    {
                        Environment.Exit(-1);
                    }
                }
                else
                {
                    if (!String.IsNullOrWhiteSpace(App.Data.Login))
                    {
                        login = App.Data.Login;
                    }
                    if (App.Data.ExpiresAt > DateTime.Now)
                    {
                        var response = Requests.GetStatus();
                        var code = response.statusCode;
                        if (code == HttpStatusCode.OK)
                        {
                            var categoriesResponse = Requests.GetCategories();
                            if (ResponseModel.TryParse(categoriesResponse, out List<Category> categories))
                            {
                                App.Categories = categories;
                            }
                            App.Tbp.CheckToken();

                            MainPage = new NavigationPage(Tbp);
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
            }
            else
            {
                MainPage = new Pages.AuthPage("");
            }
        }
        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (App.Data != null)
            {
                if (e.NetworkAccess == NetworkAccess.ConstrainedInternet || e.NetworkAccess == NetworkAccess.Internet)
                {
                    App.Tbp.CheckToken();
                }
            }
        }

        protected override void OnStart()
        {


        }

        protected override void OnSleep()
        {
        }

        protected override async void OnResume()
        {
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();
                if (location != null)
                {
                    Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}, Accuracy: {location.Accuracy}");
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }
    }
}
