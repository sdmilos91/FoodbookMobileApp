using Foodbook.MobileApp.AppResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Foodbook.MobileApp.Behaviors
{
    public class RequiredValidatorBehavior : BaseEntryBehavior
    {


        public RequiredValidatorBehavior()
        {
        }

        protected override void OnAttachedTo(Entry bindable)
        {
            mEntry = bindable;
            bindable.TextChanged += HandleTextChanged;
        }

        void HandleTextChanged(object sender, TextChangedEventArgs e)
        {
            IsValid = !string.IsNullOrEmpty(e.NewTextValue);

            if (!IsValid)
            {
                Entry ent = sender as Entry;
                mLbl.IsVisible = true;
                mLbl.Text = string.Format(LocalizationResource.RequiredFieldMsg, ent.Placeholder);

            }
            else
            {
                mLbl.IsVisible = false;
            }
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            bindable.TextChanged -= HandleTextChanged;

        }
    }
}
