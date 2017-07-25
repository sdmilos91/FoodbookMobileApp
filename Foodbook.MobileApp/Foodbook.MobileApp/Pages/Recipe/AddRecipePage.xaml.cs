using Foodbook.MobileApp.Data.Models;
using Foodbook.MobileApp.Data.Services;
using Foodbook.MobileApp.ViewModels;
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
	public partial class AddRecipePage : ContentPage
	{
        private int mPageNumber;
		public AddRecipePage ()
		{
			InitializeComponent ();
            
            mPageNumber = 0;
		}

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            RecipeCommonDataModel commonData = await RecipeDataService.GetRecipeCommonDate();
            BindingContext = new AddRecipeViewModel(commonData);


        }

        private void NextButton_Clicked(object sender, EventArgs e)
        {
            mPageNumber++;
            SetPage();
        }

        private void SetPage()
        {
            stepOneContainer.IsVisible = false;
            stepTwoContainer.IsVisible = false;
            secondPageIndicator.BackgroundColor = Color.Gray;
            secondPageLine.BackgroundColor = Color.Gray;
            thirdPageIndicator.BackgroundColor = Color.Gray;
            thirdPageLine.BackgroundColor = Color.Gray;

            if (mPageNumber == 2)
            {
                stepOneContainer.IsVisible = true;
                secondPageIndicator.BackgroundColor = Color.FromHex("#FFD54F");
                secondPageLine.BackgroundColor = Color.FromHex("#FFD54F");
                thirdPageIndicator.BackgroundColor = Color.FromHex("#FFD54F");
                thirdPageLine.BackgroundColor = Color.FromHex("#FFD54F");
            }
            else if (mPageNumber == 1)
            {
                stepTwoContainer.IsVisible = true;
                secondPageIndicator.BackgroundColor = Color.FromHex("#FFD54F");
                secondPageLine.BackgroundColor = Color.FromHex("#FFD54F");
                
            }
            else if (mPageNumber == 0)
            {
                stepOneContainer.IsVisible = true;
            }
        }

        private void BackButton_Clicked(object sender, EventArgs e)
        {
            mPageNumber--;
            SetPage();
        }
    }
}