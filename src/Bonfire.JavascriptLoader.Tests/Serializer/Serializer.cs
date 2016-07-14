using System;
using Bonfire.JavascriptLoader.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Bonfire.JavascriptLoader.Tests.Serializer
{
    [TestClass]
    public class Serializer
    {
        [TestMethod]
        public void TestCamelCaseConversion()
        {
            string name = "test";
            TestModel props = new TestModel {UpperCaseTest = "test data"};
            string id = "testId";
            bool renderServerSide = true;

            JavascriptComponent c = new JavascriptComponent(null, name, props, id, renderServerSide);
            Assert.IsTrue(c.SerializedProps.Contains("success"));
        }
    }

    public class TestModel
    {
        [JsonProperty(PropertyName = "success")]
        public string UpperCaseTest { get; set; }
    }
}
