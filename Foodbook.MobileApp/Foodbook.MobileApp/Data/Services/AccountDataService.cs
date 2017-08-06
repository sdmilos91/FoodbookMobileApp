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
    public class AccountDataService
    {
        public static async Task<LoginResponseModel> LoginUser(string username, string password)
        { 
            string url = ApiUrls.GET_USER_TOKEN;

            LoginResponseModel model = new LoginResponseModel();
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

                    var formData = new Dictionary<string, string>
                    {
                        { "grant_type", "password" },
                        { "username", username },
                        { "password", password }
                    }; 

                    FormUrlEncodedContent formContent = new FormUrlEncodedContent(formData);

                    HttpResponseMessage result = await client.PostAsync(url, formContent);
                    if (result.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string resultContent = await result.Content.ReadAsStringAsync();
                        model = (LoginResponseModel)Newtonsoft.Json.JsonConvert.DeserializeObject(resultContent.ToString(), typeof(LoginResponseModel));
                        model.IsSuccess = true;
                    }
                    else if(result.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        string resultContent = await result.Content.ReadAsStringAsync();
                        model.ErrorType = ERROR_TYPES.BAD_REQUEST;
                        model.ErrorMessage = resultContent;
                    }
                    else if (result.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                    {
                        string resultContent = await result.Content.ReadAsStringAsync();
                        model.ErrorType = ERROR_TYPES.INTERNAL_SERVER_ERROR;
                        model.ErrorMessage = resultContent;
                    }

                }
            }
            catch (Exception ex)
            {
                model.ErrorType = ERROR_TYPES.INTERNAL_SERVER_ERROR;
                model.ErrorMessage = string.Format("{0} - {1}", ex.Source, ex.Message);
            }
            return model;
        }
    }
}
