using Foodbook.MobileApp.Data.Models;
using Foodbook.MobileApp.Data.Services;
using Foodbook.MobileApp.Tools;
using Rg.Plugins.Popup.Extensions;
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
        private long mId;

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
        public Command CancelCommand { get; set; }

        public AddCommentViewModel(long id, bool cookComment)
        {
            mId = id;
            SetRatingCommand = new Command((x) => SetRating(x));
            AddCommentCommand = new Command(() => AddComment(cookComment));
            CancelCommand = new Command(() => Cancel());
            IsRatingInvalid = false;
        }

        private void SetRating(object rating)
        {
            Rating = int.Parse(rating.ToString());
            IsRatingInvalid = false;

        }

        private async void AddComment(bool cookComment)
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
                ShowDialog();

                if (!cookComment)
                {
                    PostRecipeCommentModel commentModel = new PostRecipeCommentModel
                    {
                        CommentText = CommentText,
                        Rating = Rating,
                        InsertDate = DateTime.Now,
                        RecipeId = mId
                    };
                    bool res = await RecipeDataService.AddComment(commentModel, LocalDataSecureStorage.GetToken());
                    if (res)
                    {
                        Comment comment = new Comment
                        {
                            CommentText = commentModel.CommentText,
                            InsertDate = commentModel.InsertDate,
                            Rating = commentModel.Rating,
                            CookName = LocalDataSecureStorage.GetCookName(),
                            CookPhotoUrl = LocalDataSecureStorage.GetCookPhoto()
                        };
                        
                        DataMockup.AddCommentRecipe(comment, commentModel.RecipeId);

                        try
                        {
                            MessagingCenter.Send(this, "COMMENT_ADDED");
                            MasterDetailPage masterPage = App.Current.MainPage as MasterDetailPage;
                            await masterPage.Detail.Navigation.PopPopupAsync();
                        }
                        catch (Exception ex)
                        {
                            int dasdas = 5;
                        }
                       
                    }
                }
                else
                {
                    PostCookCommentModel commentModel = new PostCookCommentModel
                    {
                        CommentText = CommentText,
                        Rating = Rating,
                        InsertDate = DateTime.Now,
                        CookId = mId
                    };

                    bool res = await CookDataService.AddComment(commentModel, LocalDataSecureStorage.GetToken());

                    if (res)
                    {
                        CookCommentModel comment = new CookCommentModel
                        {
                            CommentText = commentModel.CommentText,
                            InsertDate = commentModel.InsertDate,
                            Rating = commentModel.Rating,
                            CookName = LocalDataSecureStorage.GetCookName(),
                            CookPhotoUrl = LocalDataSecureStorage.GetCookPhoto()
                        };

                        DataMockup.AddCommentCook(comment, commentModel.CookId);

                        MessagingCenter.Send(this, "COOK_COMMENT_ADDED");
                    }
                }
                HideDialog();
                
            }
        }

        private async void Cancel()
        {
            MasterDetailPage masterPage = App.Current.MainPage as MasterDetailPage;
            await masterPage.Detail.Navigation.PopPopupAsync();
        }
    }
}
