using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using AnsattAdapterAgresso;
using AnsattAdapterAgresso.AgressoController;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AnsattAdapterAgressoTests
{
    [TestClass]
    public class AnsattAgressoControllerTest
    {
        [TestMethod, TestCategory("Integration test")]
        public void HentAnsattTest()
        {
            var ansattAgressoController = new AnsattRessursController();
            var x = ansattAgressoController.HentRessurs("91526");

            Assert.IsTrue(x != null);
            Assert.AreEqual("Ole Anders", x.FirstName);
        }

        [TestMethod, Ignore, TestCategory("Integration test"), TestCategory("Slow > 1 min")]
        public void HentAlleAnsatteTest()
        {
            var ansattAgressoController = new AnsattRessursController();
            var x = ansattAgressoController.HentRessurser("*");

            Assert.IsTrue(x != null);
            Assert.AreEqual(3886, x.Length);
        }

        [TestMethod, TestCategory("Integration test")]
        public void OppdaterEpostTilRessursTest()
        {
            var ansattAgressoController = new AnsattRessursController();
            ansattAgressoController.OppdaterEpostTilRessurs("91526", "oleanders.eidjord@vaf.no");

            var ressurs = ansattAgressoController.HentRessurs("91526");
            Assert.AreEqual("oleanders.eidjord@vaf.no", ressurs.Addresses.First().EMailList.First());
        }
    }
}