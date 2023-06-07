using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;

namespace CarsUnlimited.CartWorker
{
    public class MessageReceiver : DefaultBasicConsumer
    {
        private readonly IModel _channel;
        private readonly IConfiguration _configuration;

        public MessageReceiver(IModel channel, IConfiguration configuration)
        {
            _channel = channel;
            _configuration = configuration;
        }

        public override void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey, IBasicProperties properties, ReadOnlyMemory<byte> body)
        {
            Console.WriteLine($"Consuming Message");
            Console.WriteLine(string.Concat("Message received from the exchange ", exchange));
            Console.WriteLine(string.Concat("Consumer tag: ", consumerTag));
            Console.WriteLine(string.Concat("Delivery tag: ", deliveryTag));
            Console.WriteLine(string.Concat("Routing tag: ", routingKey));

            var msgBody = body.ToArray();
            var message = Encoding.UTF8.GetString(msgBody);
            Console.WriteLine(string.Concat("Message: ", message));

            var apiBaseUrl = _configuration.GetSection("CartApiUrl").Value;
            var apiKey = _configuration.GetSection("CartApiKey").Value;

            using (HttpClient client = new())
            {
                client.BaseAddress = new Uri(apiBaseUrl);
                client.DefaultRequestHeaders.Add("X-CarsUnlimited-CartApiKey", apiKey);
                
                var msg = JsonSerializer.Deserialize<Dictionary<string, string>>(message);
                var sId = msg.Values.FirstOrDefault();

                var completeCartTask = client.PostAsync("complete-cart", new StringContent(sId)).GetAwaiter().GetResult();

                if (completeCartTask != null)
                {
                    Console.WriteLine(completeCartTask.ToString());
                }
            }

            _channel.BasicAck(deliveryTag, false);
        }

    }
}
