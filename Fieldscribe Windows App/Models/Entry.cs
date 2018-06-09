
namespace Fieldscribe_Windows_App.Models
{
    public class Entry
    {
        public int EntryID { get; set; }

        public int EventID { get; set; }

        public int AthleteID { get; set; }

        public int CompNum { get; set; }

        public int CompetePosition { get; set; }

        public int FlightPlace { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string TeamName { get; set; }

        public Mark[] Marks { get; set; }
    }
}
