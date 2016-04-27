using System;
using System.IO;
using System.Linq;
using System.Reflection;
using KellermanSoftware.CompareNetObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace AnsattAdapterAgressoTests.Event
{
    [TestClass]
    public class EventTests
    {
        [TestMethod]
        public void CreateGetEmployeeResponseTest()
        {
            var getEmployee = new global::Event()
            {
                data = null,
                verb = "getEmployee",
                type = type.RESPONSE,
                time = DateTime.Now,
                id = "123",
                orgId = ""
            };
            var getEmployeeJson = JsonConvert.SerializeObject(getEmployee);

            var getEmployeeResponse = JsonConvert.DeserializeObject<global::Event>(getEmployeeJson);
            var result = new CompareLogic().Compare(getEmployee, getEmployeeResponse);
            Assert.IsTrue(result.AreEqual);
        }

        [TestMethod]
        public void CreateGetEmployeeResponseMedAnsattTest()
        {
            var ansatt = new Ansatt()
            {
                navn = new Personnavn() {etternavn = "Olsen", fornavn = "Ole"}
            };
            var getEmployee = new global::Event()
            {
                data = new object[] { ansatt },
                verb = "getEmployee",
                type = type.RESPONSE,
                time = DateTime.Now,
                id = "123",
                orgId = ""
            };
            var getEmployeeJson = JsonConvert.SerializeObject(getEmployee);

            var getEmployeeResponse = JsonConvert.DeserializeObject<global::Event>(getEmployeeJson);
            var getEmployeeResponseJson = JsonConvert.SerializeObject(getEmployeeResponse);
            
            Assert.IsTrue(getEmployeeJson.Equals(getEmployeeResponseJson));
        }

        [TestMethod]
        public void ValidateUpdateEmployeeJsonGeneratedFromJavaTest()
        {
            var javaJson = File.ReadAllText(@"../../Event/updateEmployee.json");
            var ev = JsonConvert.DeserializeObject<global::Event>(javaJson);
            var ansatt = JsonConvert.DeserializeObject<Ansatt>(ev.data.First().ToString());

            Assert.AreEqual("updateEmployee", ev.verb);
            Assert.AreEqual("test@test.com", ansatt.kontaktinformasjon.epostadresse);
            Assert.AreEqual("Pål", ansatt.navn.fornavn);
        }

        [TestMethod]
        public void ValidateEventJsonTest()
        {
            var eventJson = @"{ ""id"":""1463ab18-0a89-47c9-8959-999487de5751"",""verb"":""getEmployees"",""type"":""REQUEST"",""time"":""2016-04-17 13:12:11"",""orgId"":""vaf.no"",""data"":[] }";
            var eventObject = JsonConvert.DeserializeObject<global::Event>(eventJson);
            Assert.AreEqual(new DateTime(2016, 04, 17, 13, 12, 11), eventObject.time);
        }
        
    }
}
