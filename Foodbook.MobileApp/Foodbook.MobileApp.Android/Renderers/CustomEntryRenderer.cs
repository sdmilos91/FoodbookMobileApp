using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Foodbook.MobileApp.Renderers;
using Foodbook.MobileApp.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Graphics.Drawables;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRenderer))]
namespace Foodbook.MobileApp.Droid.Renderers
{
    public class CustomEntryRenderer : EntryRenderer
    {
        private GradientDrawable mNormal, mFocused;
        private CustomEntry mEntry;

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {               
                mEntry = (CustomEntry)e.NewElement;
                EditText textField = (EditText)Control;
                textField.SetPadding(50, 20, 50, 20);

                mNormal = new Android.Graphics.Drawables.GradientDrawable();
                mNormal.SetColor(Android.Graphics.Color.ParseColor("#FFFFFF")); // Changes this drawbale to use a single color instead of a gradient
                mNormal.SetCornerRadius(mEntry.MyRadius.Value);

                mNormal.SetStroke(4, Android.Graphics.Color.Gray);

                mFocused = new Android.Graphics.Drawables.GradientDrawable();
                mFocused.SetColor(Android.Graphics.Color.ParseColor("#FFFFFF")); // Changes this drawbale to use a single color instead of a gradient
                mFocused.SetCornerRadius(mEntry.MyRadius.Value);
                mFocused.SetStroke(4, Android.Graphics.Color.DarkGray);

                var sld = new StateListDrawable();
                sld.AddState(new int[] { Android.Resource.Attribute.StateFocused }, mFocused);
                sld.AddState(new int[] { }, mNormal);
                textField.SetBackgroundDrawable(sld);


                if (mEntry.EntryType == EntryTypes.MULTILINE)
                {
                    textField.SetSingleLine(false);          
                }
            }
        }
    }
}