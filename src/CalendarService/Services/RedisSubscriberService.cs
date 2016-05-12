using System;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace TeamZ.CalendarService.Services
{
    public class RedisSubscriberService
    {
        private ConnectionMultiplexer _connection;
        private readonly string _userChannelName;
        private readonly string _server;

        private bool connected;

        public RedisSubscriberService(IConfiguration configuration)
        {
            var server = configuration["REDIS_PORT"];
            if (string.IsNullOrEmpty(server))
            {
                server = configuration["RedisServer"];
            }

            const string prefixToRemove = "tcp://";
            if (server.StartsWith(prefixToRemove))
            {
                server = server.Substring(prefixToRemove.Length);
            }

            _server = server;
            _userChannelName = configuration["RedisUserChannelName"];
        }

        private void Connect()
        {
            if (_connection != null)
            {
                return;
            }

            _connection = ConnectionMultiplexer.Connect(_server);
        }

        public void Subscribe(Action<string> handler)
        {
            Connect();
            _connection.GetSubscriber().Subscribe(_userChannelName, (channel, value) => { handler.Invoke(value.ToString()); });
        }

        public void Test(string username)
        {
            _connection.GetSubscriber().Publish(_userChannelName, username);
        }
    }
}
