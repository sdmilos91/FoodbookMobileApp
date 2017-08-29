using Foodbook.MobileApp.Data.Models;
using Foodbook.MobileApp.Data.Services;
using Foodbook.MobileApp.Pages.Recipe;
using Foodbook.MobileApp.Tools;
using Foodbook.Pages;
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

        private List<ResponseCookModel> mResponseModel;

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

        public CooksPageViewModel()
        {
            Items = new ObservableCollection<ResponseCookModel>();
            InitData();
            
            ChangeTabCommand = new Command((x) => ChangeTab(x.ToString()));
            AddRecipeCommand = new Command(() => Addrecipe());

            MessagingCenter.Subscribe<UserDetailsPage, PostCookCommentModel>(this, "COOK_RATING_UPDATED", (sender, addedComment) =>
            {
                var temp = Items;
                var cook = temp.Where(x => x.CookId == addedComment.CookId).FirstOrDefault();
                cook.Comments.Add(new CookCommentModel
                {
                    CommentText = addedComment.CommentText,
                    InsertDate = addedComment.InsertDate,
                    Rating = addedComment.Rating,
                    CookName = addedComment.CookName,
                });

                cook.Rating = cook.Comments.Average(x => x.Rating);
                Items = new ObservableCollection<ResponseCookModel>(temp);
                OnPropertyChanged("Items");

            });

            MessagingCenter.Subscribe<UserDetailsViewModel, ResponseCookModel>(this, "FAVOURITE", (sender, cook) =>
            {                
                //Refresh favourite recipes tab
                if (mSelectedTab == 2)
                {
                    Items = new ObservableCollection<ResponseCookModel>(mResponseModel.Where(x => x.IsFollowed));
                    OnPropertyChanged("Items");
                }
            });
        }



        private void ChangeTab(string tab)
        {
            FirstTabColor = Color.FromHex("#effcea");
            FirstTabTextColor = Color.Gray;
            FirstTabIndicatorColor = Color.FromHex("#effcea");

            SecondTabColor = Color.FromHex("#effcea");
            SecondTabTextColor = Color.Gray;
            SecondTabIndicatorColor = Color.FromHex("#effcea");

            FirstContainer = false;
            SecondContainer = false;


            switch (tab)
            {
                case "1":
                    mSelectedTab = 1;
                    FirstTabTextColor = Color.Green;
                    FirstTabIndicatorColor = Color.Green;
                    FirstContainer = true;
                    Items = new ObservableCollection<ResponseCookModel> (mResponseModel);
                    break;

                case "2":
                    mSelectedTab = 2;
                    SecondTabTextColor = Color.Green;
                    SecondTabIndicatorColor = Color.Green;
                    SecondContainer = true;
                    Items = new ObservableCollection<ResponseCookModel>(mResponseModel.Where(x => x.IsFollowed));
                    break;
               
                default:
                    mSelectedTab = 1;
                    FirstTabTextColor = Color.Green;
                    FirstTabIndicatorColor = Color.Green;
                    Items = new ObservableCollection<ResponseCookModel>(mResponseModel);
                    break;
            }
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

            mResponseModel = await CookDataService.GetCooks(LocalDataSecureStorage.GetToken());

            if (mResponseModel != null)
                Items = new ObservableCollection<ResponseCookModel>(mResponseModel);

            
            TabContainerHeight = IsUserAuthenticated ? new GridLength(40) : new GridLength(0);

            Device.BeginInvokeOnMainThread(() => Dialogs.Hide());

            ChangeTab("1");

        }

        private void Addrecipe()
        {
            MasterDetailPage masterPage = App.Current.MainPage as MasterDetailPage;
            masterPage.Detail.Navigation.PushAsync(new AddRecipePage());
        }
    }
}
