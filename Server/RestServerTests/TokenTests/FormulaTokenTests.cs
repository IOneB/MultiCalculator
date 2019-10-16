using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestServer.Models.Tokens;
using System.Collections.Generic;
using System.Linq;

namespace RestServerTests.Tokens
{
    [TestClass]
    public class FormulaTokenTests
    {
        [TestMethod]
        public void CreateRPNTest()
        {
            //Arrange
            Token token = Token.CreateToken("formula");
            Stack<Token> stack = new Stack<Token>();
            List<Token> container = new List<Token>();
            //Act
            token.CreateRPN(stack, container);
            //Assert
            Assert.AreSame(token, container.Single());
        }
        [TestMethod]
        public void CreateTokenPositiveTest()
        {
            //Arrange
            Token token;
            //Act
            token = Token.CreateToken("ab13c");
            //Assert
            Assert.IsInstanceOfType(token, typeof(FormulaToken));
        }
        [TestMethod]
        [DataRow("2ab13c")]
        [DataRow("a_s")]
        public void CreateTokenNegativeTest(string value)
        {
            //Arrange
            Token token;
            //Act
            token = Token.CreateToken(value);
            //Assert
            Assert.IsInstanceOfType(token, typeof(UnknownToken));
        }
    }
}
