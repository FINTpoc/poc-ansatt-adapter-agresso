using System;
using System.Collections.Generic;
using System.Configuration;
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
            var queueName = ConfigurationManager.AppSettings["AnsattKomponentQueue"];

            var komponent = new AnsattKomponentController();
            komponent.GetMessagesBinding(queueName, BehandleMottattMelding);
            
            Console.WriteLine(@"Lytter etter meldinger på køen: {0} ... ", queueName);
            
        }

        private static void BehandleMottattMelding(object sender, BasicDeliverEventArgs melding)
        {
            var meldingsinnhold = Encoding.UTF8.GetString(melding.Body);
            var motattEvent = JsonConvert.DeserializeObject<Event>(meldingsinnhold);
            var replyTo = melding.BasicProperties.ReplyTo;
            Console.WriteLine();
            Console.WriteLine("Mottatt melding av typen: {0}", motattEvent.verb);
            Console.WriteLine($@" Meldingsinnhold: {meldingsinnhold}");
            switch (motattEvent.verb)
            {
                case "getEmployee":
                    SendTilbakemeldingGetEmployee(motattEvent, replyTo);
                    break;
                case "getEmployees":
                    SendTilbakemeldingGetEmployees(motattEvent, replyTo);
                    break;
                case "updateEmployee":
                    SendTilbakemeldingUpdateEmployee(motattEvent, replyTo);
                    break;
            }

            
        }

        private static void SendTilbakemeldingGetEmployee(Event requestEvent, string replyTo)
        {
            var identifikator = JsonConvert.DeserializeObject<Identifikator>(requestEvent.data.First().ToString());
            var ressursnummer = identifikator.identifikatorverdi;
            var ressurs = new AnsattRessursController().HentRessurs(ressursnummer);
            var ansatt = Mapping.AgressoRessursTilFkAnsatt(ressurs);
            var responseEventJson = LagResponseEvent(requestEvent, new object[] {ansatt});
            Console.WriteLine($@" Sendt melding: { responseEventJson }");
            SendMelding(replyTo, responseEventJson);
        }

        private static void SendTilbakemeldingUpdateEmployee(Event requestEvent, string replyTo)
        {
            var motattAnsatt = JsonConvert.DeserializeObject<Ansatt>(requestEvent.data.First().ToString());
            var ressursnummer = FinnRessursnummer(motattAnsatt.identifikatorer);
            var epostadresse = motattAnsatt.kontaktinformasjon.epostadresse;
            new AnsattRessursController().OppdaterEpostTilRessurs(ressursnummer, epostadresse);
            var responseEventJson = LagResponseEvent(requestEvent, new object[] { new EventResponse(){ status = "OK" } });
            SendMelding(replyTo, responseEventJson);
        }

        private static void SendTilbakemeldingGetEmployees(Event requestEvent, string replyTo)
        {
            var ansatte = new AnsattRessursController().HentRessurser("915*");
            var ressurser = ansatte.Select(a => (object)Mapping.AgressoRessursTilFkAnsatt(a)).ToArray();
            var responseEventJson = LagResponseEvent(requestEvent, ressurser);
            Console.WriteLine($@"Sender svar på GetEmployees. Antall ansatte: {ressurser.Count()}");
            SendMelding(replyTo, responseEventJson);
        }

        private static string FinnRessursnummer(Identifikator[] identifikator)
        {
            return identifikator.First(i => i.identifikatortype == "ressursnummer").identifikatorverdi;
        }

        private static string LagResponseEvent(Event requestEvent, object[] data)
        {
            var responseEvent = requestEvent;
            responseEvent.type = type.RESPONSE;
            responseEvent.data = data;
            var responseEventJson = JsonConvert.SerializeObject(responseEvent);
            return responseEventJson;
        }

        private static void SendMelding(string replyTo, string responseEvent)
        {
            using (var komponent = new AnsattKomponentController())
            {
                komponent.SendMessage(responseEvent, replyTo);
            }
        }
    }
}
