using Fieldscribe_Windows_App.Controllers;
using Fieldscribe_Windows_App.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Controls;


namespace Fieldscribe_Windows_App
{
    /// <summary>
    /// Interaction logic for DetailsUserControl.xaml
    /// </summary>
    public partial class DetailsUserControl : UserControl
    {
        private int currentMeetId = -1; // This will hold the current meet's id value
        private bool resultsBuilt = false;

        public DetailsUserControl()
        {
            InitializeComponent();


            if(AppDataModel.Instance.SelectedMeet != null)
                RefreshScreen();
        }

        private void RefreshResultsBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // Only refresh results if there is a meet selected and shared folder selected
            if (AppDataModel.Instance.SelectedMeet != null && AppDataModel.Instance.FolderPath != String.Empty)
            {
                ResultsBuilder resultsBuilder = new ResultsBuilder();
                resultsBuilder.CreateAllResultsFiles(currentMeetId, AppDataModel.Instance.FolderPath);
                ReadResultsEntries();
                resultsBuilt = true;
            }
        }

        public void RefreshScreen()
        {
            if (AppDataModel.Instance.SelectedMeet != null)
            {
                // Only refresh the screen when a new meet has been selected
                if (currentMeetId != AppDataModel.Instance.SelectedMeet.MeetId)
                {
                    currentMeetId = AppDataModel.Instance.SelectedMeet.MeetId;
                    eventsListBox.Items.Clear();
                    PopulateEventsList();
                }
            }
        }

        private void PopulateEventsList()
        {
            if (AppDataModel.Instance.SelectedMeet != null)
            {
                EventsController eventController = new EventsController();
                var events = eventController.GetEventsByMeetId(currentMeetId);

                foreach (Event athleticEvent in events)
                    eventsListBox.Items.Add(athleticEvent.FlightNum.ToString() + "," + athleticEvent.EventName);
            }
        }

        // This will be used for the view of the athlete results
        private List<SimpleResultInfo> ReadResultsEntries()
        {
            List<SimpleResultInfo> eventResults = new List<SimpleResultInfo>();
            List<string> tempEntriesList = new List<string>();

            // TODO: Add the file info to a list of EventResults
            foreach (string file in Directory.EnumerateFiles(AppDataModel.Instance.FolderPath, "*.lff"))
            {
                string[] lines = System.IO.File.ReadAllLines(file);

                string header = lines[0];

                for (int i = 1; i < lines.Length; i++)
                    tempEntriesList.Add(lines[i]);                   

                eventResults.Add(new SimpleResultInfo
                {
                    Header = header,
                    Entries = tempEntriesList.ToList()
                });

                tempEntriesList.Clear();
            }

            return eventResults;
        }

        private void eventsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (eventsListBox.Items.Count > 1 && resultsBuilt == true)
            {
                var curItem = eventsListBox.SelectedItem.ToString();

                var tempList = ReadResultsEntries();

                var resultsItem = tempList.Find(x => x.Header.Contains(curItem));

                StringBuilder viewText = new StringBuilder();
                resultsPreviewTxt.Clear();

                viewText.Append(String.Concat(resultsItem.Header, Environment.NewLine));

                foreach (var entryItem in resultsItem.Entries)
                    viewText.Append(String.Concat(entryItem, Environment.NewLine));

                resultsPreviewTxt.Text = viewText.ToString();

                FillResultsTable(resultsItem);
            }
        }

        private void FillResultsTable(SimpleResultInfo resultsItem)
        {
            List<ResultsEntry> items = new List<ResultsEntry>();

            foreach (var item in resultsItem.Entries)
            {
                string[] tempStrings= item.Split(',');
                

                items.Add(new ResultsEntry()
                {
                    FlightPlace = tempStrings[0],
                    AthleteId = tempStrings[1],
                    CompetePosition = tempStrings[2],
                    EventPlace = tempStrings[3],
                    LastName = tempStrings[4],
                    FirstName = tempStrings[5],
                    Affiliation = tempStrings[6],
                });

                for (int index = 7; index < tempStrings.Length; ++index)
                    items.Last().Mark += String.Concat(tempStrings[index], "   ");
            }

            resultsListView.ItemsSource = items;
        }
    }
}
