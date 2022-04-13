using MoneyCheck.Helpers;
using MoneyCheck.Methods;
using MoneyCheck.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MoneyCheck.Pages.SubPages
{
    public partial class StatPage : ContentPage
    {
        private bool IsLoaded = false;
        public StatPage()
        {
            InitializeComponent();
            Chart.EnableDataPointSelection = true;
            Chart.DataMarker = new Syncfusion.SfChart.XForms.ChartDataMarker()
            {
                ShowMarker = true,
                MarkerColor = Color.Red,
                MarkerType = Syncfusion.SfChart.XForms.DataMarkerType.Ellipse, 
                MarkerWidth = 6, 
                MarkerHeight = 6 
            };

            Diagram.EnableDataPointSelection = true;
            Diagram.DataMarker = new Syncfusion.SfChart.XForms.ChartDataMarker()
            {
                ShowMarker = true,
                MarkerColor = Color.Red,
                MarkerType = Syncfusion.SfChart.XForms.DataMarkerType.Ellipse,
                MarkerWidth = 6,
                MarkerHeight = 6
            };
            Diagram.ExplodeOnTouch = true;
            Diagram.CircularCoefficient = 0.8;
            Diagram.ExplodeRadius = 10;
            Chart.EnableAnimation = true;
            Diagram.EnableAnimation = true;
            Chart.AnimationDuration = 2;
            Diagram.AnimationDuration = 1;
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (!IsLoaded)
            {
                await Refresh();
                IsLoaded = true;
            }
        }
        private async void Refreshing(object sender, EventArgs e)
        {
            if (Connectivity.NetworkAccess.HasInternet())
            {
                await Refresh();
                ((RefreshView)sender).IsRefreshing = false;
            } 
            else
            {
                ((RefreshView)sender).IsRefreshing = false;
                return;
            }
        }

        public async Task Refresh()
        {
            List<Purchase> purchases = null;
            if (Connectivity.NetworkAccess.HasInternet())
            {
                var purchasesResponse = await Requests.GetPurchasesAsync();
                purchases = purchasesResponse.GetParsed<List<Purchase>>();
            }
            else
            {
                purchases = App.Transactions;
            }


            //purchases.Select(x => new { partx = x.BoughtAt, y = x.Amount }).OrderBy(x => x.partx).;


            var purchasesEachDate = purchases.Where(x => x.IsPurchase == true).Select(x => new PurchaseGrouping()
            {
                BoughtAt = x.BoughtAt.Date,
                Amount = x.Amount
            })
            .GroupBy(date => date.BoughtAt).Select(x => new PurchaseGrouping() { BoughtAt = x.Key, Amount = x.Sum(sum => sum.Amount) }).OrderByDescending(date => date.BoughtAt).ToList();

            var purchasesEachCategory = purchases.Select(x => new CategoryGrouping()
            {
                Category = x.CategoryName,
                Amount = x.Amount
            }).GroupBy(x => x.Category).Select(x => new CategoryGrouping() { Category = x.Key, Amount = x.Sum(s => s.Amount) }).ToList();

            //var groups = from purchase in purchases group purchase by purchase.BoughtAt into a;
            var textBinding = purchasesEachDate.OrderByDescending(x => x.Amount);

            Highest.BindingContext = textBinding.First();
            Lowest.BindingContext = textBinding.Last();

            Chart.ItemsSource = purchasesEachDate;
            Diagram.ItemsSource = purchasesEachCategory;
            Chart.Animate();
            Diagram.Animate();
        }
        public class PurchaseGrouping
        {
            public DateTime BoughtAt { get; set; }
            public decimal Amount { get; set; }

        }

        public class CategoryGrouping
        {
            public string Category { get; set; }
            public decimal Amount { get; set; }

        }

    }
}