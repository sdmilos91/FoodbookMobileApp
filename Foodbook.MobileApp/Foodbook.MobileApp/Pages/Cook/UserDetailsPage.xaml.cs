﻿using Foodbook.MobileApp;
using Foodbook.MobileApp.Data.Models;
using Foodbook.MobileApp.Data.Services;
using Foodbook.MobileApp.Pages.Cook;
using Foodbook.MobileApp.Pages.Recipe;
using Foodbook.MobileApp.Tools;
using Foodbook.MobileApp.ViewModels;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Foodbook.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class UserDetailsPage : ContentPage
	{
        
        public UserDetailsPage(ResponseCookModel cook, bool userProfile = false)
		{
			InitializeComponent ();
            UserDetailsViewModel viewModel = new UserDetailsViewModel(cook, userProfile);
            ToolbarItem favourite = new ToolbarItem
            {
                Icon = cook.IsFollowed ? "favorited.png" : "favorite.png",  
                Order = ToolbarItemOrder.Primary,
                Command = viewModel.FavouriteCookCommand                
            };            

            favourite.CommandParameter = favourite;


            ToolbarItem edit = new ToolbarItem
            {
                Icon = "edit.png",
                Order = ToolbarItemOrder.Primary
            };

            edit.Clicked += async delegate
            {
                MessagingCenter.Subscribe<EditUserDetailsPageViewModel, ResponseCookModel>(this, "USER_EDIITED", (sender, args) =>
                {
                    cook = args;
                });
                await Navigation.PushAsync(new EditUserDetailsPage(cook));
            };

            if (!string.IsNullOrEmpty(LocalDataSecureStorage.GetToken()))
            {
                if(!userProfile)
                    ToolbarItems.Add(favourite);
                else
                    ToolbarItems.Add(edit);
            }

          
            MessagingCenter.Subscribe<AddCommentViewModel>(this, "COOK_COMMENT_ADDED", async (sender) =>
            {
                viewModel.CommentAddedCommand.Execute(null);
                await PopupNavigation.PopAsync();
                //MessagingCenter.Send(this, "COOK_RATING_UPDATED");

            });

          

            BindingContext = viewModel;

        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }


        private async void RecipeSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;

            RecipeDataModel recipe = e.SelectedItem as RecipeDataModel;
            await Navigation.PushAsync(new RecipeDetailsPage(recipe));
            ((ListView)sender).SelectedItem = null;
        }
    }
}