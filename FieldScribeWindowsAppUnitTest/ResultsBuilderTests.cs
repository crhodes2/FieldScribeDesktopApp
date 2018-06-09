using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Fieldscribe_Windows_App.Controllers;
using Fieldscribe_Windows_App;

namespace FieldScribeWindowsAppUnitTest
{
    [TestClass]
    public class ResultsBuilderTests
    {
        [TestMethod]
        public void TestCreateAllResultsFiles()
        {
            var resultsBuilder = new ResultsBuilder();
            resultsBuilder.CreateAllResultsFiles(3, @"C:\Users\Alexander Ott\Desktop\FieldScribe Test Folder\");
        }
    }
}
