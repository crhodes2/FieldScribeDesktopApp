// Author: Alexander Ott
// Date:   5/12/2018

using Fieldscribe_Windows_App.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Fieldscribe_Windows_App.Controllers
{
    public class EntriesController
    {
        // Example URL: https://fieldscribeapi2017.azurewebsites.net/entries/1
        public Entry GetEntryByEntryId(int entryId)
        {
            try
            {
                JObject jsonEntryObj = JObject.Parse(
                    FieldScribeAPIRequests.GETAsync(
                        FieldScribeAPIRequests.FieldScribeAPIRootAddress + "entries/" + entryId + "?limit=1"));

                var entry = new Entry
                {
                    EntryID = Convert.ToInt32(jsonEntryObj["entryID"].ToString()),
                    EventID = Convert.ToInt32(jsonEntryObj["eventID"].ToString()),
                    AthleteID = Convert.ToInt32(jsonEntryObj["athleteID"].ToString()),
                    CompNum = Convert.ToInt32(jsonEntryObj["compNum"].ToString()),
                    CompetePosition = Convert.ToInt32(jsonEntryObj["competePosition"].ToString()),
                    LastName = jsonEntryObj["lastName"].ToString(),
                    FirstName = jsonEntryObj["firstName"].ToString(),
                    TeamName = jsonEntryObj["teamName"].ToString(),
                    Marks = jsonEntryObj["marks"].ToObject<Mark[]>()
                };

                return entry;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to get entry by entry id!", "Unexpected Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                throw ex;
            }
        }

        // Example URL: https://fieldscribeapi2017.azurewebsites.net/events/1/entries
        public IList<Entry> GetEntriesByEventId(int eventId)
        {
            try
            {
                JObject jsonEntryObj = JObject.Parse(
                        FieldScribeAPIRequests.GETAsync(
                            FieldScribeAPIRequests.FieldScribeAPIRootAddress + "events/" + eventId + "/entries?limit=100"));

                IList<Entry> entriesList = new List<Entry>();

                var tokens = jsonEntryObj["value"].Children();

                foreach (JToken token in tokens)
                    entriesList.Add(token.ToObject<Entry>());

                return entriesList;
            }
            catch(Exception ex)
            {
                MessageBox.Show("Failed to get entries by event id!", "Unexpected Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                throw ex;
            }
        }
    }
}
