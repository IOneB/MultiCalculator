using ClientsLibrary.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace ClientLibraryTests
{
    [TestClass]
    public class JsonSerializerTests
    {
        [TestMethod]
        public void SerializeTest()
        {
            //Arrange
            ISerializer jsonSerializer = new JsonSerializer();
            Dictionary<string, object> serializedParameters = new Dictionary<string, object>
            {
                [TestData.name] = "Ivan",
                [TestData.value] = 50
            };
            var expected = TestData.jsonString;

            //Act
            var actual = jsonSerializer.Serialize(serializedParameters);

            //Assert
            Assert.AreEqual(expected, actual, "Полученный сериализованный объект не соответсвует ожиданиям");
        }

        [TestMethod]
        public void SerializeNullTest()
        {
            //Arrange
            ISerializer jsonSerializer = new JsonSerializer();
            object serializedObject = null;
            string expected = "null";

            //Act
            var actual = jsonSerializer.Serialize(serializedObject);

            //Assert
            Assert.AreEqual(expected, actual, "Полученный сериализованный объект не соответсвует ожиданиям");
        }

        [TestMethod]
        public void DeserializeTest()
        {
            //Arrange
            ISerializer jsonSerializer = new JsonSerializer();
            string target = TestData.jsonString;

            //Act
            var actual = jsonSerializer.Deserialize<Dictionary<string,object>>(target);

            //Assert
            Assert.IsNotNull(actual);

            Assert.IsTrue(actual.ContainsKey(TestData.name));
            Assert.IsTrue(actual.ContainsKey(TestData.value));

            Assert.AreEqual("Ivan", actual[TestData.name]);
            Assert.AreEqual(50, Convert.ToInt32(actual[TestData.value]));
        }

        [TestMethod]
        public void DeserializeNullTest()
        {
            //Arrange
            ISerializer jsonSerializer = new JsonSerializer();
            string target = "null";

            //Act
            var actual = jsonSerializer.Deserialize<Dictionary<string, object>>(target);

            //Assert
            Assert.IsNull(actual);
        }
    }
}
