using Microsoft.VisualStudio.TestTools.UnitTesting;
using MockHttpServer;
using System.Net.Http;
using ClientsLibrary.ClientBase.Utils;
using System.Net;
using System.Collections.Generic;
using System;

namespace ClientLibraryTests
{
    [TestClass]
    public class HttpClientExtensionTests
    {
        [TestMethod]
        public void GetAsyncTest()
        {
            //Arrange
            var requestHandlers = new List<MockHttpHandler>()
            {
                new MockHttpHandler(TestData.queryParameters, "GET", (req, rsp, prm) =>
                {
                    Assert.AreEqual("GET", req.HttpMethod);
                    var expectedUrl = req.Url.ToString();
                    Assert.AreEqual(TestData.Url + TestData.queryParameters, expectedUrl);
                }),
            };

            using (var mockServer = new MockServer(80, requestHandlers))
            using (HttpClient client = new HttpClient())
            {
                //Act
                var response = client.GetAsync(TestData.Url, TestData.queryParameters).Result;
                //Assert
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            }
        }

        [TestMethod]
        public void DeletAsyncTest()
        {
            //Arrange
            var requestHandlers = new List<MockHttpHandler>()
            {
                new MockHttpHandler(TestData.queryParameters, "DELETE", (req, rsp, prm) =>
                {
                    Assert.AreEqual("DELETE", req.HttpMethod);
                    var expectedUrl = req.Url.ToString();
                    Assert.AreEqual(TestData.Url + TestData.queryParameters, expectedUrl);
                }),
            };

            using (var mockServer = new MockServer(80, requestHandlers))
            using (HttpClient client = new HttpClient())
            {
                //Act
                var response = client.DeleteAsync(TestData.Url, TestData.queryParameters).Result;
                //Assert
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            }
        }
    }
}
