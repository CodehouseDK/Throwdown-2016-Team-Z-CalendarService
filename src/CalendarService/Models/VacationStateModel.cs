namespace TeamZ.CalendarService.Models
{
    public class VacationStateModel
    {
        public string Type { get { return "VacationStateModel"; } }
        public CalendarItem[] Entries { get; set; }
    }
}