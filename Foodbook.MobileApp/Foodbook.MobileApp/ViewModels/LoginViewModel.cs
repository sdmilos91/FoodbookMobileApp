using Foodbook.MobileApp.Data.Services;
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

        public LoginViewModel()
        {
            LoginCommand = new Command(() => LoginUser());
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
    }
}
