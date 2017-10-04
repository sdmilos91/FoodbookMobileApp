using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Foodbook.MobileApp.Behaviors
{
    public class BaseEntryBehavior : Behavior<Entry>
    {
        static readonly BindablePropertyKey IsValidPropertyKey = BindableProperty.CreateReadOnly("IsValid", typeof(bool), typeof(RequiredValidatorBehavior), false);

        public static readonly BindableProperty IsValidProperty = IsValidPropertyKey.BindableProperty;

        public Entry mEntry;

        public bool IsValid
        {
            get { return (bool)base.GetValue(IsValidProperty); }
            protected set { base.SetValue(IsValidPropertyKey, value); }
        }

        public static readonly BindableProperty LabelProperty = BindableProperty.CreateAttached("mLbl", typeof(Label), typeof(RequiredValidatorBehavior), null);

        public Label mLbl
        {
            get { return (Label)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        public static readonly BindableProperty NameProperty = BindableProperty.CreateAttached("EntryName", typeof(string), typeof(RequiredValidatorBehavior), null);

        public string EntryName
        {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }

    }
}
