using SimpleRestApi.Models;
using System.Collections.Generic;
using System;

namespace SimpleRestApi
{
    public class AutorizationService
    {
        private string loggedInToken;
        private static readonly MicrosoftSqlDBService _microsoftSqlDB = MicrosoftSqlDBService.getInstance();
        private static AutorizationService instance = null;

        public AutorizationService()
        {
            loggedInToken = string.Empty;
        }

        public static AutorizationService getInstance()
        {
            if (instance == null)
            {
                instance = new AutorizationService();
            }
            return instance;
        }


        public UserInfo CheckUserToken(string user, string password)
        {
            string encodeString = user + ":" + password;
            var base64 = Base64Encode(encodeString);
            var userInfo = _microsoftSqlDB.getUserInfo(base64);
            if (userInfo != null)
            {
                loggedInToken = userInfo.Token;
            }
            else {
                loggedInToken = string.Empty;
            }
            return userInfo;
        }

        public bool checkIfVerify(string token,out string user)
        {
            user = string.Empty;
            var userInfo = _microsoftSqlDB.getUserInfo(token);

            if (loggedInToken.Equals(token) && userInfo != null)
                user = userInfo.FullName;
            return loggedInToken.Equals(token);
        }

        private static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }
        private static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

    }
}