using System;
using System.Linq;

namespace TeamZ.CalendarService.Models
{
    public class CalendarItem
    {
        public string Subject { get; set; }
        public string[] Categories { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Location { get; set; }
        public bool IsRecurring { get; set; }
        public bool IsAllDay { get; set; }

        public override string ToString()
        {
            return string.Join(" ", Start, Subject, Categories.FirstOrDefault());
        }
    }
}