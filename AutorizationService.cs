using SimpleRestApi.Models;
using System.Collections.Generic;
using System;

namespace SimpleRestApi
{
    public class AutorizationService
    {
        private string loggedInToken;
        private static AutorizationService instance = null;
        private Dictionary<string, UserInfo> _userHashMap;

        public AutorizationService()
        {
            loggedInToken = string.Empty;
            _userHashMap = new Dictionary<string, UserInfo> { };
            _userHashMap.Add("132", new UserInfo("Lior Turgeman", "123456", "132"));
            _userHashMap.Add("456", new UserInfo("Jacob Levi", "456789", "456"));
            _userHashMap.Add("bGlvcjo5ODc=", new UserInfo("lior", "987", "bGlvcjo5ODc="));
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
            string encodeString = user.Replace("{", "").Replace("}", "") + ":" + password.Replace("{", "").Replace("}", "");
            var base64 = Base64Encode(encodeString);
            if (_userHashMap.TryGetValue(base64, out var value) && value != null)
            {
                loggedInToken = value.Token;
            }
            else {
                loggedInToken = string.Empty;
            }
            return value;
        }

        public bool checkIfVerify(string token,out string user)
        {
            user = string.Empty;
            if (loggedInToken.Equals(token) && _userHashMap.TryGetValue(token, out var userInfo))
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