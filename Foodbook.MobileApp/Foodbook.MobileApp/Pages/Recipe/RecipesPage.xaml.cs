using Acr.UserDialogs;
using Foodbook.MobileApp.Data.Models;
using Foodbook.MobileApp.Data.Services;
using Foodbook.MobileApp.Tools;
using Foodbook.MobileApp.ViewModels;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Foodbook.MobileApp.Pages.Recipe
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecipesPage : ContentPage
    {
        public ObservableCollection<RecipeDataModel> Items { get; set; }
        public string RecipePhoto { get; set; }

        public RecipesPage()
        {
            InitializeComponent();

            MessagingCenter.Subscribe<AddRecipeViewModel, bool>(this, MessageCenterKeys.ADDED, (sender, args) => {

                App.Current.MainPage = new HomeMasterDetailPage();

            });

            BindingContext = new RecipesPageViewModel();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            //if (Items == null)
            //{
            //    var dialogs = UserDialogs.Instance.Loading("Loading");

            //    Device.BeginInvokeOnMainThread(() => dialogs.Show());
            //    var recipes = await RecipeDataService.GetRecipes();
            //    Device.BeginInvokeOnMainThread(() => dialogs.Hide());

            //    if (recipes != null)
            //        Items = new ObservableCollection<RecipeDataModel>(recipes);
            //    foreach (var item in Items)
            //    {
            //        item.PhotoUrl = item.Photos != null && item.Photos.Any() ? item.Photos.FirstOrDefault().Url : "http://kuhinjarecepti.com/wp-content/uploads/2012/01/%C5%A0opska-salata.jpeg";
            //    }

            //    BindingContext = this;
            //}
        }

        async void Handle_ItemTapped(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;

            RecipeDataModel recipe = e.SelectedItem as RecipeDataModel;

            await Navigation.PushAsync(new RecipeDetailsPage(recipe));

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
    }
}