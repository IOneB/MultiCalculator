using ClientsLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ClientLibraryTests
{
    [TestClass]
    public class ClientFactoryTests
    {
        private class TestClicent : ICalculatorClient
        {
            public string ServerAddress { get; set; }
            public void Dispose() { }
            public async Task<Maybe> RequestAsync<T>(Dictionary<string, string> parameters = null) 
                => await Task.FromResult(new Maybe.Error(""));
        }
        private class OtherTestClient : ICalculatorClient
        {
            public string ServerAddress { get; set; }
            public void Dispose() { }
            public async Task<Maybe> RequestAsync<T>(Dictionary<string, string> parameters = null)
                => await Task.FromResult(new Maybe.Error(""));
        }

        [TestMethod]
        public void GetClientTest()
        {
            //Arrange
            var uri = new Uri(TestData.Url);
            var factory = new ClientFactory(uri);
            //Act
            var client = factory.GetClient<TestClicent>();
            var secondClient = factory.GetClient<TestClicent>();
            var otherClient = factory.GetClient<OtherTestClient>();
            //Assert
            Assert.IsNotNull(client);
            Assert.AreSame(client, secondClient);
            Assert.AreNotSame(client, otherClient);
        }

        [TestMethod]
        public void CreateFactoryNegativeTest()
        {
            //Arrange
            var uri = new Uri("ftp://127.0.0.1");
            //Act
            Action action = () => new ClientFactory(uri);
            //Assert
            Assert.ThrowsException<ArgumentException>(action);
        }

        [TestMethod]
        public void CreateFactoryPositiveTest()
        {
            //Arrange
            var uri = new Uri(TestData.Url);
            //Act
            var factory = new ClientFactory(uri);
            //Assert
            Assert.IsNotNull(factory);
        }
    }
}
