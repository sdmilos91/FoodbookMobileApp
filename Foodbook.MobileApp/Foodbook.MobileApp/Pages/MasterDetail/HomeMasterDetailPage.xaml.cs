using Foodbook.MobileApp.Pages.Cook;
using Foodbook.MobileApp.Pages.Recipe;
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
            Detail.SetValue(NavigationPage.BarBackgroundColorProperty, Color.FromHex("#EF5350"));

            //Title color
            Detail.SetValue(NavigationPage.BarTextColorProperty, Color.White);
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as HomeMasterDetailPageMenuItem;
            if (item == null)
                return;

            var page = (Page)Activator.CreateInstance(item.TargetType);
            page.Title = item.Title;
            
           // Detail = new NavigationPage(new LoginPage());
            Detail = new NavigationPage(new CooksPage());
            //Background color
            Detail.SetValue(NavigationPage.BarBackgroundColorProperty, Color.FromHex("#EF5350"));

            //Title color
            Detail.SetValue(NavigationPage.BarTextColorProperty, Color.White);

            IsPresented = false;

            MasterPage.ListView.SelectedItem = null;
        }
    }
}