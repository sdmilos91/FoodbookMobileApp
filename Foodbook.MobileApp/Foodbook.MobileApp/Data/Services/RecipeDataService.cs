using Foodbook.MobileApp.Data.Models;
using Foodbook.MobileApp.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Foodbook.MobileApp.Data.Services
{
    public class RecipeDataService
    {
        public static async Task<ResponseRecipeModel> GetRecipes(RequestRecipeModel details)
        {
            try
            {
                string url = string.Format("{0}?text={1}&categoryId={2}&cuisineId={3}", ApiUrls.RECIPE_RESOURCE, details.Text, details.CategoryId, details.CuisineId);

                ResponseRecipeModel model = new ResponseRecipeModel();

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + details.Token);


                    HttpResponseMessage result = await client.GetAsync(url);
                    if (result.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string resultContent = await result.Content.ReadAsStringAsync();
                        try
                        {
                            model = (ResponseRecipeModel)Newtonsoft.Json.JsonConvert.DeserializeObject(resultContent.ToString(), typeof(ResponseRecipeModel));
                        }
                        catch (Exception ex)
                        {
                            model = new ResponseRecipeModel();
                        }
                    }
                }
                return model;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static async Task<RecipeCommonDataModel> GetRecipeCommonDate()
        {
            try
            {                              

                string url = ApiUrls.RECIPE_COMMON_DATA_RESOURCE;

                RecipeCommonDataModel model = new RecipeCommonDataModel();

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                    //client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + details.Token);


                    HttpResponseMessage result = await client.GetAsync(url);
                    if (result.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string resultContent = await result.Content.ReadAsStringAsync();
                        try
                        {
                            model = (RecipeCommonDataModel)Newtonsoft.Json.JsonConvert.DeserializeObject(resultContent.ToString(), typeof(RecipeCommonDataModel));
                        }
                        catch (Exception ex)
                        {
                            model = new RecipeCommonDataModel();
                        }
                    }
                }
                return model;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static async Task<bool> DeleteRecipe(long id, string token)
        {
            try
            {
                string url = string.Format("{0}/{1}", ApiUrls.RECIPE_RESOURCE, id);

                RecipeCommonDataModel model = new RecipeCommonDataModel();

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + token);


                    HttpResponseMessage result = await client.DeleteAsync(url);
                    if (result.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static async Task<bool> AddRecipe(PostRecipeModel model, string token)
        {
            foreach (var item in model.Photos.Where(x => x.IsAdded))
            {
                item.Url = await UploadFileToAzureBlob.BasicStorageBlockBlobOperationsAsync(item.PhotoStream, item.Name);
            }

            string url = ApiUrls.RECIPE_RESOURCE;

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + token);

                string content = Newtonsoft.Json.JsonConvert.SerializeObject(model);

                HttpResponseMessage result = await client.PostAsync(url, new StringContent(content, Encoding.UTF8, "application/json"));
                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return true;
                }                
            }
            return false;
        }

        public static async Task<bool> EditRecipe(PostRecipeModel model, long recipeId, string token)
        {
            foreach (var item in model.Photos.Where(x => x.IsAdded))
            {
                item.Url = await UploadFileToAzureBlob.BasicStorageBlockBlobOperationsAsync(item.PhotoStream, item.Name);
            }

            List<string> photosForDeleting = model.Photos.Where(x => x.IsDeleted).Select(x => x.Url).ToList();
            UploadFileToAzureBlob.DeleteBlob(photosForDeleting);
            model.Photos = new List<PhotoModel>(model.Photos.Where(x => !x.IsDeleted));
            string url = string.Format("{0}/{1}", ApiUrls.RECIPE_RESOURCE, recipeId);

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + token);

                string content = Newtonsoft.Json.JsonConvert.SerializeObject(model);

                HttpResponseMessage result = await client.PutAsync(url, new StringContent(content, Encoding.UTF8, "application/json"));
                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return true;
                }
                else
                {
                    string resultContent = await result.Content.ReadAsStringAsync();
                }
            }
            return false;
        }

        public static async Task<bool> AddComment(PostRecipeCommentModel model, string token)
        {

            string url = ApiUrls.RECIPE_COMMENT_RESOURCE;

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + token);

                string content = Newtonsoft.Json.JsonConvert.SerializeObject(model);

                HttpResponseMessage result = await client.PostAsync(url, new StringContent(content, Encoding.UTF8, "application/json"));
                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return true;
                }
            }
            return false;
        }

        public static async Task<bool> FavouriteRecept(long recipeId, string token)
        {

            string url = string.Format("{0}/{1}", ApiUrls.FAVOURITE_RECIPE_RESOURCE, recipeId);

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + token);

                string content = string.Empty;

                HttpResponseMessage result = await client.PutAsync(url, null);
                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
