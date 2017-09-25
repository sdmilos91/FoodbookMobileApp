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

        private RecipesPageViewModel mViewModel;

        public RecipesPage()
        {
            InitializeComponent();

            mViewModel = new RecipesPageViewModel();
            BindingContext = mViewModel;
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

        protected override void OnAppearing()
        {
            base.OnAppearing();
            mViewModel.OnViewAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            mViewModel.OnViewDisappearing();
        }

    }
}