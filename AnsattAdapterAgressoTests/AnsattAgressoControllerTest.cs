using System;
using System.Linq;
using AnsattAdapterAgresso;
using AnsattAdapterAgresso.AgressoController;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AnsattAdapterAgressoTests
{
    [TestClass]
    public class AnsattAgressoControllerTest
    {
        [TestMethod]
        public void HentAnsattTest()
        {
            var ansattAgressoController = new AnsattAgressoController();
            var x = ansattAgressoController.HentRessurs("91526");

            Assert.IsTrue(x != null);
            Assert.AreEqual("Ole Anders", x.FirstName);
        }

        [TestMethod, Ignore, TestCategory("1 min")]
        public void HentAlleAnsatteTest()
        {
            var ansattAgressoController = new AnsattAgressoController();
            var x = ansattAgressoController.HentRessurser("*");

            Assert.IsTrue(x != null);
            Assert.AreEqual(3886, x.Length);
        }
    }
}