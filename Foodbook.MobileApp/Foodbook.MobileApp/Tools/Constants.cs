using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foodbook.MobileApp.Tools
{
    public class ApiUrls
    {
        //public static string BASE_URL = "http://192.168.1.2:50226";
        public static string BASE_URL = "http://srv.dunavnet.eu/FoodBookApi";

        //Recipe
        public static string RECIPE_RESOURCE_ID(long id)
        {
            return string.Format("{0}/api/Recipe/{1}", BASE_URL, id);
        }

        public static string RECIPE_RESOURCE = string.Format("{0}/api/Recipe", BASE_URL);

        public static string RECIPE_COMMON_DATA_RESOURCE = string.Format("{0}/api/RecipeCommonData", BASE_URL);

        public static string RECIPE_COMMENT_RESOURCE = string.Format("{0}/api/RecipeComment", BASE_URL);

        public static string FAVOURITE_RECIPE_RESOURCE = string.Format("{0}/api/FavouriteRecipe", BASE_URL);

        //Cook
        public static string COOK_RESOURCE_ID(long? id)
        {
            return string.Format("{0}/api/Cook/{1}", BASE_URL, id);
        }

        public static string COOK_RESOURCE = string.Format("{0}/api/Cook", BASE_URL);

        public static string FAVOURITE_COOK_RESOURCE = string.Format("{0}/api/FavouriteCook", BASE_URL);

        public static string COOK_COMMENT_RESOURCE = string.Format("{0}/api/CookComment", BASE_URL);

        public static string COOK_INFO = string.Format("{0}/api/Cook/GetCookInfo", BASE_URL);

        
        //Account
        public static string GET_USER_TOKEN = string.Format("{0}/Token", BASE_URL);
        
        public static string REGISTER_USER = string.Format("{0}/api/Account/Register", BASE_URL);

        public static string IS_USER_AUTHENTICATED = string.Format("{0}/api/Account/IsAuthenticated", BASE_URL);

        public static string USER_INFO = string.Format("{0}/api/Account/UserInfo", BASE_URL);

    }

    public class SecureStorageKeys
    {
        public static string TOKEN = "TOKEN";
        public static string EMAIL = "EMAIL";
        public static string ID = "ID";
        public static string FULL_NAME = "FULL_NAME";
        public static string COOK_PHOTO = "COOK_PHOTO";
    }

    public class MessageCenterKeys
    {
        public static string ADDED = "ADDED";
        public static string EDITED = "EDITED";
        public static string DELETED = "DELETED";
        public static string LOGGED_IN = "LOGGED_IN";
    }

    public class MyColors
    {
        public static string PINK = "";
        public static string DARK_RED = "#b61827";
        public static string MIDDLE_RED = "#ef5350";
        public static string GREEN = "#4BB04F";
        public static string LIGHT_GREEN = "#effcea";
        public static string YELLOW = "#FFD54F";
    }

    public class AzureStorageSettings
    {
        public static string ACCOUNT_NAME = "storage4milos";
        public static string KEY_VALUE = "oYK0NFl1hflJjB8obVo61ZYxbaSrHVGa+9M3ar2pWG5CJR5DzNN1YxoJY/O+7ZMZ7J3Uh2xsR5bvuoCuxzuWeQ==";
    }
}
