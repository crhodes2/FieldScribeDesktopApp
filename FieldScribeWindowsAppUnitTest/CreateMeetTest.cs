
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Fieldscribe_Windows_App.Models;
using Fieldscribe_Windows_App.Controllers;
using System.Collections.Generic;


namespace FieldScribeWindowsAppUnitTest
{
    [TestClass]
    public class CreateMeetTest
    {
        [TestMethod]
        public void CreateMeet()
        {
            //var MeetController = new MeetsController();
            //var list = new List<Meet>(MeetController.RetrieveAllMeets());

            //var NewTestMeet = new Meet()
            //{
            //    MeetId = 123,
            //    MeetName = "Spring Meet",
            //    MeetDate = DateTime.Now,
            //    MeetLocation = "Pasir, Kansas",
            //    MeasurementType = "Metric"
            //};

            //MeetController.AddMeet(NewTestMeet);

            //var testList = new List<Meet>(MeetController.RetrieveAllMeets());
            //var id = NewTestMeet.MeetId;

            //Assert.AreNotSame(list, testList.Contains(NewTestMeet));
        }
           
        [TestMethod]
        public void CreateWithoutSave()
        {           
            //var MeetController = new MeetsController();
            //var testList = new List<Meet>(MeetController.RetrieveAllMeets());

            //var NewTestMeetWithNameAndDate = new Meet()
            //{
            //    MeetId = 123,
            //    MeetName = "Spring Meet",
            //    MeetDate = DateTime.Now,
            //    MeetLocation = "USA",
            //    MeasurementType = "English"
            //};

            //MeetController.AddMeet(NewTestMeetWithNameAndDate);
            //var testList2 = new List<Meet>(MeetController.RetrieveAllMeets());            
        }
    }
}
