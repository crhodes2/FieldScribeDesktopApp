using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Fieldscribe_Windows_App.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using Fieldscribe_Windows_App.Infrastructure;

namespace Fieldscribe_Windows_App.Controllers
{
    public class MeetsController
    {        
        public IList<Meet> RetrieveAllMeets()
        {
            JObject jsonMeetObj = JObject.Parse(FieldScribeAPIRequests.GETAsync(
                FieldScribeAPIRequests.FieldScribeAPIRootAddress + 
                "meets?limit=100&orderBy=meetDate%20desc"));

            IList<JToken> results = jsonMeetObj["value"].Children().ToList();

            IList<Meet> meets = new List<Meet>();

            foreach (JToken item in results)
                meets.Add(item.ToObject<Meet>());

            return meets;
        }

        public int GetMeetID(Meet meet)
        {
            int id = meet.MeetId;
            return id;
        }
        
        
        // Probably can limit return type later, but returning full HttpResponse for now
        // for more flexiblity. I want to have logic based on the StatusCode within the 
        // HttpResponseMessage. Could handle that here later.

        public HttpResponseMessage AddMeet(Meet meet)
        {
            string jsonMeetObj = JsonConvert.SerializeObject(meet);

            var response = FieldScribeAPIRequests.POSTJsonWithTokenAsync(
                jsonMeetObj, "meets", TokenManager.Instance.Token);

            return response;            
        }

        public HttpResponseMessage DeleteMeet(Meet meet)
        {
            int meetId = meet.MeetId;

            var response = FieldScribeAPIRequests.POSTJsonWithTokenAsync(
                "", "meets/" + meet.MeetId + "/delete",
                TokenManager.Instance.Token);

            return response;
        }

        public HttpResponseMessage EditMeet(Meet meet)
        {
            string jsonMeetObj = JsonConvert.SerializeObject(meet);

            var response = FieldScribeAPIRequests.POSTJsonWithTokenAsync(
                jsonMeetObj, "meets/" + meet.MeetId + "/edit",
                TokenManager.Instance.Token);

            return response;
        }
    }
}
