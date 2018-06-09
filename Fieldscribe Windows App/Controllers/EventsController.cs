// Author: Alexander Ott
// Date:   5/12/2018

using Fieldscribe_Windows_App.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Fieldscribe_Windows_App
{
    public class EventsController
    {
        // TODO: Modify algorithm to allow for meets that have more than 100 events. This is important!
        // Example URL: https://fieldscribeapi2017.azurewebsites.net/meets/5/events
        public IList<Event> GetEventsByMeetId(int meetId)
        {
            try
            {
                JObject jsonEventObj = JObject.Parse(
                    FieldScribeAPIRequests.GETAsync(
                        FieldScribeAPIRequests.FieldScribeAPIRootAddress + "meets/" + meetId + "/events?limit=100"));

                IList<JToken> results = jsonEventObj["value"].Children().ToList();

                IList<Event> events = new List<Event>();

                foreach (JToken item in results)
                {
                    events.Add(item.ToObject<Event>());
                    events.Last().MeasurementType = item["params"]["measurementType"].ToString();
                    events.Last().EventType = item["params"]["eventType"].ToString();
                    events.Last().Precision = Convert.ToDecimal(item["params"]["precision"].ToString());
                    events.Last().Maximum = Convert.ToDecimal(item["params"]["maximum"].ToString());
                }

                return events;
            }
            catch (Exception ex)
            {
              //  MessageBox.Show("Failed to get events by meet id!", "Unexpected Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                throw ex;
            }
        }


        // Example URL: https://fieldscribeapi2017.azurewebsites.net/events/88
        public Event GetEventByEventId(int eventId)
        {
            try
            {
                JObject jsonEventObj = JObject.Parse(
                    FieldScribeAPIRequests.GETAsync(
                        FieldScribeAPIRequests.FieldScribeAPIRootAddress + "events/" + eventId + "?limit=1"));

                var _event = new Event
                {
                    EventId = Convert.ToInt32(jsonEventObj["eventId"].ToString()),
                    MeetId = Convert.ToInt32(jsonEventObj["meetId"].ToString()),
                    EventNum = Convert.ToInt32(jsonEventObj["eventNum"].ToString()),
                    RoundNum = Convert.ToInt32(jsonEventObj["roundNum"].ToString()),
                    FlightNum = Convert.ToInt32(jsonEventObj["flightNum"].ToString()),
                    EventName = jsonEventObj["eventName"].ToString(),
                    MeasurementType = jsonEventObj["params"]["measurementType"].ToString(),
                    EventType = jsonEventObj["params"]["eventType"].ToString(),
                    Precision = Convert.ToDecimal(jsonEventObj["params"]["precision"].ToString()),
                    Maximum = Convert.ToDecimal(jsonEventObj["params"]["maximum"].ToString()),
                };

                // TODO: I am not sure what to do with bar heights. The api tends to return null values here.
                var barHeightTokens = jsonEventObj["barHeights"].Children().ToArray();

                foreach (JToken token in barHeightTokens)
                    _event.BarHeights.Add(token.ToObject<BarHeight>());

                return _event;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to get event by event id!", "Unexpected Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                throw ex;
            }
        }

        // Example URL: https://fieldscribeapi2017.azurewebsites.net/athletes/20/events
        public IList<Event> GetEventsByAthleteId(int athleteId)
        {
            try
            {
                JObject jsonEventObj = JObject.Parse(
                    FieldScribeAPIRequests.GETAsync(
                        FieldScribeAPIRequests.FieldScribeAPIRootAddress + "athletes/" + athleteId + "/events?limit=100"));

                IList<JToken> eventTokens = jsonEventObj["value"].Children().ToList();

                IList<Event> events = new List<Event>();

                foreach (JToken item in eventTokens)
                {
                    events.Add(item.ToObject<Event>());
                    events.Last().MeasurementType = item["params"]["measurementType"].ToString();
                    events.Last().EventType = item["params"]["eventType"].ToString();
                    events.Last().Precision = Convert.ToDecimal(item["params"]["precision"].ToString());
                    events.Last().Maximum = Convert.ToDecimal(item["params"]["maximum"].ToString());
                }

                return events;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to get events by athlete id!", "Unexpected Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                throw ex;
            }
        }
    }
}
