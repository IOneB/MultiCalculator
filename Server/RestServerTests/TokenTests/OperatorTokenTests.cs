using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestServer.Models.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RestServerTests.Tokens
{
    [TestClass]
    public class OperatorTokenTests
    {
        [TestMethod]
        public void CreateRPNTest()
        {
            //Arrange
            Token token = Token.CreateToken("+");
            Stack<Token> stack = new Stack<Token>();
            stack.Push(Token.CreateToken("2"));
            stack.Push(Token.CreateToken("-"));
            stack.Push(Token.CreateToken("/"));
            stack.Push(Token.CreateToken("*"));
            List<Token> container = new List<Token>();
            //Act
            token.CreateRPN(stack, container);
            //Assert
            Assert.AreEqual(3, container.Count);
            Assert.AreSame(token, stack.Pop());
            Assert.AreEqual("2", stack.Pop().Value);
        }
        [TestMethod]
        [DataRow(2, "*", 5, 10)]
        [DataRow(4, "+", 7, 11)]
        [DataRow(6, "-", 8, -2)]
        [DataRow(63, "/", 7, 9)]
        public void CalculateTest(double operand1, string @operator, double operand2, double result)
        {
            //Arrange
            var token = Token.CreateToken(@operator);
            Stack<double> stack = new Stack<double>();
            stack.Push(operand1);
            stack.Push(operand2);
            ITokenCalculator tokenCalculator = token as ITokenCalculator;
            //Act
            tokenCalculator.Calculate(stack);
            //Assert
            Assert.AreEqual(result, stack.Pop());
        }
        [TestMethod]
        public void Calculate_DivideByZeroTest()
        {
            //Arrange
            var token = Token.CreateToken("/");
            Stack<double> stack = new Stack<double>();
            stack.Push(2);
            stack.Push(0);
            ITokenCalculator tokenCalculator = token as ITokenCalculator;
            //Act
            //Assert
            Assert.ThrowsException<DivideByZeroException>(() => tokenCalculator.Calculate(stack));
        }
        [TestMethod]
        [DataRow("+")]
        [DataRow("-")]
        [DataRow("*")]
        [DataRow("/")]
        public void CreateTokenPositiveTest(string value)
        {
            //Arrange
            Token token;
            //Act
            token = Token.CreateToken(value);
            //Assert
            Assert.IsInstanceOfType(token, typeof(OperatorToken));
        }
    }
}
