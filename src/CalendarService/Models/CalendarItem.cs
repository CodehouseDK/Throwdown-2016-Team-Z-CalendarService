using System;
using System.Linq;
using static System.String;

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
            return Join(" ", Start, Subject, Categories.FirstOrDefault());
        }

        public string StartText => WriteDate(Start);
        public string EndText => WriteDate(End);

        private string WriteDate(DateTime date)
        {
            if (date == DateTime.MinValue)
            {
                return Empty;
            }

            if (date.Date == date)
            {
                return date.ToString("d/M");
            }

            return date.ToString("d/M HH:mm");
        }
    }
}