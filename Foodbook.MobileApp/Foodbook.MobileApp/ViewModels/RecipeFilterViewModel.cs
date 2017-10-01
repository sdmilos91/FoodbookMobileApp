using Foodbook.MobileApp.Data.Models;
using Foodbook.MobileApp.Data.Services;
using Foodbook.MobileApp.Tools;
using Realms;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Foodbook.MobileApp.ViewModels
{
    public class RecipeFilterViewModel : BaseViewModel
    {
        public RecipeCommonDataModel CommonData { get; set; }
        
        private RecipeCommonDataFilterModel filterModel;

        public RecipeCommonDataFilterModel FilterModel
        {
            get { return filterModel; }
            set { SetProperty(ref filterModel, value); }
        }

        public string CategoryName { get; set; }
        public int SelectedCategoryIndex { get; set; }

        public string CuisineName { get; set; }
        public int SelectedCuisineIndex { get; set; }

        public string CaloryName { get; set; }
        public int SelectedCaloricityIndex { get; set; }

        public Command CancelCommand { get; }
        public Command FilterCommand { get; }

        public RecipeFilterViewModel(RecipeCommonDataFilterModel filterData)
        {           
            FilterModel = filterData;
            InitData();

            CancelCommand = new Command(() => Cancel());
            FilterCommand = new Command(() => Filter());
        }

        private async void InitData()
        {
            
            RecipeCommonDataModel commonData = await DataMockup.GetRecipeCommonData();

            CommonData = commonData;
            CommonData.Categories.Insert(0, new FoodCategoryModel
            {
                CategoryId = -1,
                CategoryName = "Sve kategorije"
            });

            CommonData.Cuisines.Insert(0, new CuisineModel
            {
                CuisineId = -1,
                CuisineName = "Sve kuhinje"
            });

            CommonData.Caloricities.Insert(0, new CaloricityModel
            {
                CaloricityId = -1,
                Name = "Sve kaloričnosti"
            });

            SelectedCategoryIndex = FilterModel.SelectedCategory.HasValue ? CommonData.Categories.IndexOf(CommonData.Categories.FirstOrDefault(x => x.CategoryId == FilterModel.SelectedCategory)) : 0;
            SelectedCuisineIndex = FilterModel.SelectedCuisine.HasValue ? CommonData.Cuisines.IndexOf(CommonData.Cuisines.FirstOrDefault(x => x.CuisineId == FilterModel.SelectedCuisine)) : 0;
            SelectedCaloricityIndex = FilterModel.SelectedCaloricity.HasValue ? CommonData.Caloricities.IndexOf(CommonData.Caloricities.FirstOrDefault(x => x.CaloricityId == FilterModel.SelectedCaloricity)) : 0;


            CategoryName = CommonData.Categories.ElementAt(SelectedCategoryIndex).CategoryName;
            CuisineName = CommonData.Cuisines.ElementAt(SelectedCuisineIndex).CuisineName;
            CaloryName = CommonData.Caloricities.ElementAt(SelectedCaloricityIndex).Name;
            
        }

        private async void Cancel()
        {
            MasterDetailPage masterPage = App.Current.MainPage as MasterDetailPage;
            await masterPage.Detail.Navigation.PopPopupAsync();
        }

        private async void Filter()
        {
            FilterModel.SelectedCategory = SelectedCategoryIndex != 0 ? (long?)CommonData.Categories.ElementAt(SelectedCategoryIndex).CategoryId : null;
            FilterModel.SelectedCuisine = SelectedCuisineIndex != 0 ? (long?)CommonData.Cuisines.ElementAt(SelectedCuisineIndex).CuisineId : null;
            FilterModel.SelectedCaloricity = SelectedCaloricityIndex != 0 ? (long?)CommonData.Caloricities.ElementAt(SelectedCaloricityIndex).CaloricityId : null;

            MessagingCenter.Send(this, "FILTERED", FilterModel);

            MasterDetailPage masterPage = App.Current.MainPage as MasterDetailPage;
            await masterPage.Detail.Navigation.PopPopupAsync();
        }
    }
}
