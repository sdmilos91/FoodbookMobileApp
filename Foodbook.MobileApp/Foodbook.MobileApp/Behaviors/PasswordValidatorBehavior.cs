
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Foodbook.MobileApp.Behaviors
{
    
public class PasswordValidatorBehavior : BaseEntryBehavior
    {
        

        public PasswordValidatorBehavior()
        {

        }

        protected override void OnAttachedTo(Entry bindable)
        {
            mEntry = bindable;
            bindable.TextChanged += HandleTextChanged;
        }

        void HandleTextChanged(object sender, TextChangedEventArgs e)
        {
            IsValid = !string.IsNullOrEmpty(e.NewTextValue) && e.NewTextValue.Length >= 6 ;            
            bool isRequired = mEntry.Behaviors.Any(x => x.GetType() == typeof(RequiredValidatorBehavior));
            if (!(isRequired && string.IsNullOrEmpty(e.NewTextValue)))
            {
                if (!IsValid)
                {
                    Entry ent = sender as Entry;
                    mLbl.IsVisible = true;
                    mLbl.Text = string.Format("Dužina lozinke mora biti između {0} i {1} karaktera.", 6, 100);
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
