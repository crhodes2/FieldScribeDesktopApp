// Author: Alexander Ott
// Date:   5/16/2018


using Fieldscribe_Windows_App.Controllers;
using Fieldscribe_Windows_App.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fieldscribe_Windows_App
{
    public class ResultsBuilder
    {
        public async void CreateAllResultsFiles(int meetId, string folderPath)
        {
            if (folderPath.Last() != '\\')
                folderPath += "\\";

            // Before building the results file delete all existing results files
            DeleteAllResultsFiles(folderPath, ".lff");

            EventsController eventContoller = new EventsController();
            AthleteController athleteController = new AthleteController();
            EntriesController entriesController = new EntriesController();
            MarksController markController = new MarksController();

            List<ResultsHeader> resultsHeader = new List<ResultsHeader>();

            var eventsList = eventContoller.GetEventsByMeetId(meetId);

            foreach (Event item in eventsList)
            {
                resultsHeader.Add(new ResultsHeader
                {
                    EventId = item.EventId,
                    EventNum = item.EventNum,
                    RoundNum = item.FlightNum,
                    FlightNum = item.FlightNum,
                    EventName = item.EventName,
                    MeasurementSystem = item.MeasurementType
                });
            }

            StringBuilder headerText = new StringBuilder();
            StringBuilder eventFileName = new StringBuilder();

            foreach (ResultsHeader item in resultsHeader)
            {
                // Build the file name for the event results file
                eventFileName.Append(
                    String.Concat(
                        folderPath,
                        item.EventNum.ToString().Length < 3 ? item.EventNum.ToString().PadLeft(3, '0') : item.EventNum.ToString() , "-",
                        item.RoundNum.ToString(), "-",
                        item.FlightNum.ToString(), ".lff"));

                // Adds the header to each file
                using (StreamWriter writer = new StreamWriter(eventFileName.ToString(), true))
                {
                    headerText.Append(String.Concat(
                        item.EventNum.ToString(), ",",
                        item.RoundNum.ToString(), ",",
                        item.FlightNum.ToString(), ",",
                        item.EventName, ",",
                        item.MeasurementSystem, Environment.NewLine));

                    await writer.WriteAsync(headerText.ToString());
                    writer.Close();
                    headerText.Clear();
                }

                await AddAthleteResults(
                    eventFileName.ToString(), 
                    item.EventId, 
                    athleteController, 
                    entriesController,
                    markController);

                // Clear the StringBuilder for the next file name
                eventFileName.Clear();
            }
        }

        // Add athlete results for specific event
        async Task AddAthleteResults(string filePath, 
            int eventId,
            AthleteController athleteController, 
            EntriesController entriesController, 
            MarksController markController)
        {
            var entriesInfoList = entriesController.GetEntriesByEventId(eventId);
            List<ResultsEntry> entryResultsList = new List<ResultsEntry>();

            foreach(Entry item in entriesInfoList)
            {
                entryResultsList.Add(new ResultsEntry
                {
                    FlightPlace = item.FlightPlace.ToString(),
                    AthleteId = item.AthleteID.ToString(),
                    // CompetePosition = item.CompetePosition.ToString(), This will has the potential to cause errors in the Field Lynx application
                    EventPlace = string.Empty, // Not concerned about event place for mvp
                    LastName = item.LastName,
                    FirstName = item.FirstName,
                    Affiliation = item.TeamName,
                    Wind = string.Empty // Not concerned about wind for mvp
                });

                // Add the marks to the entry result
                if (item.Marks.Count() > 0)
                {
                    StringBuilder marksString = new StringBuilder();

                    foreach (Mark markItem in item.Marks)
                        marksString.Append(String.Concat(markItem.mark, ","));

                    entryResultsList.Last().Mark = marksString.ToString();
                }
                else
                    entryResultsList.Last().Mark = String.Empty;
            }

            StringBuilder entryText = new StringBuilder();

            foreach (ResultsEntry item in entryResultsList)
            {
                // Adds the header to each file
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    entryText.Append(String.Concat(
                        item.FlightPlace.ToString(), ",",
                        item.AthleteId.ToString(), ",",
                        String.Empty, ",", // This is where compete position would be
                        item.EventPlace, ",",
                        item.LastName, ",",
                        item.FirstName, ",",
                        item.Affiliation, ",",
                        item.Mark,
                        item.Wind, ",", Environment.NewLine));

                        await writer.WriteAsync(entryText.ToString());
                        entryText.Clear();
                        writer.Close();
                }
            }
        }

        // Deletes all .lff files
        void DeleteAllResultsFiles(string path, params string[] extensions)
        {
            List<FileInfo> fileInfoList = new List<FileInfo>();
            foreach (string ext in extensions)
            {
                fileInfoList.AddRange(new DirectoryInfo(path).GetFiles("*" + ext)
                    .Where(p => p.Extension.Equals(ext, StringComparison.CurrentCultureIgnoreCase)).ToArray());
            }

            foreach (FileInfo file in fileInfoList)
            {
                try
                {
                    file.Attributes = FileAttributes.Normal;
                    File.Delete(file.FullName);
                }
                catch (Exception ex)
                {
                    // Do something here
                }
            }
        }
    }
}


