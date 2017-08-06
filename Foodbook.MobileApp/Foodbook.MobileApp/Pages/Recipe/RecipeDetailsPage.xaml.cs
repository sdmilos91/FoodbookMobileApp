using Foodbook.MobileApp.Data.Models;
using Foodbook.MobileApp.ViewModels;
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

            lblTab1.GestureRecognizers.Add(new TapGestureRecognizer((view) => SwitchTabs(1)));
            lblTab2.GestureRecognizers.Add(new TapGestureRecognizer((view) => SwitchTabs(2)));
            lblTab3.GestureRecognizers.Add(new TapGestureRecognizer((view) => SwitchTabs(3)));

            RecipeDetailViewModel viewModel = new RecipeDetailViewModel(recipe);
            BindingContext = viewModel;

            MessagingCenter.Subscribe<AddCommentViewModel, PostRecipeCommentModel>(this, "COMMENT_ADDED", async (sender, model) => 
            {
                viewModel.CommentAddedCommant.Execute(model);
                await PopupNavigation.PopAsync();


            });

            

        }

        private void SwitchTabs(int selectedTab)
        {
            string tabColor = "#EF5350";
            string indicatorColor = "#FFD54F";

            indicatorTab1.BackgroundColor = Color.FromHex(tabColor);
            indicatorTab2.BackgroundColor = Color.FromHex(tabColor);
            indicatorTab3.BackgroundColor = Color.FromHex(tabColor);
            detailTab.IsVisible = false;
            preparationTab.IsVisible = false;
            commentTab.IsVisible = false;

            switch (selectedTab)
            {
                case 1: indicatorTab1.BackgroundColor = Color.FromHex(indicatorColor); detailTab.IsVisible = true; break;
                case 2: indicatorTab2.BackgroundColor = Color.FromHex(indicatorColor); preparationTab.IsVisible = true; break;
                case 3: indicatorTab3.BackgroundColor = Color.FromHex(indicatorColor); commentTab.IsVisible = true; break;
            }

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