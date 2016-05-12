using System;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace TeamZ.CalendarService.Services
{
    public class RedisSubscriberService
    {
        private readonly ConnectionMultiplexer _connection;
        private string _userChannelName;

        public RedisSubscriberService(IConfiguration configuration)
        {
            var server = configuration["RedisServer"];
            _userChannelName = configuration["RedisUserChannelName"];
            _connection = ConnectionMultiplexer.Connect(server);
        }

        public void Subscribe(Action<string> handler)
        {
            _connection.GetSubscriber().Subscribe(_userChannelName, (channel, value) => { handler.Invoke(value.ToString()); });
        }

        public void Test(string username)
        {
            _connection.GetSubscriber().Publish(_userChannelName, username);
        }
    }
}
