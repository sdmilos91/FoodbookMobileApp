using Foodbook.MobileApp.Data.Models;
using Foodbook.MobileApp.Data.Services;
using Foodbook.MobileApp.Helpers;
using Foodbook.MobileApp.Pages.Cook;
using Foodbook.MobileApp.Pages.Recipe;
using Foodbook.MobileApp.Tools;
using Foodbook.Pages;
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
    class CooksPageViewModel : BaseViewModel
    {        
        private string mCookNameFilter;
        private CommonDataSortModel OrderModel;

        private ObservableCollection<ResponseCookModel> items;

        public ObservableCollection<ResponseCookModel> Items
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

        public CooksPageViewModel()
        {
            Items = new ObservableCollection<ResponseCookModel>();
            InitData();
            
            ChangeTabCommand = new Command((x) => ChangeTab(x.ToString()));
            FilterRecipeCommand = new Command((x) => FilterRecipeAsync(x));
            SortRecipeCommand = new Command((x) => SortRecipeAsync(x));

            MessagingCenter.Subscribe<UserDetailsPage>(this, "COOK_RATING_UPDATED", (sender) =>
            {
                Items = new ObservableCollection<ResponseCookModel>(DataMockup.GetCooksByType(mSelectedTab));
                OnPropertyChanged("Items");

            });

            MessagingCenter.Subscribe<UserDetailsViewModel>(this, "FAVOURITE", (sender) =>
            {                
                
                    Items = new ObservableCollection<ResponseCookModel>(DataMockup.GetCooksByType(mSelectedTab));
                    OnPropertyChanged("Items");                
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

            FirstContainer = false;
            SecondContainer = false;


            switch (tab)
            {
                case "1":
                    mSelectedTab = CooksPageTabs.ALL_COOKS;
                    FirstTabIndicatorColor = indicatorColor;
                    FirstTabTextColor = indicatorColor;
                    FirstContainer = true;
                   
                    break;

                case "2":
                    mSelectedTab = CooksPageTabs.FAVORITE_COOKS;
                    SecondTabIndicatorColor = indicatorColor;
                    SecondTabTextColor = indicatorColor;
                    SecondContainer = true;
 
                    break;
               
                default:
                    mSelectedTab = CooksPageTabs.ALL_COOKS;
                    FirstTabIndicatorColor = indicatorColor;
                    FirstTabTextColor = indicatorColor;

                    break;
            }

            Items = new ObservableCollection<ResponseCookModel>(DataMockup.GetCooksByType(mSelectedTab));
        }

        private async void InitData()
        {


           // Device.BeginInvokeOnMainThread(() => Dialogs.Show());

            RequestRecipeModel requestModel = new RequestRecipeModel
            {
                JustsAllRecipes = false,
                Token = LocalDataSecureStorage.GetToken()
            };

            string token = LocalDataSecureStorage.GetToken();
            IsUserAuthenticated = await AccountDataService.IsUserAuthenticated(token);

            var mResponseModel = await CookDataService.GetCooks(LocalDataSecureStorage.GetToken());
            DataMockup.SaveCooks(new ObservableCollection<ResponseCookModel>(mResponseModel));

            if (mResponseModel != null)
                Items = new ObservableCollection<ResponseCookModel>(mResponseModel);

            
            TabContainerHeight = IsUserAuthenticated ? new GridLength(40) : new GridLength(0);

            //Device.BeginInvokeOnMainThread(() => Dialogs.Hide());

            ChangeTab("1");
        }

        private async void FilterRecipeAsync(object sender)
        {
            Utils.ButtonPress(sender);

            MessagingCenter.Subscribe<CookFilterViewModel, string>(this, "FILTERED", (s, cookName) => {

                var filteredItems = DataMockup.GetCooksByType(mSelectedTab).Where(x => x.FullName.ToLower().Contains(cookName.ToLower()));

                Items = new ObservableCollection<ResponseCookModel>(filteredItems);
                mCookNameFilter = cookName;
                MessagingCenter.Unsubscribe<CookFilterViewModel, string>(this, "FILTERED");
            });

            MasterDetailPage master = App.Current.MainPage as MasterDetailPage;
            await master.Detail.Navigation.PushPopupAsync(new CookFilterPopupPage(mCookNameFilter));
            HideDialog();
        }

        private async void SortRecipeAsync(object sender)
        {
            Utils.ButtonPress(sender);

            MessagingCenter.Subscribe<CookSortViewModel, CommonDataSortModel>(this, "SORTED", (s, sortModel) => {

                List<ResponseCookModel> sortedItems = new List<ResponseCookModel>();
                switch (sortModel.OrderById)
                {
                    case RecipeSort.NAME:
                        if (sortModel.OrderId == RecipeSort.ORDER_ASC)
                            sortedItems = Items.OrderBy(x => x.FullName).ToList();
                        else
                            sortedItems = Items.OrderByDescending(x => x.FullName).ToList();
                        break;
                    case RecipeSort.RATING:
                        if (sortModel.OrderId == RecipeSort.ORDER_ASC)
                            sortedItems = Items.OrderBy(x => x.Rating).ToList();
                        else
                            sortedItems = Items.OrderByDescending(x => x.Rating).ToList();
                        break;
                    default:
                        if (sortModel.OrderId == RecipeSort.ORDER_ASC)
                            sortedItems = Items.OrderBy(x => x.FullName).ToList();
                        else
                            sortedItems = Items.OrderByDescending(x => x.FullName).ToList();
                        break;
                }

                Items = new ObservableCollection<ResponseCookModel>(sortedItems);
                OrderModel = sortModel;

                MessagingCenter.Unsubscribe<CookSortViewModel, CommonDataSortModel>(this, "SORTED");

            });

            if (OrderModel == null)
            {
                OrderModel = new CommonDataSortModel();
            }

            MasterDetailPage master = App.Current.MainPage as MasterDetailPage;
            await master.Detail.Navigation.PushPopupAsync(new CookSortPopupPage(OrderModel));
            HideDialog();
        }

        public override void OnViewAppearing()
        {
            base.OnViewAppearing();
            Items = new ObservableCollection<ResponseCookModel>(DataMockup.GetCooksByType(mSelectedTab));
            OnPropertyChanged("Items");
        }
    }
}
