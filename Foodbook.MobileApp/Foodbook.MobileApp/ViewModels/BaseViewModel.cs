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
            Device.BeginInvokeOnMainThread(() =>
            {
                this.Dialogs = UserDialogs.Instance.Loading("Loading");
                this.Dialogs.Hide();
            });
        }


        protected IProgressDialog Dialogs { get; set; }

        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        public virtual void OnViewDisappearing()
        {

        }

        public virtual void OnViewAppearing()
        {

        }
    }
}
