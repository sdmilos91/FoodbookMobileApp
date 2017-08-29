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
	public partial class EditRecipePage : ContentPage
	{
		public EditRecipePage(RecipeDataModel recipe)
		{
			InitializeComponent ();

            EditRecipeViewModel viewModel = new EditRecipeViewModel(recipe);
            BindingContext = viewModel;
		}
    }
}