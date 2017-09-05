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
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Foodbook.MobileApp.ViewModels
{
    class RecipeDetailViewModel : BaseViewModel
    {
        private long mRecipeId;
        public string CookName { get; set; }
        public string Name { get; set; }
        public string CuisineName { get; set; }
        public string CategoryName { get; set; }
        public string RecipeText { get; set; }
        public string VideoUrl { get; set; }
        public string CaloricityName { get; set; }
        public DateTime InsertDate { get; set; }
        public int PreparationTime { get; set; }
        public string PhotoUrl { get; set; }
        public bool IsMine { get; set; }
        public bool IsFavourite { get; set; }
        public bool AddCommentEnabled { get; set; }

        private double? rating;

        public double? Rating
        {
            get { return rating; }
            set { SetProperty(ref rating, value); }
        }

        private ObservableCollection<Comment> comments;

        public ObservableCollection<Comment> Comments
        {
            get { return comments; }
            set { SetProperty(ref comments, value); }
        }

        private GridLength imageContainerHeight;

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

        public Command AddCommentCommand { get; }
        public Command CommentAddedCommant { get; }
        public Command SwitchTabCommand { get; }
        public Command ViewImageCommand { get; }
        public Command DeleteRecipeCommand { get;}
        public Command EditRecipeCommand { get; }
        public Command FavouriteRecipeCommand { get; }

        public RecipeDetailViewModel(RecipeDataModel recipe)
        {
            mRecipeId = recipe.RecipeId;
            CookName = recipe.CookName;
            Name = recipe.Name;
            CuisineName = recipe.CuisineName;
            CategoryName = recipe.CategoryName;
            RecipeText = recipe.RecipeText;
            VideoUrl = recipe.VideoUrl;
            CaloricityName = recipe.CaloricityName;
            InsertDate = recipe.InsertDate;
            Comments = new ObservableCollection<Comment>(recipe.Comments);
            Rating = recipe.Rating;
            PreparationTime = recipe.PreparationTime;
            PhotoUrl = recipe.ProfilePhotoUrl;
            IsMine = recipe.IsMine;
            IsFavourite = recipe.IsFavourite;
            AddCommentEnabled = !recipe.IsMine && !string.IsNullOrEmpty(LocalDataSecureStorage.GetToken());

            AddCommentCommand = new Command(() => AddComment());
            CommentAddedCommant = new Command((x) => CommentAdded(x));
            ViewImageCommand = new Command(() => ViewImage());
            ImageContainerHeight = new GridLength(1, GridUnitType.Star);
            SwitchTabCommand = new Command((x) => SwitchTab(x.ToString()));
            DeleteRecipeCommand = new Command(() => DeleteRecipe());
            EditRecipeCommand = new Command(() => EditRecipe(recipe));
            FavouriteRecipeCommand = new Command((x) => FavouriteRecipe(x, recipe));
            SwitchTab("1");

        }

        private void AddComment()
        {
            MasterDetailPage master = App.Current.MainPage as MasterDetailPage;

            master.Detail.Navigation.PushPopupAsync(new AddCommentPopupPage(mRecipeId));

        }

        private void CommentAdded(object model)
        {
            PostRecipeCommentModel commentModel = model as PostRecipeCommentModel;

            commentModel.CookName = LocalDataSecureStorage.GetCookName();

            Comment comment = new Comment
            {
                CommentText = commentModel.CommentText,
                InsertDate = commentModel.InsertDate,
                Rating = commentModel.Rating,
                CookName = LocalDataSecureStorage.GetCookName(),
                CookPhotoUrl = LocalDataSecureStorage.GetCookPhoto()
            };

            var temp = Comments;

            temp.Add(comment);
            Comments = temp;

            Rating = Comments.Average(x => x.Rating);
            OnPropertyChanged("Rating");
            

            MessagingCenter.Send(this, "RATING_UPDATED", commentModel);

        }

        private void SwitchTab(string tab)
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
            ThirdContainer = false;


            switch (tab)
            {
                case "1":

                    FirstTabIndicatorColor = indicatorColor;
                    FirstTabTextColor = indicatorColor;
                    FirstContainer = true;
                    ImageContainerHeight = new GridLength(1, GridUnitType.Star);
                    break;

                case "2":
                    SecondTabIndicatorColor = indicatorColor;
                    SecondTabTextColor = indicatorColor;
                    SecondContainer = true;
                    ImageContainerHeight = new GridLength(0, GridUnitType.Star);
                    break;

                case "3":
                    ThirdTabIndicatorColor = indicatorColor;
                    ThirdTabTextColor = indicatorColor;
                    ThirdContainer = true;
                    ImageContainerHeight = new GridLength(0, GridUnitType.Star);
                    break;
                default:
                    FirstTabIndicatorColor = indicatorColor;
                    FirstTabTextColor = indicatorColor;
                    FirstContainer = true;
                    ImageContainerHeight = new GridLength(1, GridUnitType.Star);
                    break;


            }
        }

        private void ViewImage()
        {
            MasterDetailPage masterPage = App.Current.MainPage as MasterDetailPage;
            masterPage.Detail.Navigation.PushModalAsync(new ImageViewPage(PhotoUrl));
        }

        private async void DeleteRecipe()
        {
            bool response = await App.Current.MainPage.DisplayAlert("Obaveštenje", "Da li želite da uklonite recept?", "Da", "Ne");
            if (response)
            {
                Device.BeginInvokeOnMainThread(() => Dialogs.Show());
                bool result = await RecipeDataService.DeleteRecipe(mRecipeId, LocalDataSecureStorage.GetToken());
                Device.BeginInvokeOnMainThread(() => Dialogs.Hide());
                string message = result ? "Recept je uspešno uklonjen." : "Greška prilikom uklanjanja recept.";
                await App.Current.MainPage.DisplayAlert("Obaveštenje", message, "U redu");

                if (result)
                {
                    MasterDetailPage masterPage = App.Current.MainPage as MasterDetailPage;
                    MessagingCenter.Send(this, "RECIPE_DELETED", mRecipeId);
                    await masterPage.Detail.Navigation.PopAsync();
                }


            }
        }

        private async void EditRecipe(RecipeDataModel recipe)
        {
            MasterDetailPage masterPage = App.Current.MainPage as MasterDetailPage;
            await masterPage.Detail.Navigation.PushAsync(new EditRecipePage(recipe));
        }

        private async void FavouriteRecipe(object sender, RecipeDataModel recipe)
        {
            var favouriteItem = sender as ToolbarItem;
            Device.BeginInvokeOnMainThread(() => Dialogs.Show());
            bool result = await RecipeDataService.FavouriteRecept(recipe.RecipeId, LocalDataSecureStorage.GetToken());
            Device.BeginInvokeOnMainThread(() => Dialogs.Hide());
            if (result)
            {
                recipe.IsFavourite = !recipe.IsFavourite;
                favouriteItem.Icon = recipe.IsFavourite ? "favorited" : "favorite";
                MessagingCenter.Send(this, "FAVOURITE", recipe);               
            }
        }
    }
}
