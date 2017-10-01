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
        }


        protected IProgressDialog Dialogs { get; set; }

        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        protected void ShowDialog()
        {
            //Device.BeginInvokeOnMainThread(() =>
            //{               
                if(this.Dialogs == null)
                     this.Dialogs = UserDialogs.Instance.Loading("Loading");
                this.Dialogs.Show();
            //});
        }

        protected void HideDialog()
        {
            // Device.BeginInvokeOnMainThread(() =>
            //{
            if (this.Dialogs == null)
                this.Dialogs = UserDialogs.Instance.Loading("Loading");
            this.Dialogs.Hide();

            //});
        }

        public virtual void OnViewDisappearing()
        {

        }

        public virtual void OnViewAppearing()
        {

        }

    }
}
