using CommonLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Configuration;

namespace CommonLibraryTests
{
    [TestClass]
    public class ServerConfigTests
    {
        [TestMethod]
        public void CreateConfig_WithoutConfigFile_Test()
        {
            //Arrange
            var serverConfig = CommonConfig.ServerConfig;

            //Act
            var address = serverConfig.Address;
            var port = serverConfig.Port;
            var full = serverConfig.FullAddress;

            //Assert
            Assert.AreEqual("127.0.0.1", address);
            Assert.AreEqual("80", port);
            Assert.AreEqual($"http://{address}:{port}", full);
        }
    }
}
