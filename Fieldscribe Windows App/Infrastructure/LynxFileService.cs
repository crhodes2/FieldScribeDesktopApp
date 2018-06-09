using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace Fieldscribe_Windows_App
{
    public class LynxFileService : ILynxFileService
    {
        private string _folderPath;

        public LynxFileService(string folderPath)
        {
            _folderPath = folderPath;
        }

        public bool LynxFilesExist()
        {
            return (
                File.Exists(_folderPath + @"\\lynx.evt") &&
                (File.Exists(_folderPath + @"\\fldlynx.sch") ||
                File.Exists(_folderPath + @"\\lynx.sch")) &&
                File.Exists(_folderPath + @"\\lynx.ppl")
                );
        }

        public string GetSCH()
        {
            string text ="";

            if(File.Exists(_folderPath + @"\\fldlynx.sch"))
            {
                text = ReadText(_folderPath + @"\\fldlynx.sch");
            }
            else if (File.Exists(_folderPath + @"\\lynx.sch"))
            {
                text = ReadText(_folderPath + @"\\lynx.sch");
            }

            return text;
        }

        public string GetPPL()
        {
            string text = ReadText(_folderPath + @"\\lynx.ppl");

            return Regex.Replace(text, "\".*?\"", "");
        }

        public string GetEVT()
        {
            return ReadText(_folderPath + @"\\lynx.evt");
        }

        private string ReadText(string path)
        {
            return File.ReadAllText(path);
        }
    }
}
