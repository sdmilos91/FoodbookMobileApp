using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foodbook.MobileApp.Data.Models
{
    public class CookModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Biography { get; set; }

        public string PhotoUrl { get; set; }

        public long CookId { get; set; }

    }

    public class ResponseCookModel : CookModel
    {
        public ResponseCookModel()
        {
            FollowedCooks = new List<CookModel>();
            Recipes = new List<RecipeDataModel>();
            FavouriteRecipes = new List<RecipeDataModel>();
            Comments = new List<CookCommentModel>();
        }

        public List<CookModel> FollowedCooks { get; set; }
        public List<RecipeDataModel> Recipes { get; set; }
        public List<RecipeDataModel> FavouriteRecipes { get; set; }
        public string FullName { get; set; }
        public bool IsFollowed { get; set; }
        public int NumberOfRecipes { get; set; }
        public int NumberOfFollowers { get; set; }
        public List<CookCommentModel> Comments { get; set; }
        public double? Rating { get; set; }


    }

    public class CookCommentModel
    {
        public long CommentId { get; set; }
        public int Rating { get; set; }
        public string CommentText { get; set; }
        public long CookId { get; set; }
        public string CookName { get; set; }
        public DateTime InsertDate { get; set; }

    }

    public class PostCookCommentModel
    {
        public int Rating { get; set; }

        public string CommentText { get; set; }

        public long CookId { get; set; }

        public DateTime InsertDate { get; set; }

        [JsonIgnore]
        public string CookName { get; set; }

    }
}
