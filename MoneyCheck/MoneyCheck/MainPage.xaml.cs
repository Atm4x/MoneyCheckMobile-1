using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Text.Json;
using System.Security.Cryptography;
using MoneyCheck.Helpers;
using Android.App;
using Android.Net.Wifi;
using MoneyCheck.Models;

namespace MoneyCheck
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            
        }
        //private async void SendToTabbedPage() => await Navigation.PushAsync(new NavigationPage(new Pages.TabbPage()));
    }

    
}
