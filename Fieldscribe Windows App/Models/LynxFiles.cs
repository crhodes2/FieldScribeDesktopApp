
namespace Fieldscribe_Windows_App.Models
{
    // This model will only be used to POST the lynx data to the API
    public class LynxFiles
    {
        public int MeetId { set; get; }
        public string PPLFileText { get; set; }
        public string SCHFileText { set; get; }
        public string EVTFileText { set; get; }
    }
}
