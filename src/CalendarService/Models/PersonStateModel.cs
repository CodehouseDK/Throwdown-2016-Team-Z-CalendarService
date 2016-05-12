namespace TeamZ.CalendarService.Models
{
    public class PersonStateModel
    {
        public string Type { get { return "PersonStateModel"; } }
        public string CurrentUsername { get; set; }
        public CalendarItem[] Entries { get; set; }
    }
}
