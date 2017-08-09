using Foodbook.MobileApp.Data.Models;
using Foodbook.MobileApp.ViewModels;
using Foodbook.Pages;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Foodbook.MobileApp.Pages.Recipe
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecipeDetailsPage : ContentPage
    {
        public RecipeDetailsPage(RecipeDataModel recipe)
        {
            InitializeComponent();
            //NavigationPage.SetHasNavigationBar(this, false);


            ToolbarItem favourite = new ToolbarItem
            {
                Icon = "favorite",
                Order = ToolbarItemOrder.Primary
            };

            ToolbarItem edit = new ToolbarItem
            {
                Icon = "edit",
                Order = ToolbarItemOrder.Primary
            };

            ToolbarItem delete = new ToolbarItem
            {
                Icon = "delete",
                Order = ToolbarItemOrder.Primary
            };

            ToolbarItems.Add(favourite);
            ToolbarItems.Add(edit);
            ToolbarItems.Add(delete);

            RecipeDetailViewModel viewModel = new RecipeDetailViewModel(recipe);
            BindingContext = viewModel;

            MessagingCenter.Subscribe<AddCommentViewModel, PostRecipeCommentModel>(this, "COMMENT_ADDED", async (sender, model) => 
            {
                viewModel.CommentAddedCommant.Execute(model);
                await PopupNavigation.PopAsync();


            });

            

        }
        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new ImageViewPage("http://kuhinjarecepti.com/wp-content/uploads/2012/01/%C5%A0opska-salata.jpeg"));
        }

        private void favBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddRecipePage());
        }

        private void BackButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }

    
}