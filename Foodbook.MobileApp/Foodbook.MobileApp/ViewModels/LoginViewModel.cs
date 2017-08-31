using Foodbook.MobileApp.Data.Services;
using Foodbook.MobileApp.Pages;
using Foodbook.MobileApp.Pages.Authentication;
using Foodbook.MobileApp.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Foodbook.MobileApp.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public Command LoginCommand { get; }
        public Command RegisterCommand { get; }
        public Command SkipCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new Command(() => LoginUser());
            RegisterCommand = new Command(() => Register());
            SkipCommand = new Command(() => Skip());
        }

        private async void LoginUser()
        {
            Device.BeginInvokeOnMainThread(() => Dialogs.Show());
            var result = await AccountDataService.LoginUser(Username, Password);
            Device.BeginInvokeOnMainThread(() => Dialogs.Hide());

            if (result.IsSuccess)
            {
                LocalDataSecureStorage.SaveToken(result.access_token);
                LocalDataSecureStorage.SaveEmail(result.userName);
             
            }

            MessagingCenter.Send(this, MessageCenterKeys.LOGGED_IN, result.IsSuccess);
        }

        private async void Register()
        {
            Page page = App.Current.MainPage;

            MasterDetailPage masterPage = page as MasterDetailPage;
            await page.Navigation.PushAsync(new RegisterPage());            
        }

        private void Skip()
        {
            App.Current.MainPage = new HomeMasterDetailPage();
        }
    }
}
