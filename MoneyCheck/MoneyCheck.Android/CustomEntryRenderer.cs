using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MoneyCheck.Droid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Xamarin.Forms.Entry), typeof(CustomEntryRenderer))]
namespace MoneyCheck.Droid
{
    public class CustomEntryRenderer : EntryRenderer
    {
        public CustomEntryRenderer(Context context) : base(Android.App.Application.Context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Entry> e)
        {
            base.OnElementChanged(e);
            if (Control != null && e.NewElement != null)
            {
                var entry = (Xamarin.Forms.Entry)e.NewElement;
                if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
                    Control.BackgroundTintList = ColorStateList.ValueOf(entry.BackgroundColor.ToAndroid());
                else
                    Control.Background.SetColorFilter(entry.BackgroundColor.ToAndroid(), PorterDuff.Mode.SrcAtop);
            }
        }
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == "BackgroundColor")
            {
                var entry = (Xamarin.Forms.Entry)sender;
                if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
                    Control.BackgroundTintList = ColorStateList.ValueOf(entry.BackgroundColor.ToAndroid());
                else
                    Control.Background.SetColorFilter(entry.BackgroundColor.ToAndroid(), PorterDuff.Mode.SrcAtop);
            }
        }
    }
}