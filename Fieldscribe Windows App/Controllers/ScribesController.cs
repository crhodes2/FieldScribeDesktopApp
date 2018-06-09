using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Fieldscribe_Windows_App.Models;
using System.Net;
using System.IO;
using Fieldscribe_Windows_App;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Formatting;


namespace Fieldscribe_Windows_App.Controllers
{
    class ScribesController
    {
        static HttpClient aScribe = new HttpClient();
        
        public IList<Scribes> RetrieveScribes()
        {
            IList<Scribes> scribes = new List<Scribes>();
            return scribes;
        }

        public int GetScribeId(Scribes scribe)
        {
            int ScribeId = scribe.ScribeId;
            return ScribeId;
        }

        static async Task<Uri> GetUserCredentials(Scribes scribe)
        {
           
            HttpResponseMessage response = await aScribe.PostAsJsonAsync("/token", scribe);
            response.EnsureSuccessStatusCode();
            
            return response.Headers.Location;
        }
    }
}
