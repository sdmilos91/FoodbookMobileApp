using Foodbook.MobileApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foodbook.MobileApp.ViewModels
{
    class RecipeDetailViewModel : BaseViewModel
    {
        public string CookName { get; set; }
        public string Name { get; set; }        
        public string CuisineName { get; set; }        
        public string CategoryName { get; set; }
        public string RecipeText { get; set; }
        public string VideoUrl { get; set; }        
        public string CaloricityName { get; set; }
        public DateTime InsertDate { get; set; }
        public ObservableCollection<Comment> Comments { get; set; }
        public double? Rating { get; set; }

        public RecipeDetailViewModel(RecipeDataModel recipe)
        {
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

        }
    }
}
