using System;
using System.Text;
using System.Threading;
using AnsattAdapterAgresso.RabbitMqController;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RabbitMQ.Client;

namespace AnsattAdapterAgressoTests
{
    [TestClass]
    public class AnsattKomponentControllerTests
    {
        private readonly AnsattKomponentController _komponent;
        public AnsattKomponentControllerTests()
        {
            _komponent = new AnsattKomponentController();
        }

        [TestMethod]
        public void GetConnectionChannelTest()
        {
            var channel = _komponent.GetChannel();
            Assert.IsTrue(channel.IsOpen);
        }

        [TestMethod]
        public void CreateQueueTest()
        {
            _komponent.CreateQueue("test");

            //var channel = _komponent.GetChannel(); Hvordan finne hvilken kø som er opprettet?
        }

        [TestMethod]
        public void SendMessageTest()
        {
            const string konavn = "integrasjonstest";
            const string testmelding = "testmelding";
            _komponent.CreateQueue(konavn);
            _komponent.SendMessage(testmelding, konavn);
           
            var melding = _komponent.GetChannel().BasicGet(konavn, true);
            Assert.AreEqual(testmelding, Encoding.UTF8.GetString(melding.Body));
        }
    }
}
