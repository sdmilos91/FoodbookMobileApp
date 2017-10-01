using Foodbook.MobileApp.Tools;
using Plugin.Connectivity;
using PushNotification.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Foodbook.MobileApp.Pages.Shared
{
    public class NoInternetPage : ContentPage
    {
        public NoInternetPage()
        {
            Title = "Internet konekcija";
            BackgroundColor = Color.White;
            Label text = new Label
            {
                FontSize = 17,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Text = "Niste povezani na internet. Povežite se na internet i izaberite opciju Nastavi.",
                HeightRequest = 100,
                HorizontalTextAlignment = TextAlignment.Center,
                Margin = new Thickness(0, 40, 0, 0)
            };

            Button continueBtn = new Button
            {
                HorizontalOptions = LayoutOptions.Center,
                Text = "Nastavi",
                TextColor = Color.White,
                FontAttributes = FontAttributes.Bold,
                Margin = new Thickness(0, 30, 0, 0),
                BackgroundColor = Color.FromHex(MyColors.GREEN),                
                WidthRequest = 200,
                IsEnabled = false
            };

            CrossConnectivity.Current.ConnectivityChanged += (args, e) =>
            {
                continueBtn.IsEnabled = e.IsConnected;

            };

            continueBtn.Clicked += Retry_Clicked; ;

            StackLayout mainStack = new StackLayout
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Orientation = StackOrientation.Vertical
            };

            StackLayout titleStack = new StackLayout
            {
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Padding = new Thickness(10, 10, 10, 10),
                BackgroundColor = Color.FromHex(MyColors.DARK_RED)
            };

            Label titleLbl = new Label
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HorizontalTextAlignment = TextAlignment.Center,
                TextColor = Color.White,
                FontAttributes = FontAttributes.Bold,
                Text = "Problem sa internet konekcijom",                
                
            };

            titleStack.Children.Add(titleLbl);
            mainStack.Children.Add(titleStack);

            mainStack.Children.Add(text);
            mainStack.Children.Add(continueBtn);

            Content = mainStack;
        }

        private async void Retry_Clicked(object sender, EventArgs e)
        {


            NavigationPage np;

            if (string.IsNullOrEmpty(LocalDataSecureStorage.GetToken()))
            {
                App.Current.MainPage = new NavigationPage(new LoginPage());
                //Background color
                App.Current.MainPage.SetValue(NavigationPage.BarBackgroundColorProperty, Color.FromHex(MyColors.DARK_RED));

                //Title color
                App.Current.MainPage.SetValue(NavigationPage.BarTextColorProperty, Color.White);

                LocalDataSecureStorage.ClearAllData();
            }
            else
            {
                if (Device.OS == TargetPlatform.Android || Device.OS == TargetPlatform.iOS)
                {
                    CrossPushNotification.Current.Register();
                }
                App.Current.MainPage = new HomeMasterDetailPage();
            }
        }
    }
}