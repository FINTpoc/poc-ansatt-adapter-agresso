using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AnsattAdapterAgresso.AgressoController;
using Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace AnsattAdapterAgresso
{
    public class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "152.93.120.130", Port = 5672, UserName = "guest", Password = "guest", RequestedHeartbeat = 60 };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel()) 
            {
                //channel.QueueDeclare(queue: "vaf-inn",
                //                 durable: false,
                //                 exclusive: false,
                //                 autoDelete: false,
                //                 arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += Consumer_Received;
                consumer.Shutdown += Consumer_Shutdown;
                channel.BasicConsume("vaf-inn", true, consumer);
                
                //var body = Encoding.UTF8.GetString(test.Body);

                //var json = JsonParser.Deserialize(body);
                //// json.Id, json.Identifikatortype

                //var a = new AnsattAgressoController().HentRessurs(json.Id);
                //var svar = @"{ ""navn"": """ + a.FirstName + " " + a.Surname + @""" }";

                //channel.BasicPublish(exchange: "",
                //     routingKey: test.BasicProperties.ReplyTo,
                //     basicProperties: null,
                //     body: Encoding.UTF8.GetBytes(svar));

                //Console.WriteLine(body);
                Console.WriteLine(@"Venter på meldinger... Trykk 'Enter' for å avbryte");
                Console.ReadLine();

                if (consumer.ShutdownReason != null)
                {
                    Console.WriteLine("Årsak til shutdown: " + consumer.ShutdownReason);
                    Console.ReadLine();
                }
            }
        }

        private static void Consumer_Received(object sender, BasicDeliverEventArgs melding)
        {
            var body = melding.Body;
            var meldingsinnhold = Encoding.UTF8.GetString(body);
            Console.WriteLine(" [x] Received {0}", meldingsinnhold);
            SendTilbakemeldingTilAnsattFelleskomponent(melding);
        }

        private static void Consumer_Shutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("Listener shutdown!");
        }

        private static void SendTilbakemeldingTilAnsattFelleskomponent(BasicDeliverEventArgs message)
        {
            var factory = new ConnectionFactory() { HostName = "152.93.120.130", Port = 5672, UserName = "guest", Password = "guest" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var body = Encoding.UTF8.GetString(message.Body);

                var json = JsonParser.Deserialize(body);

                var a = new AnsattAgressoController().HentRessurs(json.Id);
                var svar = @"{ ""navn"": """ + a.FirstName + " " + a.Surname + @""" }";

                channel.BasicPublish(exchange: "",
                    routingKey: message.BasicProperties.ReplyTo,
                    basicProperties: null,
                    body: Encoding.UTF8.GetBytes(svar));

            }
        }

    }
}
