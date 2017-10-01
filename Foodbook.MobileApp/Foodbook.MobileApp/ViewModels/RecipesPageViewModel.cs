using Foodbook.MobileApp.Data.Models;
using Foodbook.MobileApp.Data.Services;
using Foodbook.MobileApp.Helpers;
using Foodbook.MobileApp.Pages.Recipe;
using Foodbook.MobileApp.Tools;
using Realms;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Foodbook.MobileApp.ViewModels
{
    class RecipesPageViewModel : BaseViewModel
    {        
        private RecipeCommonDataFilterModel FilterModel;
        private CommonDataSortModel OrderModel;

        private ObservableCollection<RecipeDataModel> items;

        public ObservableCollection<RecipeDataModel> Items
        {
            get { return items; }
            set { SetProperty(ref items, value); }
        }

        #region TAB

        private int mSelectedTab;

        private Color firstTabColor;

        public Color FirstTabColor
        {
            get { return firstTabColor; }
            set { SetProperty(ref firstTabColor, value); }
        }

        private Color firstTabTextColor;

        public Color FirstTabTextColor
        {
            get { return firstTabTextColor; }
            set { SetProperty(ref firstTabTextColor, value); }
        }

        private Color firstTabIndicatorColor;

        public Color FirstTabIndicatorColor
        {
            get { return firstTabIndicatorColor; }
            set { SetProperty(ref firstTabIndicatorColor, value); }
        }


        private Color secondTabColor;

        public Color SecondTabColor
        {
            get { return secondTabColor; }
            set { SetProperty(ref secondTabColor, value); }
        }

        private Color secondTabTextColor;

        public Color SecondTabTextColor
        {
            get { return secondTabTextColor; }
            set { SetProperty(ref secondTabTextColor, value); }
        }

        private Color secondTabIndicatorColor;

        public Color SecondTabIndicatorColor
        {
            get { return secondTabIndicatorColor; }
            set { SetProperty(ref secondTabIndicatorColor, value); }
        }


        private Color thirdTabColor;

        public Color ThirdTabColor
        {
            get { return thirdTabColor; }
            set { SetProperty(ref thirdTabColor, value); }
        }

        private Color thirdTabTextColor;

        public Color ThirdTabTextColor
        {
            get { return thirdTabTextColor; }
            set { SetProperty(ref thirdTabTextColor, value); }
        }

        private Color thirdTabIndicatorColor;

        public Color ThirdTabIndicatorColor
        {
            get { return thirdTabIndicatorColor; }
            set { SetProperty(ref thirdTabIndicatorColor, value); }
        }

        private bool firstContainer;

        public bool FirstContainer
        {
            get { return firstContainer; }
            set { SetProperty(ref firstContainer, value); }
        }

        private bool secondContainer;

        public bool SecondContainer
        {
            get { return secondContainer; }
            set { SetProperty(ref secondContainer, value); }
        }





        #endregion
        
        private GridLength tabContainerHeight;

        public GridLength TabContainerHeight
        {
            get { return tabContainerHeight; }
            set { SetProperty(ref tabContainerHeight, value); }
        }

        private bool isUserAuthenticated;

        public bool IsUserAuthenticated
        {
            get { return isUserAuthenticated; }
            set { SetProperty(ref isUserAuthenticated, value); }
        }


        public Command ChangeTabCommand { get; }

        public Command AddRecipeCommand { get;}

        public Command FilterRecipeCommand { get; }

        public Command SortRecipeCommand { get; }

        public RecipesPageViewModel()
        {
            InitData();
            
            ChangeTabCommand = new Command((x) => ChangeTab(x.ToString()));
            AddRecipeCommand = new Command(() => Addrecipe());
            FilterRecipeCommand = new Command((x) => FilterRecipeAsync(x));
            SortRecipeCommand = new Command((x) => SortRecipeAsync(x));
        }

        private void ChangeTab(string tab)
        {
            Color mainColor = Color.FromHex(MyColors.LIGHT_GREEN);
            Color indicatorColor = Color.FromHex(MyColors.GREEN);

            FirstTabColor = mainColor;
            FirstTabIndicatorColor = mainColor;
            FirstTabTextColor = Color.Gray;

            SecondTabColor = mainColor;
            SecondTabIndicatorColor = mainColor;
            SecondTabTextColor = Color.Gray;

            ThirdTabColor = mainColor;
            ThirdTabIndicatorColor = mainColor;
            ThirdTabTextColor = Color.Gray;

            FirstContainer = false;
            SecondContainer = false;


            switch (tab)
            {
                case "1":
                    mSelectedTab = RecipesPageTabs.MY_RECIPES;
                    FirstTabIndicatorColor = indicatorColor;
                    FirstTabTextColor = indicatorColor;
                    FirstContainer = true;
                    
                    break;

                case "2":
                    mSelectedTab = RecipesPageTabs.FAVORITE_RECIPES;
                    SecondTabIndicatorColor = indicatorColor;
                    SecondTabTextColor = indicatorColor;
                    SecondContainer = true;

                    break;

                case "3":
                    mSelectedTab = RecipesPageTabs.ALL_RECIPES;
                    ThirdTabIndicatorColor = indicatorColor;
                    ThirdTabTextColor = indicatorColor;

                    break;
                default:
                    mSelectedTab = RecipesPageTabs.MY_RECIPES;
                    FirstTabIndicatorColor = indicatorColor;
                    FirstTabTextColor = indicatorColor;

                    break;
            }

            Items = new ObservableCollection<RecipeDataModel>(DataMockup.GetRecipesByType(mSelectedTab));
            FilterModel = new RecipeCommonDataFilterModel();
            OrderModel = new CommonDataSortModel();
        }

        private async void InitData()
        {


            //Device.BeginInvokeOnMainThread(() => Dialogs.Show());

            RequestRecipeModel requestModel = new RequestRecipeModel
            {
                JustsAllRecipes = false,
                Token = LocalDataSecureStorage.GetToken()
            };

            string token = LocalDataSecureStorage.GetToken();
            IsUserAuthenticated = await AccountDataService.IsUserAuthenticated(token);

            ResponseRecipeModel mResponseModel = await RecipeDataService.GetRecipes(requestModel);

            DataMockup.SaveRecipes(mResponseModel);

            if (mResponseModel != null)
                Items = IsUserAuthenticated ? new ObservableCollection<RecipeDataModel>(mResponseModel.MyRecipes) : new ObservableCollection<RecipeDataModel>(mResponseModel.AllRecipes);
           
            if (IsUserAuthenticated)
            {
                TabContainerHeight = new GridLength(40);
                ChangeTab(RecipesPageTabs.MY_RECIPES.ToString());
            }
            else
            {
                TabContainerHeight = new GridLength(0);
            }

            //Device.BeginInvokeOnMainThread(() => Dialogs.Hide());
        }

        private void Addrecipe()
        {
            MasterDetailPage masterPage = App.Current.MainPage as MasterDetailPage;
            masterPage.Detail.Navigation.PushAsync(new AddRecipePage());
        }

        private async void FilterRecipeAsync(object sender)
        {
            Utils.ButtonPress(sender);

            MessagingCenter.Subscribe<RecipeFilterViewModel, RecipeCommonDataFilterModel>(this, "FILTERED", (s, filterModel) => {

                var filteredItems = DataMockup.GetRecipesByType(mSelectedTab).Where(x =>
                        (!string.IsNullOrEmpty(filterModel.RecipeName) ? x.Name.ToLower().Contains(filterModel.RecipeName.ToLower()) : true) &&
                        (filterModel.SelectedCategory.HasValue ? x.CategoryId == filterModel.SelectedCategory.Value : true) &&
                        (filterModel.SelectedCuisine.HasValue ? x.CuisineId == filterModel.SelectedCuisine.Value : true) &&
                        (filterModel.SelectedCaloricity.HasValue ? x.CaloricityId == filterModel.SelectedCaloricity.Value : true)
                    );

                Items = new ObservableCollection<RecipeDataModel>(filteredItems);
                FilterModel = filterModel;
                MessagingCenter.Unsubscribe<RecipeFilterViewModel, RecipeCommonDataFilterModel>(this, "FILTERED");
            });

            if (FilterModel == null)
            {
                FilterModel = new RecipeCommonDataFilterModel();
            }

            MasterDetailPage master = App.Current.MainPage as MasterDetailPage;
            await master.Detail.Navigation.PushPopupAsync(new RecipeFilerPopupPage(FilterModel));
            HideDialog();


        }

        private async void SortRecipeAsync(object sender)
        {
            Utils.ButtonPress(sender);

            MessagingCenter.Subscribe<RecipeSortViewModel, CommonDataSortModel>(this, "SORTED", (s, sortModel) => {

                List<RecipeDataModel> sortedItems = new List<RecipeDataModel>();
                switch (sortModel.OrderById)
                {
                    case RecipeSort.NAME:
                        if (sortModel.OrderId == RecipeSort.ORDER_ASC)
                            sortedItems = Items.OrderBy(x => x.Name).ToList();
                        else
                            sortedItems = Items.OrderByDescending(x => x.Name).ToList();
                        break;
                    case RecipeSort.RATING:
                        if (sortModel.OrderId == RecipeSort.ORDER_ASC)
                            sortedItems = Items.OrderBy(x => x.Rating).ToList();
                        else
                            sortedItems = Items.OrderByDescending(x => x.Rating).ToList();
                        break;
                    case RecipeSort.PREPARATION_TIME:
                        if (sortModel.OrderId == RecipeSort.ORDER_ASC)
                            sortedItems = Items.OrderBy(x => x.PreparationTime).ToList();
                        else
                            sortedItems = Items.OrderByDescending(x => x.PreparationTime).ToList();
                        break;
                    default:
                        if (sortModel.OrderId == RecipeSort.ORDER_ASC)
                            sortedItems = Items.OrderBy(x => x.Name).ToList();
                        else
                            sortedItems = Items.OrderByDescending(x => x.Name).ToList();
                        break;
                }

                Items = new ObservableCollection<RecipeDataModel>(sortedItems);
                OrderModel = sortModel;

                MessagingCenter.Unsubscribe<RecipeSortViewModel, CommonDataSortModel>(this, "SORTED");
                
            });

            if (OrderModel == null)
            {
                OrderModel = new CommonDataSortModel();
            }

            MasterDetailPage master = App.Current.MainPage as MasterDetailPage;
            await master.Detail.Navigation.PushPopupAsync(new RecipeSortPopupPage(OrderModel));
            HideDialog();
        }

        public override void OnViewAppearing()
        {
            base.OnViewAppearing();
            Items = new ObservableCollection<RecipeDataModel>(DataMockup.GetRecipesByType(mSelectedTab));
            OnPropertyChanged("Items");
        }
    }
}
