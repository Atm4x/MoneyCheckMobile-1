using MoneyCheck.Models;
using MoneyCheck.Pages.SubPages.SubPagesMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MoneyCheck.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DebtorControl : Frame
    {
        public DebtorControl()
        {
            InitializeComponent();
        }
        public void OnTapped(object sender, EventArgs e)
        {
            App.Tbp.Navigation.PushAsync(new DebtsOfDebtorListPage((DebtorType)this.BindingContext));
        }
    }
}