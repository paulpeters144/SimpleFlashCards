using Newtonsoft.Json;
using SimpleFlashCards.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

namespace SimpleFlashCards.Helpers
{
    public class GoogleUserDetails
    {
        private readonly string GoogleApiTokenInfoUrl = "https://www.googleapis.com/oauth2/v3/tokeninfo?id_token={0}";
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Locale { get; set; }
        public string Name { get; set; }
        public string ProviderUserId { get; set; }
        public GoogleUserDetails GetUserDetails(string providerToken)
        {
            var httpClient = new HttpClient();
            var requestUri = new Uri(string.Format(GoogleApiTokenInfoUrl, providerToken));
            HttpResponseMessage httpResponseMessage;

            try
            {
                httpResponseMessage = httpClient.GetAsync(requestUri).Result;
            }
            catch (Exception)
            {
                return null;
            }

            if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
                return null;

            var response = httpResponseMessage.Content.ReadAsStringAsync().Result;
            var userInfo = JsonConvert.DeserializeObject<GoogleTokenModel>(response);

            return new GoogleUserDetails
            {
                Email = userInfo.email,
                FirstName = userInfo.given_name,
                LastName = userInfo.family_name,
                Locale = userInfo.locale,
                Name = userInfo.name,
                ProviderUserId = userInfo.sub
            };
        }
    }
}
