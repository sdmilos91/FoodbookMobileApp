using Foodbook.MobileApp.Data.Models;
using Foodbook.MobileApp.Data.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Foodbook.MobileApp.ViewModels
{
    public class AddRecipeViewModel : BaseViewModel
    {
        public PostRecipeModel DataModel { get; set; }

        public ObservableCollection<FoodCategoryModel> Categories { get; set; }
        public int SelectedCategory { get; set; }

        public ObservableCollection<CuisineModel> Cuisines { get; set; }
        public int SelectedCuisine { get; set; }

        public ObservableCollection<CaloricityModel> Caloricities { get; set; }
        public int SelectedCaloricity { get; set; }

        private RecipeCommonDataModel mCommonData;

        public Command SaveCommand { get; }

        public AddRecipeViewModel(RecipeCommonDataModel commonData)
        {
            DataModel = new PostRecipeModel();
            SaveCommand = new Command(() => SaveRecipe());

            Categories = new ObservableCollection<FoodCategoryModel>(commonData.Categories);
            SelectedCategory = 0;

            Cuisines = new ObservableCollection<CuisineModel>(commonData.Cuisines);
            SelectedCuisine = 0;

            Caloricities = new ObservableCollection<CaloricityModel>(commonData.Caloricities);
            SelectedCaloricity = 0;

            mCommonData = commonData;
        }

        private async void SaveRecipe()
        {
            DataModel.CategoryId = mCommonData.Categories.ElementAt(SelectedCategory).CategoryId;
            DataModel.CuisineId = mCommonData.Cuisines.ElementAt(SelectedCuisine).CuisineId;
            DataModel.CaloricityId = mCommonData.Caloricities.ElementAt(SelectedCaloricity).CaloricityId;

            bool result = await RecipeDataService.AddRecipe(DataModel, "JK9cXlAXRIx9rw3cvgmzULe1O1za0YPq6dl-7APf3AxALPfmImshZkFSRouRUV_jSH3rMkgoE2DzXsaki5PcfcFtjueiYq7tG9YxUEEibp_gMdBRY7kSDWk5vZioBCPHRn4ljz4I1bTdDoQYya1pTPrc-05Hqno2U-X-b9MP43hJjwPIc4jXZJcH31EV9Y79Zd523QZpTobfkVCGAYpigHn5CEcOxEdsEjW7aTQnAJTbiQryaUgWACRMSGyrO2F49BfBB20iELYdDQgdPcenSXM3BKaGPAL0eYztLN7cgwJKlvB15mZ6ofYUyHo_9DbnY4VFlIsLWAJmW1wrJTvxlh4Xaj-ULUCqrJRrPjYWO4kE_4j-YUWBcsyEFdeMhKn4RES9jDYaBRniVaGwRX5IuYnxd3Iv5aEAeEGnyLqqXmJhg9ibjcQ975hFXir1PlwMNDHE0CaOk7ER2qzWGRVQx_ZIiAa1GkdCPBiHkxt2PxL27uu1v512qThXE5hDKn70");

            var i = 3;
            
        }
    }
}
