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
    public class CookDataService
    {
        public static async Task<ResponseCookModel> GetCookInfo(long id, string token)
        {
            try
            {
                string url = ApiUrls.COOK_RESOURCE_ID(id);

                ResponseCookModel model = new ResponseCookModel();

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + token);

                    HttpResponseMessage result = await client.GetAsync(url);
                    if (result.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string resultContent = await result.Content.ReadAsStringAsync();
                        try
                        {
                            model = (ResponseCookModel)Newtonsoft.Json.JsonConvert.DeserializeObject(resultContent.ToString(), typeof(ResponseCookModel));
                        }
                        catch (Exception ex)
                        {
                            model = new ResponseCookModel();
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

        public static async Task<List<ResponseCookModel>> GetCooks(string token = null)
        {
            try
            {
                string url = ApiUrls.COOK_RESOURCE;

                List<ResponseCookModel> model = new List<ResponseCookModel>();

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + token);


                    HttpResponseMessage result = await client.GetAsync(url);
                    if (result.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string resultContent = await result.Content.ReadAsStringAsync();
                        try
                        {
                            model = (List<ResponseCookModel>)Newtonsoft.Json.JsonConvert.DeserializeObject(resultContent.ToString(), typeof(List<ResponseCookModel>));
                        }
                        catch (Exception ex)
                        {
                            model = new List<ResponseCookModel>();
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

        public static async Task<bool> FavouriteCook(long cookId, string token)
        {

            string url = string.Format("{0}/{1}", ApiUrls.FAVOURITE_COOK_RESOURCE, cookId);

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

        public static async Task<bool> AddComment(PostCookCommentModel model, string token)
        {

            string url = ApiUrls.COOK_COMMENT_RESOURCE;

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

        public static async Task<bool> EditCook(PostRegisterModel model, long cookId, string token)
        {

            if (model.Photo != null)
            {
                model.PhotoUrl = await UploadFileToAzureBlob.BasicStorageBlockBlobOperationsAsync(model.Photo.PhotoStream, model.Photo.Name);
            }

            if (!string.IsNullOrEmpty(model.RemovedPhotoUrl))
            {
                List<string> photosForDeleting = new List<string>();
                photosForDeleting.Add(model.RemovedPhotoUrl);
                UploadFileToAzureBlob.DeleteBlob(photosForDeleting);
            }

            string url = ApiUrls.COOK_RESOURCE_ID(cookId);

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
            }
            return false;
        }

    }
}
