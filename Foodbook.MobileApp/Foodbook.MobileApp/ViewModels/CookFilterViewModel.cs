using Foodbook.MobileApp.Data.Models;
using Foodbook.MobileApp.Data.Services;
using Foodbook.MobileApp.Tools;
using Realms;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Foodbook.MobileApp.ViewModels
{
    public class CookFilterViewModel : BaseViewModel
    {
        public string CookName { get; set; }

        public Command CancelCommand { get; }
        public Command FilterCommand { get; }

        public CookFilterViewModel(string cookName)
        {
            CookName = cookName;

            CancelCommand = new Command(() => Cancel());
            FilterCommand = new Command(() => Filter());
        }

        private async void Cancel()
        {
            MasterDetailPage masterPage = App.Current.MainPage as MasterDetailPage;
            await masterPage.Detail.Navigation.PopPopupAsync();
        }

        private async void Filter()
        {            
            MessagingCenter.Send(this, "FILTERED", CookName);

            MasterDetailPage masterPage = App.Current.MainPage as MasterDetailPage;
            await masterPage.Detail.Navigation.PopPopupAsync();
        }
    }
}
