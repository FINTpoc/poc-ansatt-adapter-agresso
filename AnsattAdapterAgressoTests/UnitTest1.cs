using System;
using System.Linq;
using AnsattAdapterAgresso;
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
            var x = ansattAgressoController.HentAnsatt("91526");

            Assert.IsTrue(x != null);
            Assert.AreEqual("Ole Anders", x.FirstName);
        }
    }
}