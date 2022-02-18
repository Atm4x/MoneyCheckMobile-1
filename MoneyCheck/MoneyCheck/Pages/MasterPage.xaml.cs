using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MoneyCheck.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterPage : MasterDetailPage
    {
        public Label NotFoundLabel;
        public MasterPage()
        {
            InitializeComponent();
            Detail = new Pages.SubPages.GeneralPage();
            IsPresented = false;
        }


        
        private void Button_Clicked(object sender, EventArgs e)
        {
            IsPresented = true;
        }
    }

    
}