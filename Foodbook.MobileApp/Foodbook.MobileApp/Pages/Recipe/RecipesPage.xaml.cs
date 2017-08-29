﻿using Acr.UserDialogs;
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

            MessagingCenter.Subscribe<AddRecipeViewModel>(this, MessageCenterKeys.ADDED, (sender) => {

                DisplayAlert("Obaveštenje", "Recept je uspešno dodat.", "U redu");
                App.Current.MainPage = new HomeMasterDetailPage();

            });

            MessagingCenter.Subscribe<EditRecipeViewModel>(this, MessageCenterKeys.EDITED, (sender) => {

                DisplayAlert("Obaveštenje", "Recept je uspešno ažuriran.", "U redu");
                App.Current.MainPage = new HomeMasterDetailPage();

            });

            BindingContext = new RecipesPageViewModel();
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