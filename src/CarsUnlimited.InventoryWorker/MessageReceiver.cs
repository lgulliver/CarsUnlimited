using RabbitMQ.Client;
using System;
using System.Net.Http;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace CarsUnlimited.InventoryWorker
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

            var apiBaseUrl = _configuration.GetSection("InventoryApiUrl").Value;
            var apiKey = _configuration.GetSection("InventoryApiKey").Value;

            using (HttpClient client = new())
            {
                client.BaseAddress = new Uri(apiBaseUrl);
                client.DefaultRequestHeaders.Add("X-CarsUnlimited-InventoryApiKey", apiKey);

                var completeCartTask = client.PutAsync("update-stock", new StringContent(message, Encoding.UTF8, "application/json")).GetAwaiter().GetResult();

                if (completeCartTask != null)
                {
                    Console.WriteLine(completeCartTask.ToString());
                }
            }

            _channel.BasicAck(deliveryTag, false);
        }

    }
}
