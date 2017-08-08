using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Foodbook.MobileApp.Behaviors
{
   public  class BasePasswordBehavior : BaseEntryBehavior
    {

        public static readonly BindableProperty EntryProperty = BindableProperty.CreateAttached("PasswordEntry", typeof(Entry), typeof(RequiredValidatorBehavior), null);

        public Entry PasswordEntry
        {
            get { return (Entry)GetValue(EntryProperty); }
            set { SetValue(EntryProperty, value); }
        }
    }
}
