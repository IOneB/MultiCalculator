using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestServer.Models;
using RestServer.Models.Tokens;
using System.Collections.Generic;
using System.Linq;
using System;

namespace RestServerTests.Tokens
{
    [TestClass]
    public class TokenCollectionTests
    {
        [TestMethod]
        public void AddManyTest()
        {
            //Arrange
            var stringTokens = "( 2 + 3 ) / 10".Split();
            TokenCollection tokens = new TokenCollection();
            const int summaryTokenCount = 7;
            const int brokenTokensCount = 0;
            //Act
            tokens.AddMany(stringTokens);
            //Assert
            Assert.AreEqual(summaryTokenCount, tokens.Count);
            Assert.AreEqual(brokenTokensCount, tokens.Where(token => token is UnknownToken).Count());
        }
        [TestMethod]
        public void AddMany_ManyIsNotValidTest()
        {
            //Arrange
            var stringTokens = "( 2 + 3)/ 10".Split();
            TokenCollection tokens = new TokenCollection();
            const int summaryTokenCount = 5;
            const int brokenTokensCount = 1;
            //Act
            tokens.AddMany(stringTokens);
            //Assert
            Assert.AreEqual(summaryTokenCount, tokens.Count);
            Assert.AreEqual(brokenTokensCount, tokens.Where(token => token is UnknownToken).Count());
        }

        [TestMethod]
        [DataRow("( 10 + 13 )", 23)]
        [DataRow("( 33 ) + 2 * 2", 37)]
        [DataRow("5,8 - 2 * 3 / ( 33 - 31 )", 2.8)]
        [DataRow("( ( ( 13 - 3 + 10 ) - 19 ) * ( 3 - 10,2 ) ) + ( 45 - 15 ) / 2", 7.8)]
        public void CalcuateTest(string expression, double expectedResult)
        {
            //Arrange
            var stringTokens = expression.Split();
            TokenCollection tokens = new TokenCollection();
            List<ServerFormula> formulas = new List<ServerFormula>();
            //Act
            tokens.AddMany(stringTokens);
            tokens.CreateRPN();
            var result = tokens.Calculate(formulas);
            //Assert
            Assert.AreEqual(expectedResult, result, 0.01);
        }
        [TestMethod]
        public void Calcuate_WithCorrectListOfFormulaTest()
        {
            //Arrange
            var stringTokens = "2 + a * 10".Split();
            TokenCollection tokens = new TokenCollection();
            List<ServerFormula> formulas = new List<ServerFormula>()
            {
                new ServerFormula { Name = "a", Value = 13}
            };
            //Act
            tokens.AddMany(stringTokens);
            tokens.CreateRPN();
            var result = tokens.Calculate(formulas);
            //Assert
            Assert.AreEqual(132, result, 0.01);
        }
        [TestMethod]
        public void Calcuate_WithIncorrectListOfFormulaTest()
        {
            //Arrange
            var stringTokens = "2 + a * c".Split();
            TokenCollection tokens = new TokenCollection();
            List<ServerFormula> formulas = new List<ServerFormula>()
            {
                new ServerFormula { Name = "a", Value = 13}
            };
            //Act
            tokens.AddMany(stringTokens);
            tokens.CreateRPN();
            //Assert
            Assert.ThrowsException<ArgumentException>(() => tokens.Calculate(formulas));
        }
        [TestMethod]
        public void Calculate_WithoutRPN_ReturnExceptionTest()
        {
            //Arrange
            var stringTokens = "2 + 2".Split();
            TokenCollection tokens = new TokenCollection();
            List<ServerFormula> formulas = new List<ServerFormula>();
            //Act
            tokens.AddMany(stringTokens);
            //Assert
            Assert.ThrowsException<InvalidOperationException>(() => tokens.Calculate(formulas));
        }
        [TestMethod]
        public void Calculate_IncorrectExpressionTest()
        {
            //Arrange
            var stringTokens = "2 + _2".Split();
            TokenCollection tokens = new TokenCollection();
            //Act
            tokens.AddMany(stringTokens);
            //Assert
            Assert.ThrowsException<NotImplementedException>(() => tokens.CreateRPN());
        }

        [TestMethod]
        public void Calculate_DivideByZeroTest()
        {
            //Arrange
            var stringTokens = "2 / 0".Split();
            TokenCollection tokens = new TokenCollection();
            List<ServerFormula> formulas = new List<ServerFormula>();
            //Act
            tokens.AddMany(stringTokens);
            tokens.CreateRPN();
            //Assert
            Assert.ThrowsException<DivideByZeroException>(() => tokens.Calculate(formulas));
        }

    }
}
