
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Foodbook.MobileApp.Behaviors
{
    
    public class ConfirmPasswordValidatorBehavior : BasePasswordBehavior
    {

        public ConfirmPasswordValidatorBehavior()
        {        
        }

        protected override void OnAttachedTo(Entry bindable)
        {
            mEntry = bindable;
            bindable.TextChanged += HandleTextChanged;
        }

        void HandleTextChanged(object sender, TextChangedEventArgs e)
        {
            IsValid = !string.IsNullOrEmpty(PasswordEntry.Text) && PasswordEntry.Text.Equals(e.NewTextValue);

            if (IsValid)
            {
                mLbl.IsVisible = false;
                ConfirmPasswordValidatorBehavior confirmPasswordBehavior = PasswordEntry.Behaviors.FirstOrDefault(x => x.GetType() == typeof(ConfirmPasswordValidatorBehavior)) as ConfirmPasswordValidatorBehavior;
                if (confirmPasswordBehavior != null)
                {
                    confirmPasswordBehavior.IsValid = true;
                }
            }
            else if (!string.IsNullOrEmpty(PasswordEntry.Text))
            {
                mLbl.IsVisible = true;
                mLbl.Text = "Lozinka i potvrda lozinke se ne slažu.";
            }
            else
            {
                IsValid = true;
            }
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            bindable.TextChanged -= HandleTextChanged;

        }
    }
}
