using Acr.UserDialogs;
using Foodbook.MobileApp.Data.Models;
using Foodbook.MobileApp.Data.Services;
using Foodbook.MobileApp.Pages.Recipe;
using Foodbook.MobileApp.Tools;
using Foodbook.MobileApp.ViewModels;
using Foodbook.Pages;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Foodbook.MobileApp.Pages.Cook
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CooksPage : ContentPage
    {
        public ObservableCollection<RecipeDataModel> Items { get; set; }
        public string RecipePhoto { get; set; }

        public CooksPage()
        {
            InitializeComponent();

            BindingContext = new CooksPageViewModel();
        }


        async void Handle_ItemTapped(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;

            ResponseCookModel cook = e.SelectedItem as ResponseCookModel;

            await Navigation.PushAsync(new UserDetailsPage(cook));

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
    }
}