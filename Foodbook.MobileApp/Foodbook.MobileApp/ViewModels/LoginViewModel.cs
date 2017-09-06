using Foodbook.MobileApp.Data.Models;
using Foodbook.MobileApp.Data.Services;
using Foodbook.MobileApp.Pages;
using Foodbook.MobileApp.Pages.Authentication;
using Foodbook.MobileApp.Tools;
using Plugin.NotificationHub;
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
            if (!string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password))
            {
                Device.BeginInvokeOnMainThread(() => Dialogs.Show());
                LoginResponseModel result = await AccountDataService.LoginUser(Username, Password);
                Device.BeginInvokeOnMainThread(() => Dialogs.Hide());

                if (result.IsSuccess)
                {
                    LocalDataSecureStorage.SaveToken(result.access_token);
                    LocalDataSecureStorage.SaveEmail(result.userName);
                    LocalDataSecureStorage.SaveCookId(result.CookId);
                    LocalDataSecureStorage.SaveCookName(result.CookFullName);
                    LocalDataSecureStorage.SaveCookPhoto(result.PhotoUrl);
                    Task.Run(() =>
                        {
                            CrossNotificationHub.Current.Unregister();
                            CrossNotificationHub.Current.Register(PushNotificationSettings.CONNECTION_STRING, PushNotificationSettings.HUB_NAME, LocalDataSecureStorage.GetNotificationToken(), LocalDataSecureStorage.GetEmail());
                        });

                    App.Current.MainPage = new HomeMasterDetailPage();
                }
                else
                {
                    string message = result.ErrorType == ERROR_TYPES.BAD_REQUEST ? "Pogrešno korisničko ime ili lozinka." : "Greška prilikom prijavljivanja korisnika.";
                    await App.Current.MainPage.DisplayAlert("Obaveštenje", message, "U redu");
                }
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Obaveštenje", "Morate uneti korisničko ime i lozinku.", "U redu");
            }
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
