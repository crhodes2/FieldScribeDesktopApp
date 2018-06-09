// Author: Alexander Ott
// Date:   5/11/2018

using Fieldscribe_Windows_App.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Fieldscribe_Windows_App.Controllers
{
    public class AthleteController
    {
        
        // Example URL: https://fieldscribeapi2017.azurewebsites.net/athletes
        public IList<Athlete> GetAllAthletes()
        {
            try
            {
                IList<JToken> athleteTokens;
                IList<Athlete> athletes = new List<Athlete>();

                // Get the first athlete and determine the total number of athletes
                JObject jsonAthleteObj = JObject.Parse(FieldScribeAPIRequests.GETAsync(FieldScribeAPIRequests.FieldScribeAPIRootAddress + "athletes?limit=1"));

                // Add the first athlete to the athletes list
                athletes.Add(jsonAthleteObj["value"].Children().ToList().First().ToObject<Athlete>());

                // Set the var that holds the total athlete count
                var totalAthletes = Convert.ToUInt32(jsonAthleteObj["size"].ToString());

                // Can not have an offset of zero, start at one and get all athletes in the database
                for (int offsetNum = 1; offsetNum < totalAthletes; offsetNum += 100)
                {
                    jsonAthleteObj = JObject.Parse(
                       FieldScribeAPIRequests.GETAsync(
                           FieldScribeAPIRequests.FieldScribeAPIRootAddress + "athletes?limit=100&offset=" + offsetNum));

                    athleteTokens = jsonAthleteObj["value"].Children().ToList();

                    // Add current 100 athlete tokens to the athlete list
                    foreach (JToken item in athleteTokens)
                        athletes.Add(item.ToObject<Athlete>());
                }
        
                return athletes;
            }
            catch(Exception ex)
            {
                MessageBox.Show("Failed to get all athletes!", "Unexpected Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                throw ex;
            }
        }

        // Example URL: https://fieldscribeapi2017.azurewebsites.net/athletes/99 
        public Athlete GetAthleteByAthleteId(int athleteId)
        {
            try
            {
                JObject jsonAthleteObj = JObject.Parse(
                    FieldScribeAPIRequests.GETAsync(
                        FieldScribeAPIRequests.FieldScribeAPIRootAddress + "athletes/" + athleteId + "?limit=1"));

                var athlete = new Athlete
                {
                    AthleteId = Convert.ToInt32(jsonAthleteObj["athleteId"].ToString()),
                    MeetId = Convert.ToInt32(jsonAthleteObj["meetId"].ToString()),
                    CompNum = Convert.ToInt32(jsonAthleteObj["compNum"].ToString()),
                    FirstName = jsonAthleteObj["firstName"].ToString(),
                    LastName = jsonAthleteObj["lastName"].ToString(),
                    TeamName = jsonAthleteObj["teamName"].ToString(),
                    Gender = jsonAthleteObj["gender"].ToString()
                };

                return athlete;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to get athlete by athlete id!", "Unexpected Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                throw ex;
            }

        }

        // TODO: Refactor GetAthletesByMeetId method, very similar code to the GetAllAthletes mehtod above
        // Example URL: https://fieldscribeapi2017.azurewebsites.net/meets/1/athletes
        public IList<Athlete> GetAthletesByMeetId(int meetId)
        {
            try
            {               
                IList<JToken> athleteTokens;
                IList<Athlete> athletes = new List<Athlete>();

                JObject jsonAthletesObj = JObject.Parse(
                    FieldScribeAPIRequests.GETAsync(
                        FieldScribeAPIRequests.FieldScribeAPIRootAddress + "meets/" + meetId + "/athletes?limit=1"));

                // Set the var that holds the total athlete count
                var totalAthletes = Convert.ToUInt32(jsonAthletesObj["size"].ToString());

                // When the meet does not exist expect 0 total athletes 
                if (totalAthletes < 1)
                    throw new InvalidOperationException();

                // Add the first athlete to the athletes list
                athletes.Add(jsonAthletesObj["value"].Children().ToList().First().ToObject<Athlete>());


                // Can not have an offset of zero, start at one and get all athletes in the database
                for (int offsetNum = 1; offsetNum < totalAthletes; offsetNum += 100)
                {
                    jsonAthletesObj = JObject.Parse(
                       FieldScribeAPIRequests.GETAsync(
                           FieldScribeAPIRequests.FieldScribeAPIRootAddress + "meets/" + meetId + "/athletes?limit=100&offset=" + offsetNum));

                    athleteTokens = jsonAthletesObj["value"].Children().ToList();

                    // Add current 100 athlete tokens to the athlete list
                    foreach (JToken item in athleteTokens)
                        athletes.Add(item.ToObject<Athlete>());
                }

                return athletes;
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to get athletes by meet id!", "Unexpected Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                throw ex;
            }
        }      
    }
}
