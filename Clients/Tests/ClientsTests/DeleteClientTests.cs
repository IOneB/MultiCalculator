using ClientsLibrary;
using InputClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MockHttpServer;
using System.Collections.Generic;

namespace ClientLibraryTests
{
    [TestClass]
    public class DeleteClientTests
    {
        [TestMethod]
        public void RequestAsyncPositiveTest()
        {
            //Arrange
            var requestHandlers = new List<MockHttpHandler>()
            {
                new MockHttpHandler(TestData.ApiUrl, "DELETE", (req, rsp, prm) => rsp.Content("null")),
            };

            using (var mockServer = new MockServer(80, requestHandlers))
            using (var client = new InputClient.DeleteClient { ServerAddress = TestData.Url })
            {
                //Act
                var result = client.RequestAsync<object>(TestData.Parameters).Result;
                //Assert
                Assert.IsInstanceOfType(result, typeof(Maybe.Result<object>));
            }
        }
    }
}
