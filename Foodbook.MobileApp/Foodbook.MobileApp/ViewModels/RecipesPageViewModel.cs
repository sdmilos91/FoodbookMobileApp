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

        private ResponseRecipeModel mResponseModel;
        private ObservableCollection<RecipeDataModel> mUnfilteredItems;
        private RecipeCommonDataFilterModel FilterModel;

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

        public RecipesPageViewModel()
        {
            mResponseModel = new ResponseRecipeModel();
            InitData();
            
            ChangeTabCommand = new Command((x) => ChangeTab(x.ToString()));
            AddRecipeCommand = new Command(() => Addrecipe());
            FilterRecipeCommand = new Command((x) => FilterRecipeAsync(x));

            MessagingCenter.Subscribe<RecipeDetailViewModel, PostRecipeCommentModel>(this, "RATING_UPDATED", (sender, addedComment) =>
            {
                var temp = Items;
                var recipe = temp.Where(x => x.RecipeId == addedComment.RecipeId).FirstOrDefault();
                recipe.Comments.Add(new Comment
                {
                    CommentText = addedComment.CommentText,
                    InsertDate = addedComment.InsertDate,
                    Rating = addedComment.Rating,
                    CookName = addedComment.CookName,
                    CookPhotoUrl = LocalDataSecureStorage.GetCookPhoto()
                });

                recipe.Rating = recipe.Comments.Average(x => x.Rating);
                Items = new ObservableCollection<RecipeDataModel>(temp);
                OnPropertyChanged("Items");
                
            });

            MessagingCenter.Subscribe<RecipeDetailViewModel, long>(this, "RECIPE_DELETED", (sender, recipeId) => {

                RecipeDataModel recipe = Items.FirstOrDefault(x => x.RecipeId == recipeId);
                Items.Remove(recipe);

            });


            MessagingCenter.Subscribe<RecipeDetailViewModel, RecipeDataModel>(this, "FAVOURITE", (sender, recipe) =>
            {
                if (mResponseModel.FavouriteRecipes.Any(x => x.RecipeId == recipe.RecipeId))
                {
                    mResponseModel.FavouriteRecipes.Remove(recipe);
                }
                else
                {
                    mResponseModel.FavouriteRecipes.Add(recipe);
                }

                //Refresh favourite recipes tab
                if (mSelectedTab == 2)
                {
                    Items = new ObservableCollection<RecipeDataModel>(mResponseModel.FavouriteRecipes);
                    OnPropertyChanged("Items");
                }
            });

            MessagingCenter.Subscribe<RecipeFilterViewModel, RecipeCommonDataFilterModel>(this, "FILTERED", (sender, filterModel) => {

                var filteredItems = mUnfilteredItems.Where(x =>
                        (!string.IsNullOrEmpty(filterModel.RecipeName) ? x.Name.ToLower().Contains(filterModel.RecipeName.ToLower()) : true) &&
                        (filterModel.SelectedCategory.HasValue ? x.CategoryId == filterModel.SelectedCategory.Value : true) &&
                        (filterModel.SelectedCuisine.HasValue ? x.CuisineId == filterModel.SelectedCuisine.Value : true) &&
                        (filterModel.SelectedCaloricity.HasValue ? x.CaloricityId == filterModel.SelectedCaloricity.Value : true)
                    );

                Items = new ObservableCollection<RecipeDataModel>(filteredItems);
                FilterModel = filterModel;

            });
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
                    mSelectedTab = 1;
                    FirstTabIndicatorColor = indicatorColor;
                    FirstTabTextColor = indicatorColor;
                    FirstContainer = true;
                    Items = new ObservableCollection<RecipeDataModel> (mResponseModel.MyRecipes);
                    break;

                case "2":
                    mSelectedTab = 2;
                    SecondTabIndicatorColor = indicatorColor;
                    SecondTabTextColor = indicatorColor;
                    SecondContainer = true;
                    Items = new ObservableCollection<RecipeDataModel>(mResponseModel.FavouriteRecipes);
                    break;

                case "3":
                    mSelectedTab = 3;
                    ThirdTabIndicatorColor = indicatorColor;
                    ThirdTabTextColor = indicatorColor;
                    Items = new ObservableCollection<RecipeDataModel>(mResponseModel.AllRecipes);
                    break;
                default:
                    mSelectedTab = 1;
                    FirstTabIndicatorColor = indicatorColor;
                    FirstTabTextColor = indicatorColor;
                    Items = new ObservableCollection<RecipeDataModel>(mResponseModel.MyRecipes);
                    break;
            }

            mUnfilteredItems = Items;
            FilterModel = new RecipeCommonDataFilterModel();
        }

        private async void InitData()
        {


            Device.BeginInvokeOnMainThread(() => Dialogs.Show());

            RequestRecipeModel requestModel = new RequestRecipeModel
            {
                JustsAllRecipes = false,
                Token = LocalDataSecureStorage.GetToken()
            };

            string token = LocalDataSecureStorage.GetToken();
            IsUserAuthenticated = await AccountDataService.IsUserAuthenticated(token);

            mResponseModel = await RecipeDataService.GetRecipes(requestModel);

            if (mResponseModel != null)
                Items = IsUserAuthenticated ? new ObservableCollection<RecipeDataModel>(mResponseModel.MyRecipes) : new ObservableCollection<RecipeDataModel>(mResponseModel.AllRecipes);

            mUnfilteredItems = Items;
           
            if (IsUserAuthenticated)
            {
                TabContainerHeight = new GridLength(40);
                ChangeTab("1");
            }
            else
            {
                TabContainerHeight = new GridLength(0);
            }

            Device.BeginInvokeOnMainThread(() => Dialogs.Hide());
        }

        private void Addrecipe()
        {
            MasterDetailPage masterPage = App.Current.MainPage as MasterDetailPage;
            masterPage.Detail.Navigation.PushAsync(new AddRecipePage());
        }

        private async void FilterRecipeAsync(object sender)
        {
            Utils.ButtonPress(sender);
           

            if (FilterModel == null)
            {
                FilterModel = new RecipeCommonDataFilterModel();
            }

            MasterDetailPage master = App.Current.MainPage as MasterDetailPage;
            await master.Detail.Navigation.PushPopupAsync(new RecipeFilerPopupPage(FilterModel));
            Device.BeginInvokeOnMainThread(() => Dialogs.Hide());
        }
    }
}
