using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Newtonsoft.Json;

namespace TeamZ.CalendarService.Services
{
    public interface INotificationService
    {
        Task Send(string message);
        Task Send(object obj);
    }

    public class WebSocketsService : INotificationService
    {
        private static readonly ConcurrentBag<WebSocket> _sockets = new ConcurrentBag<WebSocket>();

        public static void AddSocket(WebSocket socket) => _sockets.Add(socket);

        public async Task Send(string message)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(message);
            await Task.WhenAll(_sockets.Where(s => s.State == WebSocketState.Open).Select(async socket =>
            {
                await socket.SendAsync(new ArraySegment<byte>(bytes, 0, bytes.Length), WebSocketMessageType.Text, true, CancellationToken.None);
            }));
        }

        public async Task Send(object obj)
        {
            var json = JsonConvert.SerializeObject(obj);
            await Send(json);
        }
    }

    public static class WebSocketsStartupExtensions
    {
        public static IApplicationBuilder UseWebSocketsServer(this IApplicationBuilder app)
        {
            app.Use(async (http, next) =>
            {
                if (!http.WebSockets.IsWebSocketRequest)
                {
                    await next();
                    return;
                }

                var websocket = await http.WebSockets.AcceptWebSocketAsync();
                if (websocket != null && websocket.State == WebSocketState.Open)
                {
                    WebSocketsService.AddSocket(websocket);
                }
            });

            return app;
        }
    }
}
