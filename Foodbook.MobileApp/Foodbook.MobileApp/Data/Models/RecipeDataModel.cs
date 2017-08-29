using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foodbook.MobileApp.Data.Models
{

    public class RecipeDataModel
    {
        public long RecipeId { get; set; }
        public long CookId { get; set; }
        public string CookName { get; set; }
        public string Name { get; set; }
        public long CuisineId { get; set; }
        public string CuisineName { get; set; }
        public long CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string RecipeText { get; set; }
        public string VideoUrl { get; set; }
        public long CaloricityId { get; set; }
        public string CaloricityName { get; set; }
        public DateTime InsertDate { get; set; }
        public List<Comment> Comments { get; set; }
        public double? Rating { get; set; }
        public int PreparationTime { get; set; }
        public List<PhotoModel> Photos { get; set; }
        public string ProfilePhotoUrl { get; set; }
        public bool IsMine { get; set; }
        public bool IsFavourite { get; set; }
    }

    public class Comment
    {
        public int CommentId { get; set; }
        public int Rating { get; set; }
        public string CommentText { get; set; }
        public int CookId { get; set; }
        public string CookName { get; set; }
        public DateTime InsertDate { get; set; }
    }

    public class RequestRecipeModel
    {
        public RequestRecipeModel()
        {
            StartIndex = 0;
            Text = string.Empty;
            CategoryId = null;
            CuisineId = null;
            JustsAllRecipes = true;
            
        }

        [JsonIgnore]
        public string Token { get; set; }

        public int StartIndex { get; set; }
        public string Text { get; set; }
        public long? CategoryId { get; set; }
        public long? CuisineId { get; set; }
        public bool JustsAllRecipes { get; set; }
    }

    public class ResponseRecipeModel
    {
        public ResponseRecipeModel()
        {
            MyRecipes = new List<RecipeDataModel>();
            FavouriteRecipes = new List<RecipeDataModel>();
            AllRecipes = new List<RecipeDataModel>();
        }

        public List<RecipeDataModel> MyRecipes { get; set; }
        public List<RecipeDataModel> FavouriteRecipes { get; set; }
        public List<RecipeDataModel> AllRecipes { get; set; }
    }

    public class PostRecipeModel
    {
        public PostRecipeModel()
        {
            Photos = new List<PhotoModel>();
        }
        public string Name { get; set; }

        public long CuisineId { get; set; }
        public long CategoryId { get; set; }
        public string RecipeText { get; set; }
        public string VideoUrl { get; set; }
        public long? CaloricityId { get; set; }
        public int PreparationTime { get; set; }


        public List<PhotoModel> Photos { get; set; }
    }

    public class PhotoModel
    {
        public string Url { get; set; }

        public string Name { get; set; }

        [JsonIgnore]
        public bool IsAdded { get; set; }

        [JsonIgnore]
        public bool IsDeleted { get; set; }

        [JsonIgnore]
        public Stream PhotoStream { get; set; }
    }

    public class PostRecipeCommentModel
    {

        public int Rating { get; set; }

        public string CommentText { get; set; }

        public long RecipeId { get; set; }

        [JsonIgnore]
        public string CookName { get; set; }

        public DateTime InsertDate { get; set; }

    }




}
