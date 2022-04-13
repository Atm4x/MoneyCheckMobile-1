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
    public partial class CalcPage : ContentPage
    {
        private bool IsLoaded = false;
        public CalcPage()
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
            Chart.EnableAnimation = true;
            Chart.AnimationDuration = 2;
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
            if (Connectivity.NetworkAccess == NetworkAccess.Internet || Connectivity.NetworkAccess == NetworkAccess.ConstrainedInternet)
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
            var response = await Requests.GetInflationForYearAsync();

            if (ResponseModel.TryParse(response, out InflationChunkModel[] chunks))
            {
                Chart.ItemsSource = chunks;
                Chart.Animate();
            }
        }
    }
}