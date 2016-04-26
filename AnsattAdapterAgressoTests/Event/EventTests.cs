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
        
    }
}
