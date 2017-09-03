using Plugin.SecureStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foodbook.MobileApp.Tools
{
    public class LocalDataSecureStorage
    {

        #region Token
        public static void SaveToken(string token)
        {
            CrossSecureStorage.Current.SetValue(SecureStorageKeys.TOKEN, token);
        }

        public static string GetToken()
        {
            return CrossSecureStorage.Current.GetValue(SecureStorageKeys.TOKEN);
        }

        public static void DeleteToken()
        {
            CrossSecureStorage.Current.DeleteKey(SecureStorageKeys.TOKEN);
        }

        #endregion

        #region Email
        public static void SaveEmail(string email)
        {
            CrossSecureStorage.Current.SetValue(SecureStorageKeys.EMAIL, email);
        }

        public static string GetEmail()
        {
            return CrossSecureStorage.Current.GetValue(SecureStorageKeys.EMAIL);
        }

        public static void DeleteEmail()
        {
            CrossSecureStorage.Current.DeleteKey(SecureStorageKeys.EMAIL);
        }

        #endregion

        #region ID
        public static void SaveCookId(long id)
        {
            CrossSecureStorage.Current.SetValue(SecureStorageKeys.ID, id.ToString());
        }

        public static long? GetCookId()
        {
            string id = CrossSecureStorage.Current.GetValue(SecureStorageKeys.ID);

            try
            {
                return long.Parse(id);
            }
            catch
            {
                return null;
            }
        }

        public static void DeleteCookId()
        {
            CrossSecureStorage.Current.DeleteKey(SecureStorageKeys.ID);
        }

        #endregion

        #region CookName
        public static void SaveCookName(string name)
        {
            CrossSecureStorage.Current.SetValue(SecureStorageKeys.FULL_NAME, name);
        }

        public static string GetCookName()
        {
            return CrossSecureStorage.Current.GetValue(SecureStorageKeys.FULL_NAME);
        }

        public static void DeleteCookName()
        {
            CrossSecureStorage.Current.DeleteKey(SecureStorageKeys.FULL_NAME);
        }

        #endregion

        public static void ClearAllData()
        {
            DeleteToken();
            DeleteCookId();
            DeleteCookName();
            DeleteEmail();
        }

    }
}
