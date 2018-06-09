using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Fieldscribe_Windows_App
{
    public static class FieldScribeAPIRequests
    {
        // live server
        public const string FieldScribeAPIRootAddress = "https://fieldscribeapi2017.azurewebsites.net/";

        // local host (for testing / debugging)
        //public const string FieldScribeAPIRootAddress = "https://localhost:44357/";

        private static readonly HttpClient client = new HttpClient();

        public static string GETAsync(string URL)
        {
            return client.GetStringAsync(URL).Result;
        }

        public static HttpResponseMessage GETwithTokenAsync(string URLExtension, string token)
        {
            client.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Bearer", token);

            return client.GetAsync(
                FieldScribeAPIRootAddress + URLExtension).Result;
                
        }

        public static HttpResponseMessage POSTJsonWithTokenAsync(
            string jsonObject, string URLExtension, string token)
        {
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            return client.PostAsync(
                FieldScribeAPIRootAddress + URLExtension,
                new StringContent(jsonObject.ToString(), 
                Encoding.UTF8, "application/json"))
                .Result;
        }

        public static HttpResponseMessage POSTUrlEncodedAsync(
            FormUrlEncodedContent urlContent, string URLExtension)
        {
            return client.PostAsync(
                FieldScribeAPIRootAddress + URLExtension, urlContent)
                .Result;
        }
    }
}
