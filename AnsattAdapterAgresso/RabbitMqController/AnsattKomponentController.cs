using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace AnsattAdapterAgresso.RabbitMqController
{
    public class AnsattKomponentController : IDisposable
    {
        private ConnectionFactory _factory;
        private IConnection _connection;
        private IModel _channel;

        public AnsattKomponentController()
        {
            _factory = new ConnectionFactory()
            {
                HostName = ConfigurationManager.AppSettings["AnsattKomponentHostName"],
                Port = int.Parse(ConfigurationManager.AppSettings["AnsattKomponentPort"]),
                UserName = ConfigurationManager.AppSettings["AnsattKomponentUserName"],
                Password = ConfigurationManager.AppSettings["AnsattKomponentPassword"],
                RequestedHeartbeat = 60
            };
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public IConnection GetConnection()
        {
            if (_connection != null && _connection.IsOpen)
            {
                return _connection;
            }
            return _factory.CreateConnection();
        }

        public IModel GetChannel()
        {
            return _channel ?? GetConnection().CreateModel();
        }

        public void Dispose()
        {
            _connection.Close();
        }

        public void CreateQueue(string navn)
        {
            GetChannel().QueueDeclare(queue: navn, durable: false, exclusive: false, autoDelete: false, arguments: null);
        }


        public void SendMessage(string message, string queue)
        {
            GetChannel().BasicPublish(exchange: "",
                    routingKey: queue,
                    basicProperties: null,
                    body: Encoding.UTF8.GetBytes(message));
        }

        public void GetMessagesBinding(string queue, EventHandler<BasicDeliverEventArgs> consumerReceived)
        {
            var channel = GetChannel();
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += consumerReceived;
            consumer.Shutdown += consumerShutdown;
            channel.BasicConsume(queue, true, consumer);
        }

        private void consumerShutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("Listener shutdown!");
        }
    }
}
