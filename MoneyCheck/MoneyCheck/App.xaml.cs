﻿using MoneyCheck.Helpers;
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
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MoneyCheck
{
    public partial class App : Application
    {
        public static string BaseUrl = "https://moneycheck.gym1551.ru";
        public static List<Purchase> Transactions = new List<Purchase>();
        public static List<DebtorType> Debtors = new List<DebtorType>();
        public static DataHelper.Data Data;
        public static UserBalance UBalance = new UserBalance();
        public static List<Category> Categories = new List<Category>();
        public static TabbPage Tbp;
        public static List<ContentPage> ListPages = new List<ContentPage>();

        public static List<Purchase> LocalPurchases = new List<Purchase>();
        public static List<DebtorType> LocalDebtors = new List<DebtorType>();
        public static string BackupFilePath = String.Empty;

        public static Page MainWindow;

        public App()
        {
            InitializeComponent();
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NTY0NDI5QDMxMzkyZTM0MmUzMFU0eG9iMVp3NXdNMksyM0F2WEFHc0JFZmY4ajJxTGtvMWwxU0RncXBnMTQ9;NTY0NDMwQDMxMzkyZTM0MmUzMFY2ZVBDY291bU9naU41czh2UlRiZXI4RVVhdjB0Q2JiY1lYdjV5MEZGbGM9;NTY0NDMxQDMxMzkyZTM0MmUzMFlCdFhFTmN0TTB2dFgzWU51QUkxeXBISDZPRTBXNDd1OEcyWHdRdC9naWM9;NTY0NDMyQDMxMzkyZTM0MmUzMGtVR2xrZk95djJaNEpvaEx0ZE0zWmRkT1UrN3hKeVJxaEZIdGh6T2FCeTg9");
        }

        public async void InitApp()
        {
            MainWindow = MainPage;
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

            Data = DataHelper.GetData();

            string login = "";

            var connection = Connectivity.NetworkAccess;
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            if (App.Data != null)
            {

                if (!Connectivity.NetworkAccess.HasInternet())
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

                        await ((GeneralPage)ListPages.FirstOrDefault(x => x is GeneralPage)).Refresh(true);

                        MainPage = new NavigationPage(Tbp);
                    }
                    else
                    {
                        await ((GeneralPage)ListPages.FirstOrDefault(x => x is GeneralPage)).Refresh(true);

                        MainPage = new NavigationPage(Tbp);
                        //Environment.Exit(-1);
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
                        await ((GeneralPage)ListPages.FirstOrDefault(x => x is GeneralPage)).Refresh(true);
                        MainPage = new NavigationPage(Tbp);
                    }
                    else
                    {
                        MainPage = new NavigationPage(new Pages.AuthPage(login));
                    }

                }
            }
            else
            {
                MainPage = new NavigationPage(new Pages.AuthPage(""));
            }
        }
        private async void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (App.Data != null)
            {
                if (e.NetworkAccess.HasInternet())
                {
                    if (App.Tbp.IsLoaded)
                    {
                        await App.Tbp.CheckToken();
                    }
                }
            }
        }

        
        protected override void OnStart()
        {
            InitApp();
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
