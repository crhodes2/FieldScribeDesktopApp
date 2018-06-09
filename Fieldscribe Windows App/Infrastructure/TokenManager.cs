using AspNet.Security.OpenIdConnect.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Fieldscribe_Windows_App.Infrastructure
{
    class TokenManager
    {
        private static TokenManager _instance = null;
        private string _token = "";
        private Credentials _creds = new Credentials { Username = "", Password = "" };
        private DateTime _expireTime = DateTime.MinValue;
        private static readonly object padlock = new object();

        private TokenManager() { }

        public Credentials UserCredentials
        {
            get { return _creds; }
            set { _creds = value; }
        }

        public string Token 
        {
            get
            {
                if (_expireTime < DateTime.Now.AddSeconds(-300))
                {
                    GetToken();                 
                }
                return _token;
            }
            set
            {
                _token = value;
            }
        }


        // Singleton for instance of TokenManager
        public static TokenManager Instance
        {
            get
            {
                lock (padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new TokenManager();
                    }
                    return _instance;
                }
            }
        }


        public (bool success, string Error) GetToken()
        {
            OpenIdConnectRequest request = new OpenIdConnectRequest
            {
                GrantType = "password",
                Username = _creds.Username,
                Password = _creds.Password
            };

            List<KeyValuePair<string, string>> data 
                = new List<KeyValuePair<string, string>>();

            data.Add(new KeyValuePair<string, string>("grant_type", "password"));
            data.Add(new KeyValuePair<string, string>("username", _creds.Username));
            data.Add(new KeyValuePair<string, string>("password", _creds.Password));

            FormUrlEncodedContent content = new FormUrlEncodedContent(data);

            string jsonMeetObj = String.Concat("grant_type=password&username=",
                _creds.Username, "&password=", _creds.Password);

            var response = FieldScribeAPIRequests.POSTUrlEncodedAsync(
                content, "token");

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Task<string> receiveStream = response.Content.ReadAsStringAsync();

                TokenResponse tokenObject = JsonConvert
                    .DeserializeObject<TokenResponse>(receiveStream.Result);

                _token = tokenObject.access_token;
                _expireTime = DateTime.Now.AddSeconds(tokenObject.expires_in);

                return (true, null);
            }

            _token = "";

            // Return error message instea of null later if needed
            return (false, null);
        }
    }
}
