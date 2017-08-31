using Foodbook.MobileApp.Data.Services;
using Foodbook.MobileApp.Pages;
using Foodbook.MobileApp.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Foodbook.MobileApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            string token = LocalDataSecureStorage.GetToken();
            

            if (string.IsNullOrEmpty(token))
            {
                MainPage = new NavigationPage(new LoginPage());
                //Background color
                MainPage.SetValue(NavigationPage.BarBackgroundColorProperty, Color.FromHex(MyColors.DARK_RED));

                //Title color
                MainPage.SetValue(NavigationPage.BarTextColorProperty, Color.White);
            }
            else
            {

                MainPage = new HomeMasterDetailPage();
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
