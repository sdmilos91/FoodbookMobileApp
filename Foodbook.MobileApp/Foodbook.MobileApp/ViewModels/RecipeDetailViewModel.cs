using Foodbook.MobileApp.Data.Models;
using Foodbook.MobileApp.Pages;
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

        public Command AddCommentCommand { get; }
        public Command CommentAddedCommant { get; }

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


    }
}
