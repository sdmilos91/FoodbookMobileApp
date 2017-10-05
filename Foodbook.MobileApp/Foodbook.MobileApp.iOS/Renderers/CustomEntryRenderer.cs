using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using Foodbook.MobileApp.Renderers;
using Xamarin.Forms;
using Foodbook.MobileApp.iOS.Renderers;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRenderer))]
namespace Foodbook.MobileApp.iOS.Renderers
{
    public class CustomEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            try
            {
                CustomEntry entry = e.NewElement as CustomEntry;
                entry.HeightRequest = 38;


                if (Control != null)
                {
                    // do whatever you want to the UITextField here!
                    UITextField utf = Control as UITextField;

                    if (entry.EntryType == EntryTypes.MULTILINE)
                    {
                        entry.HeightRequest = 200;
                    }

                    utf.Layer.BorderColor = Color.Gray.ToCGColor();

                    entry.Focused += delegate
                    {
                        utf.Layer.BorderColor = Color.DarkGray.ToCGColor();
                    };

                    entry.Unfocused += delegate
                    {
                        utf.Layer.BorderColor = Color.Gray.ToCGColor();
                    };

                    Control.BorderStyle = UITextBorderStyle.RoundedRect;
                    Control.Layer.BorderWidth = 1;
                }
            }
            catch { }
        }
    }
}
