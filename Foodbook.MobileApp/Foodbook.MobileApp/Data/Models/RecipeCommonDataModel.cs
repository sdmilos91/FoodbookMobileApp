using Foodbook.MobileApp.Tools;
using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foodbook.MobileApp.Data.Models
{
    public class RecipeCommonDataModel
    {
        public List<FoodCategoryModel> Categories { get; set; }
        public List<CuisineModel> Cuisines { get; set; }
        public List<CaloricityModel> Caloricities { get; set; }

    }

    public class FoodCategoryModel : RealmObject
    {

        public long CategoryId { get; set; }
        public string CategoryName { get; set; }

    }

    public class CuisineModel : RealmObject
    {

        public long CuisineId { get; set; }
        public string CuisineName { get; set; }

    }

    public class CaloricityModel : RealmObject
    {

        public long CaloricityId { get; set; }
        public int? MinValue { get; set; }
        public int? MaxValue { get; set; }
        public string Name { get; set; }
    }

    public class RecipeCommonDataFilterModel
    {
        public long? SelectedCategory { get; set; }
        public long? SelectedCuisine { get; set; }
        public long? SelectedCaloricity { get; set; }
        public string RecipeName { get; set; }
    }

    public class CommonDataSortModel
    {
        public int OrderById { get; set; }
        public int OrderId { get; set; }

        public List<Item> OrderByItems { get; set; }
        public List<Item> OrderItems { get; set; }

        public CommonDataSortModel()
        {
            OrderById = RecipeSort.NAME;
            OrderId = RecipeSort.ORDER_ASC;
        }
    }

    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
