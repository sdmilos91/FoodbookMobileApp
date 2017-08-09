﻿using Foodbook.MobileApp.Data.Models;
using Foodbook.MobileApp.Pages;
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
            PhotoUrl = recipe.PhotoUrl;

            AddCommentCommand = new Command(() => AddComment());
            CommentAddedCommant = new Command((x) => CommentAdded(x));
            ViewImageCommand = new Command(() => ViewImage());
            ImageContainerHeight = new GridLength(1, GridUnitType.Star);
            SwitchTabCommand = new Command((x) => SwitchTab(x.ToString()));
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

            commentModel.CookName = CookName;

            Comment comment = new Comment
            {
                CommentText = commentModel.CommentText,
                InsertDate = commentModel.InsertDate,
                Rating = commentModel.Rating,
                CookName = CookName,
            };

            var temp = Comments;

            temp.Add(comment);
            Comments = temp;

            rating = Comments.Average(x => x.Rating);
            

            MessagingCenter.Send(this, "RATING_UPDATED", commentModel);

        }

        private void SwitchTab(string tab)
        {
            FirstTabColor = Color.FromHex("#EF5350");
            FirstTabIndicatorColor = Color.FromHex("#EF5350");

            SecondTabColor = Color.FromHex("#EF5350");
            SecondTabIndicatorColor = Color.FromHex("#EF5350");

            ThirdTabColor = Color.FromHex("#EF5350");
            ThirdTabIndicatorColor = Color.FromHex("#EF5350");

            FirstContainer = false;
            SecondContainer = false;
            ThirdContainer = false;


            switch (tab)
            {
                case "1":

                    FirstTabIndicatorColor = Color.FromHex("#FFD54F");
                    FirstContainer = true;
                    ImageContainerHeight = new GridLength(1, GridUnitType.Star);
                    break;

                case "2":
                    SecondTabIndicatorColor = Color.FromHex("#FFD54F");
                    SecondContainer = true;
                    ImageContainerHeight = new GridLength(0, GridUnitType.Star);
                    break;

                case "3":
                    ThirdTabIndicatorColor = Color.FromHex("#FFD54F");
                    ThirdContainer = true;
                    ImageContainerHeight = new GridLength(0, GridUnitType.Star);
                    break;
                default:
                    FirstTabIndicatorColor = Color.FromHex("#FFD54F");
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
    }
}
