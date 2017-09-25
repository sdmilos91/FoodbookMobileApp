using Foodbook.MobileApp.Data.Models;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Foodbook.MobileApp.ViewModels
{
    public class AddIngredientViewModel : BaseViewModel
    {
        private string ingredientName;

        public string IngredientName
        {
            get { return ingredientName; }
            set { SetProperty(ref ingredientName, value); }
        }


        private string ingredientValue;

        public string IngredientValue
        {
            get { return ingredientValue; }
            set { SetProperty(ref ingredientValue, value); }
        }

        public Command AddIngredientCommand { get; }
        public Command CancelCommand { get; }

        public AddIngredientViewModel()
        {
            AddIngredientCommand = new Command(() => AddIngredient());
            CancelCommand = new Command(() => Cancel());
        }

        private async void AddIngredient()
        {
            bool isInvalidForm = false;

            if (string.IsNullOrEmpty(IngredientName))
            {
                IngredientName = "";
                isInvalidForm = true;
            }

            if (string.IsNullOrEmpty(IngredientValue))
            {
                IngredientValue = "";
                isInvalidForm = true;
            }

            if (!isInvalidForm)
            {
                Ingredient ingredient = new Ingredient
                {
                    Name = IngredientName,
                    Value = ingredientValue
                };

                MessagingCenter.Send(this, "INGREDIENT_ADDED", ingredient);

                MasterDetailPage masterPage = App.Current.MainPage as MasterDetailPage;
                await masterPage.Detail.Navigation.PopPopupAsync();
            }

            
        }

        private async void Cancel()
        {
            MasterDetailPage masterPage = App.Current.MainPage as MasterDetailPage;
            await masterPage.Detail.Navigation.PopPopupAsync();
        }

    }
}
