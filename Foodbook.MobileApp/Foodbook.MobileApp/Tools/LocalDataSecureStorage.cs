﻿using Plugin.SecureStorage;
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

    }
}