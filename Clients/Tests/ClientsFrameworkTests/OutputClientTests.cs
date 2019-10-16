using Microsoft.VisualStudio.TestTools.UnitTesting;
using MockHttpServer;
using System.Collections.Generic;
using ClientsLibrary;
using System;
using OutputClient;

namespace ClientLibraryTests
{
    [TestClass]
    public class OutputClientTests
    {
        [TestMethod]
        public void RequestAsyncPositiveTest()
        {
            //Arrange
            var requestHandlers = new List<MockHttpHandler>()
            {
                new MockHttpHandler(TestData.ApiUrl, "GET", (req, rsp, prm) => rsp.Content("null")),
            };

            using (var mockServer = new MockServer(80, requestHandlers))
            using (var client = new OutputClient.OutputClient { ServerAddress = TestData.Url })
            {
                //Act
                var result = client.RequestAsync<object>(TestData.Parameters).Result;
                //Assert
                Assert.IsInstanceOfType(result, typeof(Maybe.Result<object>));
            }
        }

        [TestMethod]
        public void RequestAsync_Positive_WithContentParsingTest()
        {
            //Arrange
            var requestHandlers = new List<MockHttpHandler>()
            {
                new MockHttpHandler(TestData.ApiUrl, "GET", (req, rsp, prm) => rsp.Content(TestData.jsonString)),
            };

            using (var mockServer = new MockServer(80, requestHandlers))
            using (var client = new OutputClient.OutputClient { ServerAddress = TestData.Url })
            {
                //Act
                var result = client.RequestAsync<Dictionary<string, object>>(TestData.Parameters).Result;
                var actual = (result as Maybe.Result<Dictionary<string, object>>)?.Value;
                //Assert
                Assert.IsNotNull(actual);

                Assert.IsTrue(actual.ContainsKey(TestData.name));
                Assert.IsTrue(actual.ContainsKey(TestData.value));

                Assert.AreEqual("Ivan", actual[TestData.name]);
                Assert.AreEqual(50, Convert.ToInt32(actual[TestData.value]));
            }
        }

        [TestMethod]
        public void RequestAsyncNegativeTest()
        {
            //Arrange
            var requestHandlers = new List<MockHttpHandler>()
            {
                new MockHttpHandler(TestData.ApiUrl, "GET", (req, rsp, prm) => rsp.StatusCode = 400),
            };

            using (var mockServer = new MockServer(80, requestHandlers))
            using (var client = new OutputClient.OutputClient { ServerAddress = TestData.Url })
            {
                //Act
                var result = client.RequestAsync<object>(TestData.Parameters).Result;
                //Assert
                Assert.IsInstanceOfType(result, typeof(Maybe.Error));
            }
        }

        [TestMethod]
        public void RequestAsyncNegative_NoConnectionTest()
        {
            //Arrange
            using (var client = new OutputClient.OutputClient { ServerAddress = TestData.Url })
            {
                //Act
                var result = client.RequestAsync<object>(TestData.Parameters).Result;
                //Assert
                Assert.IsInstanceOfType(result, typeof(Maybe.Error));
            }
        }
    }
}
