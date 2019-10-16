using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RestServer.Models;
using RestServer.Services.Implementations;
using RestServer.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestServerTests.ServicesTests
{
    [TestClass]
    public class FormulaCalculatorTests
    {
        private IFormulaCalculator calculator;

        [TestInitialize]
        public void Init()
        {
            var logger = new Mock<ILog>();
            logger.Setup(log => log.Error(It.IsAny<string>()));
            var discriminator = new StandartTokenDiscriminator(logger.Object);
            calculator = new StandartCalculator(discriminator);
        }

        [TestMethod]
        public void CalculateTest()
        {
            //Arrange
            var formula = new ServerFormula() { Name = "a", FormulaString = "2 + 2" };
            //Act
            var success = calculator.Calculate(formula, new List<ServerFormula>());
            //Assert
            Assert.IsTrue(success);
        }

        [TestMethod]
        public void Calculate_WithNotEnoughRequirements()
        {
            //Arrange
            var formula = new ServerFormula() { Name = "a", FormulaString = "2 + b" };
            //Act
            var success = calculator.Calculate(formula, new List<ServerFormula>());
            //Assert
            Assert.IsFalse(success);
        }

    }
}
