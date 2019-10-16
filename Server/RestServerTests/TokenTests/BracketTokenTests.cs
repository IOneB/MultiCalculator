using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestServer.Models.Tokens;
using System.Collections.Generic;

namespace RestServerTests.Tokens
{
    [TestClass]
    public class BracketTokenTests
    {
        [TestMethod]
        public void LeftBracketCreateRPNTest()
        {
            //Arrange
            Token token = Token.CreateToken("(");
            Stack<Token> stack = new Stack<Token>();
            List<Token> container = new List<Token>();
            //Act
            token.CreateRPN(stack, container);
            //Assert
            Assert.AreSame(token, stack.Pop());
        }
        [TestMethod]
        public void RightBracketCreateRPNTest()
        {
            //Arrange
            Token token = Token.CreateToken(")");
            Stack<Token> stack = new Stack<Token>();
            stack.Push(Token.CreateToken("("));
            stack.Push(Token.CreateToken("2"));
            stack.Push(Token.CreateToken("+"));
            stack.Push(Token.CreateToken("3"));
            List<Token> container = new List<Token>();
            const int notBracketTokenCount = 3;
            //Act
            token.CreateRPN(stack, container);
            //Assert
            Assert.AreEqual(0, stack.Count);
            Assert.AreEqual(notBracketTokenCount, container.Count);
        }
        [TestMethod]
        public void CreateLeftBracketTest()
        {
            //Arrange
            Token token;
            //Act
            token = Token.CreateToken("(");
            //Assert
            Assert.IsInstanceOfType(token, typeof(BracketToken));
            Assert.IsTrue((token as BracketToken).IsOpening);
        }
        [TestMethod]
        public void CreateRightBracketTest()
        {
            //Arrange
            Token token;
            //Act
            token = Token.CreateToken(")");
            //Assert
            Assert.IsInstanceOfType(token, typeof(BracketToken));
            Assert.IsTrue((token as BracketToken).IsClosing);
        }
    }
}
