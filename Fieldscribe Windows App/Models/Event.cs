// Author: Alexander Ott
// Date:   5/12/2018

using System.Collections.Generic;

namespace Fieldscribe_Windows_App.Models
{
    public class Event
    {
        public int EventId { get; set; }

        public int MeetId { get; set; }

        public int EventNum { get; set; }

        public int RoundNum { get; set; }

        public int FlightNum { get; set; }

        public string EventName { get; set; }

        public string MeasurementType { get; set; }

        public string EventType { get; set; }

        public decimal Precision { get; set; }

        public decimal Maximum { get; set; }

        public List<BarHeight> BarHeights { get; set; }
    }

    public class BarHeight
    {
        public int HeightNum { get; set; }
        public string Height { get; set; }
    }
}
