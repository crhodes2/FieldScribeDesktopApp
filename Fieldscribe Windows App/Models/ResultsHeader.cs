// Author: Alexander Ott
// Date:   5/12/2018

namespace Fieldscribe_Windows_App.Models
{
    public class ResultsHeader
    {
        public int EventId { get; set; } // Only for use in the ResultsBuilder class, is the id in the data base

        public int EventNum { get; set; } // From the event controller

        public int RoundNum { get; set; } // From the event controller

        public int FlightNum { get; set; } // From the event controller

        public string EventName { get; set; } // From the event contoller

        public string MeasurementSystem { get; set; } // From the event contoller
    }
}
