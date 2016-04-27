using System;
using System.Linq;
using AnsattAdapterAgresso;
using AnsattAdapterAgresso.AgressoUserAdministrationServiceReference;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AnsattAdapterAgressoTests
{
    [TestClass]
    public class MappingTests
    {
        [TestMethod]
        public void KanMappeFraAgressoRessursTilFkAnsattTest()
        {
            var ressurs = new Resource()
            {
                ResourceId = "12345",
                Surname = "Olsen",
                FirstName = "Ole",
                Name = "Olsen, Ole"
            };
            var ansatt = Mapping.AgressoRessursTilFkAnsatt(ressurs);
            Assert.AreEqual("Ole", ansatt.navn.fornavn);
            Assert.AreEqual("Olsen", ansatt.navn.etternavn);
            Assert.AreEqual("12345", ansatt.identifikatorer.Single(a => a.identifikatortype == "ressursnummer").identifikatorverdi);
        }

        [TestMethod]
        public void KanMappeFraAgressoRessursTilFkAnsattEpostTest()
        {
            var ressurs = new Resource()
            {
                ResourceId = "12345",
                Surname = "Olsen",
                FirstName = "Ole",
                Name = "Olsen, Ole",
                Addresses = new Address[] { new Address() {EMailList = new ArrayOfString() { "test@test.no" } } }
            };
            var ansatt = Mapping.AgressoRessursTilFkAnsatt(ressurs);
            Assert.AreEqual("Ole", ansatt.navn.fornavn);
            Assert.AreEqual("Olsen", ansatt.navn.etternavn);
            Assert.AreEqual("12345", ansatt.identifikatorer.Single(a => a.identifikatortype == "ressursnummer").identifikatorverdi);
            Assert.AreEqual("test@test.no", ansatt.kontaktinformasjon.epostadresse);
        }

    }
}
