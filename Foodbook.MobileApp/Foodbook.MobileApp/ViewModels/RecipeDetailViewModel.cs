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
        private long mCookId;
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
        public List<Ingredient> Ingredients { get; set; }
        public List<PhotoModel> Photos { get; set; }
        public int IngradientsContainer { get; set; }

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

        private int currentPhototIndex;

        public int CurrentPhototIndex
        {
            get { return currentPhototIndex; }
            set { currentPhototIndex = value; OnPropertyChanged(); }
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
        public Command ViewCookProfileCommand { get; }

        public RecipeDetailViewModel(RecipeDataModel recipe)
        {
            mRecipeId = recipe.RecipeId;
            mCookId = recipe.CookId;
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
            Ingredients = recipe.Ingredients;
            IngradientsContainer = Ingredients.Count() * 40;
            Photos = recipe.Photos;
            if (!Photos.Any())
            {
                Photos.Add(new PhotoModel
                {
                    Url = recipe.ProfilePhotoUrl
                });
            }

            AddCommentCommand = new Command(() => AddComment());
            ViewImageCommand = new Command(() => ViewImage());
            ImageContainerHeight = new GridLength(1, GridUnitType.Star);
            SwitchTabCommand = new Command((x) => SwitchTab(x.ToString()));
            DeleteRecipeCommand = new Command(() => DeleteRecipe());
            EditRecipeCommand = new Command(() => EditRecipe(recipe));
            FavouriteRecipeCommand = new Command((x) => FavouriteRecipe(x, recipe));
            ViewCookProfileCommand = new Command((x) => ViewCookProfile(x));
            SwitchTab("1");

          
        }

        private void AddComment()
        {
            MessagingCenter.Subscribe<AddCommentViewModel>(this, "COMMENT_ADDED", (sender) =>
            {
                CommentAdded();
                MessagingCenter.Unsubscribe<AddCommentViewModel>(this, "COMMENT_ADDED");
            });

            MasterDetailPage master = App.Current.MainPage as MasterDetailPage;

            master.Detail.Navigation.PushPopupAsync(new AddCommentPopupPage(mRecipeId));

        }

        private void CommentAdded()
        {
            var recipe = DataMockup.GetRecipeById(mRecipeId);
            if (recipe != null)
            {
                Comments = new ObservableCollection<Comment>(recipe.Comments);
                OnPropertyChanged("Comments");

                Rating = Comments.Average(x => x.Rating);
                OnPropertyChanged("Rating");

                MessagingCenter.Send(this, "RATING_UPDATED");
            }            
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
                    //SecondTabTextColor = indicatorColor;
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

        private void ViewImage()
        {
            MasterDetailPage masterPage = App.Current.MainPage as MasterDetailPage;
            masterPage.Detail.Navigation.PushModalAsync(new ImageViewPage(Photos.ElementAt(currentPhototIndex).Url));
        }

        private async void DeleteRecipe()
        {
            bool response = await App.Current.MainPage.DisplayAlert("Obaveštenje", "Da li želite da uklonite recept?", "Da", "Ne");
            if (response)
            {
                ShowDialog();
                bool result = await RecipeDataService.DeleteRecipe(mRecipeId, LocalDataSecureStorage.GetToken());
                HideDialog();
                string message = result ? "Recept je uspešno uklonjen." : "Greška prilikom uklanjanja recept.";
                await App.Current.MainPage.DisplayAlert("Obaveštenje", message, "U redu");

                if (result)
                {
                    DataMockup.RemoveRecipe(mRecipeId);
                    MasterDetailPage masterPage = App.Current.MainPage as MasterDetailPage;
                    MessagingCenter.Send(this, "RECIPE_DELETED");
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
            ShowDialog();
            bool result = await RecipeDataService.FavouriteRecept(recipe.RecipeId, LocalDataSecureStorage.GetToken());
            HideDialog();
            if (result)
            {
                DataMockup.AddOrRemoveFavoutiteRecipe(recipe);
                //recipe.IsFavourite = !recipe.IsFavourite;
                favouriteItem.Icon = recipe.IsFavourite ? "favorited" : "favorite";
                MessagingCenter.Send(this, "FAVOURITE");               
            }
        }

        private async void ViewCookProfile(object sender)
        {
            Helpers.Utils.ButtonPress(sender);
            ShowDialog();
            List<ResponseCookModel> cooks = await CookDataService.GetCooks();
            HideDialog();
            ResponseCookModel cook = cooks.FirstOrDefault(x => x.CookId == mCookId);
            if (cook != null)
            {
                MasterDetailPage masterPage = App.Current.MainPage as MasterDetailPage;
                await masterPage.Detail.Navigation.PushAsync(new UserDetailsPage(cook));
            }

        }
    }
}
