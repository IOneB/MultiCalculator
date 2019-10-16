using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestServer.Models.Tokens;
using System;

namespace RestServerTests.Tokens
{
    [TestClass]
    public class UnknownTokenTests
    {
        [TestMethod]
        public void CreateTest()
        {
            //Arrange
            Token token;
            //Act
            token = Token.CreateToken("_");
            //Assert
            Assert.IsInstanceOfType(token, typeof(UnknownToken));
        }

        [TestMethod]
        public void CreateRPNTest()
        {
            //Arrange
            Token token;
            //Act
            token = Token.CreateToken("_");
            //Assert
            Assert.ThrowsException<NotImplementedException>(() => token.CreateRPN(null, null));
        }
    }
}
