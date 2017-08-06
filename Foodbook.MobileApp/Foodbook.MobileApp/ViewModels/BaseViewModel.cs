using Acr.UserDialogs;
using Foodbook.MobileApp.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Foodbook.MobileApp.ViewModels
{
    public abstract class BaseViewModel : ObservableObject
    {

        protected BaseViewModel()
        {
            this.Dialogs = UserDialogs.Instance.Loading("Loading");
            Device.BeginInvokeOnMainThread(() => Dialogs.Hide());
        }


        protected IProgressDialog Dialogs { get; }

        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }
    }
}
