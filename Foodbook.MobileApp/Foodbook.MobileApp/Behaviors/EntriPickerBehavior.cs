using Foodbook.MobileApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Foodbook.MobileApp.Behaviors
{
    public class EntriPickerBehavior : Behavior<Entry>
    {
        private Entry mEntry;
        public static readonly BindableProperty PickerProperty = BindableProperty.CreateAttached("RelatedPicker", typeof(Picker), typeof(EntriPickerBehavior), null);

        public Picker RelatedPicker
        {
            get { return (Picker)GetValue(PickerProperty); }
            set { SetValue(PickerProperty, value); }
        }

        public static readonly BindableProperty PickerTypeProperty = BindableProperty.CreateAttached("PickerType", typeof(string), typeof(EntriPickerBehavior), null);
        public string PickerType
        {
            get { return (string)GetValue(PickerTypeProperty); }
            set { SetValue(PickerProperty, value); }
        }

        protected override void OnAttachedTo(Entry bindable)
        {
            bindable.Focused += Bindable_Focused;
            RelatedPicker.Unfocused += RelatedPicker_Unfocused;
            mEntry = bindable;
        }

    

        protected override void OnDetachingFrom(Entry bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.Focused -= Bindable_Focused;
            RelatedPicker.Unfocused -= RelatedPicker_Unfocused;
        }

        private void RelatedPicker_Unfocused(object sender, FocusEventArgs e)
        {
            if (RelatedPicker.SelectedIndex != -1)
            {
                switch (PickerType)
                {
                    case "CATEGORY":
                        FoodCategoryModel categoryModel = RelatedPicker.SelectedItem as FoodCategoryModel;
                        mEntry.Text = categoryModel.CategoryName;
                        break;
                    case "CUISINE":
                        CuisineModel cuisineModel = RelatedPicker.SelectedItem as CuisineModel;
                        mEntry.Text = cuisineModel.CuisineName;
                        break;
                    case "CALORY":
                        CaloricityModel caloricityModel = RelatedPicker.SelectedItem as CaloricityModel;
                        mEntry.Text = caloricityModel.Name;
                        break;
                    default:
                        mEntry.Text = RelatedPicker.SelectedItem.ToString();
                        break;

                }

            }
        }

        private void Bindable_Focused(object sender, FocusEventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                mEntry.Unfocus();
                RelatedPicker.Focus();
            });
        }
    }
}
