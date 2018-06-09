
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fieldscribe_Windows_App.Models;
using Newtonsoft.Json;

namespace Fieldscribe_Windows_App
{
    public class FieldScribeFileDataController
    {
        public bool AddMeetFiles(
            int meetId, ILynxFileService fileService, string token)
        {
            var lynxFiles = new LynxFiles
            {
                MeetId = meetId,
                PPLFileText = fileService.GetPPL(),
                SCHFileText = fileService.GetSCH(),
                EVTFileText = fileService.GetEVT()
            };

            string jsonLynxFiles = JsonConvert.SerializeObject(lynxFiles);
         
            var response = FieldScribeAPIRequests.POSTJsonWithTokenAsync(
                jsonLynxFiles, "lynx", token)
                .StatusCode;

            if(response == System.Net.HttpStatusCode.Created)           
                return true;            

            return false;
        }
    }
}
