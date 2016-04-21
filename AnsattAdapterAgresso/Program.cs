using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AnsattAdapterAgresso.AgressoController;
using AnsattAdapterAgresso.RabbitMqController;
using Newtonsoft.Json;
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

            // JSON TEST: 
            // string ansattObject = JsonConvert.DeserializeObject<Ansatt>(ansattJson)
            // string ansattJson = JsonConvert.SerializeObject(ansattObject)

            // Eksempel: 

            dynamic data = new ExpandoObject(); 
            data.Status = "ok";
            var e = new Event
            {
                id = "123",
                verb = "updateEmployee",
                time = 0,
                type = type.RESPONSE,
                data = new object[] { data }
            };
            var eJson = JsonConvert.SerializeObject(e);
            
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

                dynamic json = JsonConvert.DeserializeObject(body);

                var a = new AnsattRessursController().HentRessurs(json.Id);
                var svar = @"{ ""navn"": """ + a.FirstName + " " + a.Surname + @""" }";

                komponent.SendMessage(Encoding.UTF8.GetBytes(svar), message.BasicProperties.ReplyTo);

            }
        }

    }
}
