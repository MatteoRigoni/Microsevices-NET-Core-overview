using System;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using platformservice.Dtos;
using RabbitMQ.Client;

namespace platformservice.AsyncDataServices
{
    public class MessageBusClient : IMessageBusClient
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public MessageBusClient(IConfiguration configuration)
        {
            _configuration = configuration;

            var factory = new ConnectionFactory(){
                HostName = _configuration["RabbitMqHost"],
                Port = int.Parse(_configuration["RabbitMqPort"]),
                UserName = _configuration["RabbitUsername"],
                Password = _configuration["RabbitPassword"],
            };

            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);

                _connection.ConnectionShutdown += RabbitMq_ConnectionShutDown;

                Console.WriteLine("--> Connected to message bus");
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("--> Init MessageBusClient: " + ex.Message);
            }
        }

        private void RabbitMq_ConnectionShutDown (object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("--> RabbitMq connection shut down");
        }

        public void PublishNewPlatform(PlatformPublishedDto platformPublished)
        {
            var message = JsonSerializer.Serialize(platformPublished);

            if (_connection.IsOpen)
            {
                Console.WriteLine("--> RabbitMq connection open, sending message...");
                SendMessage(message);
            }
            else
            {
                Console.WriteLine("--> RabbitMq connection is closed");
            }
        }

        private void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(exchange: "trigger", 
                routingKey: "",
                basicProperties: null,
                body:body);

                Console.WriteLine($"We have sent message {message}");
        }

        public void Dispose()
        {
            if (_channel.IsOpen)
            {
                _channel.Close();
            }
            if (_connection.IsOpen)
            {
                _connection.Close();
            }
            
            Console.WriteLine("MessageBus disposed");
        }
    }
}