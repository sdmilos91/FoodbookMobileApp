
using Foodbook.MobileApp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Foodbook.MobileApp.Behaviors
{
    
public class EmailValidatorBehavior : BaseEntryBehavior
    {

        

        public EmailValidatorBehavior()
        {          
        }

        protected override void OnAttachedTo(Entry bindable)
        {
            bindable.TextChanged += HandleTextChanged;
            mEntry = bindable;
        }

        void HandleTextChanged(object sender, TextChangedEventArgs e)
        {
            IsValid = Utils.IsEmailValid(e.NewTextValue);
            //((Entry)sender).TextColor = IsValid ? Color.Default : Color.Red;

            bool isRequired = mEntry.Behaviors.Any(x => x.GetType() == typeof(RequiredValidatorBehavior));
            if (!(isRequired && string.IsNullOrEmpty(e.NewTextValue)))
            {
                if (!IsValid)
                {
                    mLbl.Text = "Nevalidan format email adrese.";
                    mLbl.IsVisible = true;
                }
                else
                {
                    mLbl.IsVisible = false;
                }
            }
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            bindable.TextChanged -= HandleTextChanged;

        }
    }
}
