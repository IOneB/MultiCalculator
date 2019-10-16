using ClientsLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MockHttpServer;
using System.Collections.Generic;
using System.Threading;

namespace ClientLibraryTests
{
    [TestClass]
    public class InputClientTests
    {
        [TestMethod]
        public void RequestAsyncPositiveTest()
        {
            //Arrange
            var requestHandlers = new List<MockHttpHandler>()
            {
                new MockHttpHandler(TestData.ApiUrl, "POST", (req, rsp, prm) => rsp.Content("null")),
            };

            using (var mockServer = new MockServer(80, requestHandlers))
            using (var client = new InputClient.InputClient { ServerAddress = TestData.Url })
            {
                //Act
                var result = client.RequestAsync<object>(TestData.Parameters).Result;
                //Assert
                Assert.IsInstanceOfType(result, typeof(Maybe.Result<object>));
            }
        }
    }
}
