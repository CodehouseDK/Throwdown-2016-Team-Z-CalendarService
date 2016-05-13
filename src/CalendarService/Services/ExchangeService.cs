using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Extensions.Configuration;
using TeamZ.CalendarService.Models;

namespace TeamZ.CalendarService.Services
{
    public interface IExchangeService
    {
        Task<IEnumerable<CalendarItem>> GetAppointments(string username);
        Task<IEnumerable<CalendarItem>> OnVacation(DateTime date);
    }

    public class ExchangeService : IExchangeService
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string ServiceAddress { get; set; }


        private readonly HttpClient _client;

        public ExchangeService(IConfiguration configuration)
        {
            Username = configuration["ExchangeUser"];
            Password = configuration["ExchangePassword"];
            ServiceAddress = configuration["ExchangeService"] ?? "https://mail.codehouse.com/ews/exchange.asmx";

            var handler = new HttpClientHandler
            {
                AllowAutoRedirect = false,
                Credentials = new NetworkCredential(Username, Password, "AD"),
            };
            _client = new HttpClient(handler);
        }

        public async Task<IEnumerable<CalendarItem>> OnVacation(DateTime date)
        {
            var appointmentTasks = CalendarFolderMap.Ids.Keys
                .Select(x => GetAppointments(x, date.Date, date.Date.AddDays(7), 50))
                .ToList();
            await Task.WhenAll(appointmentTasks);
            var outOfOfficeItems = appointmentTasks
                .SelectMany(task => task.Result.Where(item => item.OutOfOffice && item.IsAllDay))
                .OrderBy(x => x.Start)
                .ToList();

            return outOfOfficeItems;
        }

        public async Task<IEnumerable<CalendarItem>> GetAppointments(string username)
        {
            return await GetAppointments(username, DateTime.Now.AddMinutes(-15), DateTime.Now.Date.AddDays(1), 10);
        }

        public async Task<IEnumerable<CalendarItem>> GetAppointments(string username, DateTime fromDate, DateTime endDate, int maxItems)
        {
            const string xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:m=\"http://schemas.microsoft.com/exchange/services/2006/messages\" xmlns:t=\"http://schemas.microsoft.com/exchange/services/2006/types\" xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\">  <soap:Header>    <t:RequestServerVersion Version=\"Exchange2013_SP1\" />  </soap:Header>  <soap:Body>    <m:FindItem Traversal=\"Shallow\">    <m:ItemShape>        <t:BaseShape>IdOnly</t:BaseShape>        <t:AdditionalProperties>          <t:FieldURI FieldURI=\"item:Subject\" />          <t:FieldURI FieldURI=\"calendar:Location\" />          <t:FieldURI FieldURI=\"calendar:Start\" />          <t:FieldURI FieldURI=\"calendar:End\" />          <t:FieldURI FieldURI=\"item:Categories\" />          <t:FieldURI FieldURI=\"calendar:IsAllDayEvent\" />          <t:FieldURI FieldURI=\"calendar:IsRecurring\" />      <t:FieldURI FieldURI=\"calendar:LegacyFreeBusyStatus\" xmlns:t=\"http://schemas.microsoft.com/exchange/services/2006/types\" />  </t:AdditionalProperties>      </m:ItemShape>      <m:CalendarView MaxEntriesReturned=\"{3}\" StartDate=\"{1}\" EndDate=\"{2}\" />      <m:ParentFolderIds>        <t:FolderId Id=\"{0}\" />      </m:ParentFolderIds>    </m:FindItem>  </soap:Body></soap:Envelope>";
            string folderId;
            if (!CalendarFolderMap.Ids.TryGetValue(username.ToLowerInvariant(), out folderId))
            {
                throw new InvalidOperationException("Unknown user: " + username);
            }

            const string dateTimeFormat = "yyyy-MM-ddTHH:mm:ss.000Z";
            var request = new StringContent(string.Format(xml, folderId, fromDate.ToString(dateTimeFormat), endDate.ToString(dateTimeFormat), maxItems));
            request.Headers.ContentType = new MediaTypeHeaderValue("text/xml");

            var response = await _client.PostAsync(ServiceAddress, request);
            var xmlResult = await response.Content.ReadAsStringAsync();
            return GetCalendarItemsFromXml(xmlResult, username);
        }

        public IEnumerable<CalendarItem> GetCalendarItemsFromXml(string xml, string username)
        {
            var doc = new XmlDocument();
            doc.LoadXml(xml);
            
            var body = doc.DocumentElement.FirstChild.NextSibling;
            var responseElement = body.FirstChild.FirstChild.FirstChild;
            var folder = responseElement.FirstChild.NextSibling;
            var items = folder.FirstChild.ChildNodes;

            const string itemNamespace = "http://schemas.microsoft.com/exchange/services/2006/types";
            foreach (XmlNode item in items)
            {
                var subject = item["Subject", itemNamespace]?.InnerText;
                var location = item["Location", itemNamespace]?.InnerText;
                var status = item["LegacyFreeBusyStatus", itemNamespace]?.InnerText;
                var categories = item["Categories", itemNamespace]?.ChildNodes.OfType<XmlElement>()
                    .Select(x => x.InnerText)
                    .ToArray() ?? new string[0];
                DateTime start, end;
                DateTime.TryParse(item["Start", itemNamespace]?.InnerText, out start);
                DateTime.TryParse(item["End", itemNamespace]?.InnerText, out end);
                bool recurring, allDay;
                bool.TryParse(item["IsRecurring", itemNamespace]?.InnerText, out recurring);
                bool.TryParse(item["IsAllDayEvent", itemNamespace]?.InnerText, out allDay);

                var resultItem = new CalendarItem
                {
                    Username = username,
                    Start = start,
                    End = end,
                    Subject = subject,
                    Status = status,
                    Location = location,
                    Categories = categories,
                    IsRecurring = recurring,
                    IsAllDay = allDay
                };
                yield return resultItem;
            }
        }
    }
}
