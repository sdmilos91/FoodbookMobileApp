using Foodbook.MobileApp.Data.Services;
using Foodbook.MobileApp.Pages;
using Foodbook.MobileApp.Pages.Shared;
using Foodbook.MobileApp.Tools;
using Plugin.Connectivity;
using Plugin.NotificationHub;
using PushNotification.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Foodbook.MobileApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

       
            string token = LocalDataSecureStorage.GetToken();
            CrossConnectivity.Current.ConnectivityChanged += (sender, args) =>
            {
                if (!args.IsConnected)
                {
                    Application.Current.MainPage = new NavigationPage(new NoInternetPage());
                }
            };

            //Ako uredja nije povezan na internet prikazi NoInternet ekran
            if (!CrossConnectivity.Current.IsConnected)
            {
                Application.Current.MainPage = new NavigationPage(new NoInternetPage());
            }
            else
            {
                if (string.IsNullOrEmpty(token))
                {
                    MainPage = new NavigationPage(new LoginPage());
                    //Background color
                    MainPage.SetValue(NavigationPage.BarBackgroundColorProperty, Color.FromHex(MyColors.DARK_RED));

                    //Title color
                    MainPage.SetValue(NavigationPage.BarTextColorProperty, Color.White);

                    LocalDataSecureStorage.ClearAllData();
                }
                else
                {
                    if (Device.OS == TargetPlatform.Android || Device.OS == TargetPlatform.iOS)
                    {
                        CrossPushNotification.Current.Register();
                    }
                    MainPage = new HomeMasterDetailPage();
                }
            }
        }

        protected override async void OnStart()
        {            

            // Handle when your app starts
            string token = LocalDataSecureStorage.GetToken();
            bool isAuth = await AccountDataService.IsUserAuthenticated(token);

            if (!isAuth)
            {
                MainPage = new NavigationPage(new LoginPage());
                //Background color
                MainPage.SetValue(NavigationPage.BarBackgroundColorProperty, Color.FromHex(MyColors.DARK_RED));

                //Title color
                MainPage.SetValue(NavigationPage.BarTextColorProperty, Color.White);
            }
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
