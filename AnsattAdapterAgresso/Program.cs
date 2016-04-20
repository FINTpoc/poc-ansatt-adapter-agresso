using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AnsattAdapterAgresso.AgressoController;
using AnsattAdapterAgresso.RabbitMqController;
using Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace AnsattAdapterAgresso
{
    public class Program
    {
        static void Main(string[] args)
        {
            var queueName = "test";

            var komponent = new AnsattKomponentController();
            komponent.GetMessagesBinding(queueName, Consumer_Received);

            Console.WriteLine(@"Lytter etter meldinger på køen: {0} ... ", queueName);
            //Console.ReadLine();
        }

        private static void Consumer_Received(object sender, BasicDeliverEventArgs melding)
        {
            var body = melding.Body;
            var meldingsinnhold = Encoding.UTF8.GetString(body);
            Console.WriteLine(" Mottatt melding: {0}", meldingsinnhold);
            //SendTilbakemeldingTilAnsattFelleskomponent(melding);
        }

        private static void SendTilbakemeldingTilAnsattFelleskomponent(BasicDeliverEventArgs message)
        {
            using (var komponent = new AnsattKomponentController())
            {
                var body = Encoding.UTF8.GetString(message.Body);

                var json = JsonParser.Deserialize(body);

                var a = new AnsattRessursController().HentRessurs(json.Id);
                var svar = @"{ ""navn"": """ + a.FirstName + " " + a.Surname + @""" }";

                komponent.SendMessage(Encoding.UTF8.GetBytes(svar), message.BasicProperties.ReplyTo);

            }
        }

    }
}
