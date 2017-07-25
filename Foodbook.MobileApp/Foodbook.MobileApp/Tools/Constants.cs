using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foodbook.MobileApp.Tools
{
    public class ApiUrls
    {
        public static string BASE_URL = "http://192.168.1.2:50226";

        //Recipe
        public static string RECIPE_RESOURCE_ID(long id)
        {
            return string.Format("{0}/api/Recipe/{1}", BASE_URL, id);
        }

        public static string RECIPE_RESOURCE = string.Format("{0}/api/Recipe", BASE_URL);

        public static string RECIPE_COMMON_DATA_RESOURCE = string.Format("{0}/api/RecipeCommonData", BASE_URL);

        //Cook
        public static string COOK_RESOURCE_ID(long id)
        {
            return string.Format("{0}/api/Cook/{1}", BASE_URL, id);
        }

        public static string COOK_RESOURCE = string.Format("{0}/api/Cook", BASE_URL);



    }
}
