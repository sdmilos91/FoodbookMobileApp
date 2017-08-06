using Foodbook.MobileApp.Data.Models;
using Foodbook.MobileApp.Data.Services;
using Foodbook.MobileApp.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Foodbook.MobileApp.ViewModels
{
    public class AddCommentViewModel : BaseViewModel
    {
        private long mRecipeId;

        private int rating;

        public int Rating
        {
            get { return rating; }
            set { SetProperty(ref rating, value); }
        }

        private string commentText;

        public string CommentText
        {
            get { return commentText; }
            set { SetProperty(ref commentText, value); }
        }


        private bool isRatingInvalid;

        public bool IsRatingInvalid
        {
            get { return isRatingInvalid; }
            set { SetProperty(ref isRatingInvalid, value); }
        }


        public Command SetRatingCommand { get;}
        public Command AddCommentCommand { get; }

        public AddCommentViewModel(long recipeId)
        {
            mRecipeId = recipeId;
            SetRatingCommand = new Command((x) => SetRating(x));
            AddCommentCommand = new Command(() => AddComment());
            IsRatingInvalid = false;
        }

        private void SetRating(object rating)
        {
            Rating = int.Parse(rating.ToString());
            IsRatingInvalid = false;

        }

        private async void AddComment()
        {
            bool isInvalidForm = false;

            if (rating == 0)
            {
                IsRatingInvalid = true;
                isInvalidForm = true;
            }

            if (string.IsNullOrEmpty(CommentText))
            {
                CommentText = "";
                isInvalidForm = true;
            }

            if (!isInvalidForm)
            {
                PostRecipeCommentModel model = new PostRecipeCommentModel
                {
                    CommentText = CommentText,
                    Rating = Rating,
                    InsertDate = DateTime.Now,
                    RecipeId = mRecipeId
                };
                Device.BeginInvokeOnMainThread(() => Dialogs.Show());
                bool res = await RecipeDataService.AddComment(model, LocalDataSecureStorage.GetToken());
                if (res)
                {
                    MessagingCenter.Send(this, "COMMENT_ADDED", model);
                }
                Device.BeginInvokeOnMainThread(() => Dialogs.Hide());
                
            }
        }
    }
}
