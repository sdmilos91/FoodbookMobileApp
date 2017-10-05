using Foodbook.MobileApp.Data.Models;
using Foodbook.MobileApp.Data.Services;
using Foodbook.MobileApp.Pages;
using Foodbook.MobileApp.Pages.Recipe;
using Foodbook.MobileApp.Tools;
using Foodbook.Pages;
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
    public class UserDetailsViewModel : BaseViewModel
    {
        private ResponseCookModel mCook;

        public ResponseCookModel Cook
        {
            get { return mCook; }
            set { SetProperty(ref mCook, value); }
        }


        public ObservableCollection<RecipeDataModel> Recipes { get; set; }
        public ObservableCollection<CookCommentModel> Comments { get; set; }

        public string[] Items { get; set; }

        private GridLength imageContainerHeight;

        private double? rating;

        public double? Rating
        {
            get { return rating; }
            set { SetProperty(ref rating, value); }
        }

        public bool AddCommentEnabled { get; set; }

        public Command SwitchTabCommand { get; }
        public Command ViewImageCommand { get; }
        public Command EditCookCommand { get; }
        public Command FavouriteCookCommand { get; }
        public Command AddCommentCommand { get; }
        public Command CommentAddedCommand { get; }

        public GridLength ImageContainerHeight
        {
            get { return imageContainerHeight; }
            set { imageContainerHeight = value; OnPropertyChanged(); }
        }

        #region TAB

        private Color firstTabColor;

        public Color FirstTabColor
        {
            get { return firstTabColor; }
            set { firstTabColor = value; OnPropertyChanged(); }
        }

        private Color firstTabTextColor;

        public Color FirstTabTextColor
        {
            get { return firstTabTextColor; }
            set { firstTabTextColor = value; OnPropertyChanged(); }
        }

        private Color firstTabIndicatorColor;

        public Color FirstTabIndicatorColor
        {
            get { return firstTabIndicatorColor; }
            set { firstTabIndicatorColor = value; OnPropertyChanged(); }
        }


        private Color secondTabColor;

        public Color SecondTabColor
        {
            get { return secondTabColor; }
            set { secondTabColor = value; OnPropertyChanged(); }
        }

        private Color secondTabTextColor;

        public Color SecondTabTextColor
        {
            get { return secondTabTextColor; }
            set { secondTabTextColor = value; OnPropertyChanged(); }
        }

        private Color secondTabIndicatorColor;

        public Color SecondTabIndicatorColor
        {
            get { return secondTabIndicatorColor; }
            set { secondTabIndicatorColor = value; OnPropertyChanged(); }
        }


        private Color thirdTabColor;

        public Color ThirdTabColor
        {
            get { return thirdTabColor; }
            set { thirdTabColor = value; OnPropertyChanged(); }
        }

        private Color thirdTabTextColor;

        public Color ThirdTabTextColor
        {
            get { return thirdTabTextColor; }
            set { thirdTabTextColor = value; OnPropertyChanged(); }
        }

        private Color sthirdTabIndicatorColor;

        public Color ThirdTabIndicatorColor
        {
            get { return sthirdTabIndicatorColor; }
            set { sthirdTabIndicatorColor = value; OnPropertyChanged(); }
        }

        private bool firstContainer;

        public bool FirstContainer
        {
            get { return firstContainer; }
            set { firstContainer = value; OnPropertyChanged(); }
        }

        private bool secondContainer;

        public bool SecondContainer
        {
            get { return secondContainer; }
            set { secondContainer = value; OnPropertyChanged(); }
        }

        private bool thirdContainer;

        public bool ThirdContainer
        {
            get { return thirdContainer; }
            set { thirdContainer = value; OnPropertyChanged(); }
        }




        #endregion

        public UserDetailsViewModel(ResponseCookModel cook, bool userProfile = false)
        {
            Cook = cook;
            Recipes = new ObservableCollection<RecipeDataModel>(cook.Recipes);
            Comments = new ObservableCollection<CookCommentModel>(cook.Comments);
            ImageContainerHeight = new GridLength(1, GridUnitType.Star);
            AddCommentEnabled = !userProfile;
            SwitchTabCommand = new Command((x) => SwitchTab(x.ToString()));
            ViewImageCommand = new Command(() => ViewImage(cook.PhotoUrl));
            FavouriteCookCommand = new Command((x) => FavouriteCook(x, cook));
            AddCommentCommand = new Command(() => AddComment(cook.CookId));
            CommentAddedCommand = new Command(() => CommentAdded());
            SwitchTab("1");

            MessagingCenter.Subscribe<EditUserDetailsPageViewModel, ResponseCookModel>(this, "USER_EDIITED", (sender, args) =>
            {
                Cook = args;
                OnPropertyChanged("Cook");
            });
        }



        private void SwitchTab(string tab)
        {

            Color mainColor = Color.FromHex(MyColors.LIGHT_GREEN);
            Color indicatorColor = Color.FromHex(MyColors.DARK_RED);
            Color textColor = Color.FromHex(MyColors.TEXT_PRIMARY);

            FirstTabColor = mainColor;
            FirstTabIndicatorColor = mainColor;
            FirstTabTextColor = textColor;

            SecondTabColor = mainColor;
            SecondTabIndicatorColor = mainColor;
            SecondTabTextColor = textColor;

            ThirdTabColor = mainColor;
            ThirdTabIndicatorColor = mainColor;
            ThirdTabTextColor = textColor;

            FirstContainer = false;
            SecondContainer = false;
            ThirdContainer = false;


            switch (tab)
            {
                case "1":

                    FirstTabIndicatorColor = indicatorColor;
                    //FirstTabTextColor = indicatorColor;
                    FirstContainer = true;
                    ImageContainerHeight = new GridLength(1, GridUnitType.Star);
                    break;

                case "2":
                    SecondTabIndicatorColor = indicatorColor;
                   // SecondTabTextColor = indicatorColor;
                    SecondContainer = true;
                    ImageContainerHeight = new GridLength(0, GridUnitType.Star);
                    break;

                case "3":
                    ThirdTabIndicatorColor = indicatorColor;
                    //ThirdTabTextColor = indicatorColor;
                    ThirdContainer = true;
                    ImageContainerHeight = new GridLength(0, GridUnitType.Star);
                    break;
                default:
                    FirstTabIndicatorColor = indicatorColor;
                    //FirstTabTextColor = indicatorColor;
                    FirstContainer = true;
                    ImageContainerHeight = new GridLength(1, GridUnitType.Star);
                    break;


            }
        }

        private void ViewImage(string imageUrl)
        {
            MasterDetailPage masterPage = App.Current.MainPage as MasterDetailPage;
            masterPage.Detail.Navigation.PushModalAsync(new ImageViewPage(imageUrl));
        }

        private async void FavouriteCook(object sender, ResponseCookModel cook)
        {
            var favouriteItem = sender as ToolbarItem;
            ShowDialog();
            bool result = await CookDataService.FavouriteCook(cook.CookId, LocalDataSecureStorage.GetToken());
            HideDialog();
            if (result)
            {
                DataMockup.AddOrRemoveFavoutiteCook(cook);
                favouriteItem.Icon = cook.IsFollowed ? "favorited" : "favorite";
                MessagingCenter.Send(this, "FAVOURITE");
            }
        }

        private void AddComment(long cookId)
        {
            MasterDetailPage master = App.Current.MainPage as MasterDetailPage;

            master.Detail.Navigation.PushPopupAsync(new AddCommentPopupPage(cookId, true));

        }

        private void CommentAdded()
        {
            Comments =  new ObservableCollection<CookCommentModel>(DataMockup.GetCookById(mCook.CookId).Comments);
            OnPropertyChanged("Comments");

            Rating = Comments.Average(x => x.Rating);
            OnPropertyChanged("Rating");            

        }
    }
}
