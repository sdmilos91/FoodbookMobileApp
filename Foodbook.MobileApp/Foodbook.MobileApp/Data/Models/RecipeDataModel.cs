using System;
using System.Collections.Generic;
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
        public Comment[] Comments { get; set; }
        public double? Rating { get; set; }
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

    public class PostRecipeModel
    {
        public string Name { get; set; }
        public long CuisineId { get; set; }
        public long CategoryId { get; set; }
        public string RecipeText { get; set; }
        public string VideoUrl { get; set; }
        public long? CaloricityId { get; set; }
        public int PreparationTime { get; set; }
    }




}
