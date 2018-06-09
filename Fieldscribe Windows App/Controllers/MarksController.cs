using Fieldscribe_Windows_App.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fieldscribe_Windows_App.Controllers
{
    public class MarksController
    {
        public Mark RetrieveMarksByEntryId(int entryId)
        {
            JObject jsonMarkObj = JObject.Parse(FieldScribeAPIRequests.GETAsync(FieldScribeAPIRequests.FieldScribeAPIRootAddress + "entries/" + entryId + "/marks?limit=1"));

            return jsonMarkObj["value"].Children().ToList().First().ToObject<Mark>();
        }
    }
}
