using Foodbook.MobileApp.Data.Models;
using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foodbook.MobileApp.RealServices
{
    public class RecipeRealmService
    {
        public static void SaveRecipes(List<RecipeDataModel> recipes)
        {
            var realm = Realm.GetInstance();

            realm.Write(() =>
            {
                foreach (var recipe in recipes)
                {
                }
            });
        }
    }
}
