using Foodbook.MobileApp.Data.Models;
using Foodbook.MobileApp.Data.Services;
using Foodbook.MobileApp.Tools;
using Foodbook.MobileApp.ViewModels;
using Plugin.Media;
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
		public AddRecipePage ()
		{
			InitializeComponent ();

            AddRecipeViewModel viewModel = new AddRecipeViewModel();
            BindingContext = viewModel;
		}

        private void ingredientsList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }
    }
}