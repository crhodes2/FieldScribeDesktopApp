
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Fieldscribe_Windows_App.Controllers;
using Fieldscribe_Windows_App;

namespace FieldScribeWindowsAppUnitTest
{
    [TestClass]
    public class BasicControllerTests
    {
        // Test the athlete controller class
        [TestMethod]
        public void TestGetAllAthletes()
        {
            AthleteController athletesController = new AthleteController();
            athletesController.GetAllAthletes();
        }

        [TestMethod]
        public void TestGetAthleteByAthleteId()
        {
            AthleteController athletesController = new AthleteController();
            var newAthlete = athletesController.GetAthleteByAthleteId(5);
        }

        [TestMethod]
        public void TestGetAthletesByMeetId()
        {
            AthleteController athleteController = new AthleteController();
            var athletesList = athleteController.GetAthletesByMeetId(2);
        }

        //[TestMethod]
        //[ExpectedException(typeof(InvalidOperationException))]
        //public void TestGetAthletesByMeetId_MeetDoesNotExist()
        //{
        //    AthleteController athleteController = new AthleteController();
        //    var athletesList = athleteController.GetAthletesByMeetId(200);
        //}

        // Test the entries Controller class
        [TestMethod]
        public void TestGetEntryByEntryId()
        {
            EntriesController entryController = new EntriesController();
            var entry = entryController.GetEntryByEntryId(5);
        }

        [TestMethod]
        public void TestGetEntriesByEventId()
        {
            EntriesController entryContoller = new EntriesController();
            var entriesList = entryContoller.GetEntriesByEventId(7);
        }

        // Test the events controller class
        [TestMethod]
        public void TestGetEventsByMeetId()
        {
            EventsController eventController = new EventsController();
            var eventsList = eventController.GetEventsByMeetId(5);
        }

        [TestMethod]
        public void TestGetEventByEventId()
        {
            EventsController eventController = new EventsController();
            var newEvent = eventController.GetEventByEventId(1);
        }

        [TestMethod]
        public void TestGetEventsByAthleteId()
        {
            EventsController eventController = new EventsController();
            var newEvent = eventController.GetEventsByAthleteId(20);
        }
    }
}
