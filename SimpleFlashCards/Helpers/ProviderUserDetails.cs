using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SimpleFlashCards.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace SimpleFlashCards.Helpers
{
    public class ProviderUserDetails
    {

        private const string GoogleApiTokenInfoUrl = "https://www.googleapis.com/oauth2/v3/tokeninfo?id_token={0}";

        public GoogleUserModel GetUserDetails(string providerToken)
        {
            var httpClient = new HttpClient();
            var requestUri = new Uri(string.Format(GoogleApiTokenInfoUrl, providerToken));

            HttpResponseMessage httpResponseMessage;
            try
            {
                httpResponseMessage = httpClient.GetAsync(requestUri).Result;
            }
            catch (Exception ex)
            {
                return null;
            }

            if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }

            var response = httpResponseMessage.Content.ReadAsStringAsync().Result;
            var googleApiTokenInfo = JsonConvert.DeserializeObject<GoogleApiTokenInfo>(response);

            var googleUser = new GoogleUserModel
            {
                HasInfo = false,
                Email = googleApiTokenInfo.email,
                FirstName = googleApiTokenInfo.given_name,
                LastName = googleApiTokenInfo.family_name,
                Locale = googleApiTokenInfo.locale,
                Name = googleApiTokenInfo.name,
                ProviderUserId = googleApiTokenInfo.sub
            };

            if (checkForInfo(googleUser))
                googleUser.HasInfo = true;

            return googleUser;
        }

        private bool checkForInfo(GoogleUserModel googleUser)
        {
            bool result = true;
            
            if (googleUser.Email == null) 
                result = false;
            
            else if (googleUser.FirstName == null) 
                result = false;
            
            else if (googleUser.LastName == null) 
                result = false;
            
            else if (googleUser.Locale == null) 
                result = false;
            
            else if (googleUser.Name == null) 
                result = false;
            
            else if (googleUser.ProviderUserId == null) 
                result = false;

            return result;
        }
    }

    public class GoogleApiTokenInfo
    {
        public string iss { get; set; }
        public string at_hash { get; set; }
        public string aud { get; set; }
        public string sub { get; set; }
        public string email_verified { get; set; }
        public string azp { get; set; }
        public string email { get; set; }
        public string iat { get; set; }
        public string exp { get; set; }
        public string name { get; set; }
        public string picture { get; set; }
        public string given_name { get; set; }
        public string family_name { get; set; }
        public string locale { get; set; }
        public string alg { get; set; }
        public string kid { get; set; }
    }
}
