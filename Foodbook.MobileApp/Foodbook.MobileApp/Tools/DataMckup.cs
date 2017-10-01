using Foodbook.MobileApp.Data.Models;
using Foodbook.MobileApp.Data.Services;
using Realms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foodbook.MobileApp.Tools
{
    public class DataMockup
    {
        private static ResponseRecipeModel RECIPES;
        private static ObservableCollection<ResponseCookModel> COOKS;

        #region RECIPE

        public static void SaveRecipes(ResponseRecipeModel recipes)
        {
            RECIPES = recipes;
        }


        public static ResponseRecipeModel GetRecipes()
        {
            return RECIPES != null ? RECIPES : new ResponseRecipeModel();
        }

        public static RecipeDataModel GetRecipeById(long recipeId)
        {
            return RECIPES != null ? RECIPES.AllRecipes.FirstOrDefault(x => x.RecipeId == recipeId) : new RecipeDataModel();
        }

        public static List<RecipeDataModel> GetRecipesByType(int recipeType)
        {
            switch (recipeType)
            {
                case RecipesPageTabs.MY_RECIPES:
                    return RECIPES != null ? RECIPES.MyRecipes : new List<RecipeDataModel>();
                case RecipesPageTabs.FAVORITE_RECIPES:
                    return RECIPES != null ? RECIPES.FavouriteRecipes : new List<RecipeDataModel>();
                case RecipesPageTabs.ALL_RECIPES:
                    return RECIPES != null ? RECIPES.AllRecipes : new List<RecipeDataModel>();
                default:
                    return RECIPES != null ? RECIPES.MyRecipes : new List<RecipeDataModel>();

            }
        }

        public static void RemoveRecipe(long recipeId)
        {

            if (RECIPES.AllRecipes.Any(x => x.RecipeId == recipeId))
            {
                var recipe = RECIPES.AllRecipes.FirstOrDefault(x => x.RecipeId == recipeId);
                RECIPES.AllRecipes.Remove(recipe);
            }

            if (RECIPES.MyRecipes.Any(x => x.RecipeId == recipeId))
            {
                var recipe = RECIPES.MyRecipes.FirstOrDefault(x => x.RecipeId == recipeId);                
                RECIPES.MyRecipes.Remove(recipe);
            }
        }

        public static void AddOrRemoveFavoutiteRecipe(RecipeDataModel recipe)
        {
            if (RECIPES.FavouriteRecipes.Any(x => x.RecipeId == recipe.RecipeId))
            {
                RECIPES.FavouriteRecipes.Remove(recipe);
                recipe.IsFavourite = false;
                RECIPES.AllRecipes.FirstOrDefault(x => x.RecipeId == recipe.RecipeId).IsFavourite = false;
            }
            else
            {
                RECIPES.FavouriteRecipes.Add(recipe);
                recipe.IsFavourite = true;
                RECIPES.AllRecipes.FirstOrDefault(x => x.RecipeId == recipe.RecipeId).IsFavourite = true;
            }


        }

        public static void AddCommentRecipe(Comment newComment, long recipeId)
        {

            if (RECIPES.AllRecipes.Any(x => x.RecipeId == recipeId))
            {
                var recipe = RECIPES.AllRecipes.FirstOrDefault(x => x.RecipeId == recipeId);
                recipe.Comments.Add(newComment);
                recipe.Rating = recipe.Comments.Average(x => x.Rating);
            }

            if (RECIPES.FavouriteRecipes.Any(x => x.RecipeId == recipeId))
            {
                var recipe = RECIPES.FavouriteRecipes.FirstOrDefault(x => x.RecipeId == recipeId);
                recipe.Comments.Add(newComment);
                recipe.Rating = recipe.Comments.Average(x => x.Rating);
            }
        }

        public static async Task<RecipeCommonDataModel> GetRecipeCommonData()
        {
            RecipeCommonDataModel mCommonData = new RecipeCommonDataModel();

            var realm = Realm.GetInstance();
            var isCategoryFull = realm.All<FoodCategoryModel>().Any();
            var isCuisineFull = realm.All<CuisineModel>().Any();
            var isCaloricityFull = realm.All<CaloricityModel>().Any();

            if (isCategoryFull && isCuisineFull && isCaloricityFull)
            {
                var categories = new ObservableCollection<FoodCategoryModel>(realm.All<FoodCategoryModel>());
                var cuisines = new ObservableCollection<CuisineModel>(realm.All<CuisineModel>());
                var caloricities = new ObservableCollection<CaloricityModel>(realm.All<CaloricityModel>());

                mCommonData = new RecipeCommonDataModel
                {
                    Caloricities = caloricities.ToList(),
                    Categories = categories.ToList(),
                    Cuisines = cuisines.ToList()
                };
            }
            else
            {
                RecipeCommonDataModel commonData = await RecipeDataService.GetRecipeCommonDate();

                realm.Write(() =>
                {

                    foreach (var item in commonData.Categories)
                    {
                        realm.Add(item);
                    }

                    foreach (var item in commonData.Cuisines)
                    {
                        realm.Add(item);
                    }

                    foreach (var item in commonData.Caloricities)
                    {
                        realm.Add(item);
                    }
                });

                var categories = new ObservableCollection<FoodCategoryModel>(commonData.Categories);
                var cuisines = new ObservableCollection<CuisineModel>(commonData.Cuisines);
                var caloricities = new ObservableCollection<CaloricityModel>(commonData.Caloricities);

                mCommonData = commonData;
            }

            return mCommonData;
        }

        #endregion

        #region COOKS

        public static void SaveCooks(ObservableCollection<ResponseCookModel> cooks)
        {
            COOKS = cooks;
        }


        public static ObservableCollection<ResponseCookModel> GeCooks()
        {
            return COOKS != null ? COOKS : new ObservableCollection<ResponseCookModel>();
        }

        public static ResponseCookModel GetCookById(long cookId)
        {
            return COOKS != null ? COOKS.FirstOrDefault(x => x.CookId == cookId) : new ResponseCookModel();
        }

        public static List<ResponseCookModel> GetCooksByType(int cookType)
        {
            switch (cookType)
            {
                case CooksPageTabs.ALL_COOKS:
                    return COOKS != null ? COOKS.ToList() : new List<ResponseCookModel>();
                case CooksPageTabs.FAVORITE_COOKS:
                    return COOKS != null ? COOKS.Where(x => x.IsFollowed).ToList() : new List<ResponseCookModel>();
                default:
                    return COOKS != null ? COOKS.ToList() : new List<ResponseCookModel>();

            }
        }

        public static void RemoveCook(long cookId)
        {

            if (COOKS.Any(x => x.CookId == cookId))
            {
                var cook = COOKS.FirstOrDefault(x => x.CookId == cookId);
                COOKS.Remove(cook);
            }

        }

        public static void AddOrRemoveFavoutiteCook(ResponseCookModel cook)
        {
            if (COOKS.Any(x => x.CookId == cook.CookId))
            {
                COOKS.FirstOrDefault(x => x.CookId == cook.CookId).IsFollowed = !COOKS.FirstOrDefault(x => x.CookId == cook.CookId).IsFollowed;
            }
            
        }

        public static void AddCommentCook(CookCommentModel newComment, long cookId)
        {

            if (COOKS.Any(x => x.CookId == cookId))
            {
                var cook = COOKS.FirstOrDefault(x => x.CookId == cookId);
                cook.Comments.Add(newComment);
                cook.Rating = cook.Comments.Average(x => x.Rating);
            }

          
        }

        #endregion
    }
}
