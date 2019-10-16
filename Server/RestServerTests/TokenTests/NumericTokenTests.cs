using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestServer.Models.Tokens;
using System.Collections.Generic;
using System.Linq;

namespace RestServerTests.Tokens
{
    [TestClass]
    public class NumericTokenTests
    {
        [TestMethod]
        public void CreateRPNTest()
        {
            //Arrange
            Token token = Token.CreateToken("1");
            Stack<Token> stack = new Stack<Token>();
            List<Token> container = new List<Token>();
            //Act
            token.CreateRPN(stack, container);
            //Assert
            Assert.AreSame(token, container.Single());
        }
        [TestMethod]
        public void CalculateTest()
        {
            //Arrange
            var token = Token.CreateToken("1");
            Stack<double> stack = new Stack<double>();
            ITokenCalculator tokenCalculator = token as ITokenCalculator;
            //Act
            tokenCalculator.Calculate(stack);
            //Assert
            Assert.AreEqual(token.Value, stack.Pop().ToString());
        }
        [TestMethod]
        [DataRow("12")]
        [DataRow("2,5")]
        public void CreateTokenPositiveTest(string value)
        {
            //Arrange
            Token token;
            //Act
            token = Token.CreateToken(value);
            //Assert
            Assert.IsInstanceOfType(token, typeof(NumericToken));
        }
    }
}
