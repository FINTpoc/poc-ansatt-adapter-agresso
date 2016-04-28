using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AnsattAdapterAgressoTests.Modell
{
    [TestClass]
    public class AnsattTests
    {
        [TestMethod]
        public void KjonnEnumTest()
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings()
            {
                Converters = new List<JsonConverter>() { new StringEnumConverter() }
            };

            var ansatt = new Ansatt() {kjonn = kjonn.MANN };
            var ansattJson = JsonConvert.SerializeObject(ansatt);
            dynamic ansattListe = JsonConvert.DeserializeObject<dynamic>(ansattJson);
            Assert.AreEqual("MANN", ansattListe.kjonn.Value);
        }
    }
}
