using Foodbook.MobileApp.Pages.Cook;
using Foodbook.MobileApp.Pages.Recipe;
using Foodbook.MobileApp.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Foodbook.MobileApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomeMasterDetailPage : MasterDetailPage
    {
        public HomeMasterDetailPage()
        {
            InitializeComponent();
            Detail = new NavigationPage(new Recipe.RecipesPage());
            MasterPage.ListView.ItemSelected += ListView_ItemSelected;

            //Background color
            Detail.SetValue(NavigationPage.BarBackgroundColorProperty, Color.FromHex(MyColors.DARK_RED));

            //Title color
            Detail.SetValue(NavigationPage.BarTextColorProperty, Color.White);
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as HomeMasterDetailPageMenuItem;
            if (item == null)
                return;

            var page = (Page)Activator.CreateInstance(item.TargetType);
            if (item.Id == 4)
            {
                if (string.IsNullOrEmpty(LocalDataSecureStorage.GetToken()))
                {
                    App.Current.MainPage = new NavigationPage(new LoginPage());
                    //Background color
                    App.Current.MainPage.SetValue(NavigationPage.BarBackgroundColorProperty, Color.FromHex(MyColors.DARK_RED));

                    //Title color
                    App.Current.MainPage.SetValue(NavigationPage.BarTextColorProperty, Color.White);
                }
                else
                {
                    LocalDataSecureStorage.DeleteEmail();
                    LocalDataSecureStorage.DeleteToken();
                    App.Current.MainPage = new HomeMasterDetailPage();
                }
            }
            else
            {
                page.Title = item.Title;

                // Detail = new NavigationPage(new LoginPage());
                Detail = new NavigationPage(page);
                //Background color
                Detail.SetValue(NavigationPage.BarBackgroundColorProperty, Color.FromHex(MyColors.DARK_RED));

                //Title color
                Detail.SetValue(NavigationPage.BarTextColorProperty, Color.White);
            }
            IsPresented = false;

            MasterPage.ListView.SelectedItem = null;
        }
    }
}