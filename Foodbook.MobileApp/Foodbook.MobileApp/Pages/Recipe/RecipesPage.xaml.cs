using Foodbook.MobileApp.Data.Models;
using Foodbook.MobileApp.Data.Services;
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

        public RecipesPage()
        {
            InitializeComponent();
            
            
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            var recipes = await RecipeDataService.GetRecipes();
            if(recipes != null)
                Items =new ObservableCollection<RecipeDataModel>(recipes);
            BindingContext = this;
            int i = 2;
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