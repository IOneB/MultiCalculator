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
    public class ValidatorTests
    {
        private StandartValidator validator;

        [TestInitialize]
        public void Init()
        {
            var logger = new Mock<ILog>();
            logger.Setup(log => log.Error(It.IsAny<string>()));
            var discriminator = new StandartTokenDiscriminator(logger.Object);
            validator = new StandartValidator(discriminator);
        }

        [TestMethod]
        public void ValidateSuccessTest()
        {
            //Arrange
            var formula = new ServerFormula() { Name = "a", FormulaString = "(12 + 10) * (c)" };
            //Act
            var isValid = validator.Validate(formula);
            //Assert
            Assert.IsTrue(isValid);
        }

        [TestMethod]
        public void ValidateRecursiveReferenceTest()
        {
            //Arrange
            var formula = new ServerFormula() { Name = "a", FormulaString = "(12 + 10) * (a)" };
            //Act
            var isValid = validator.Validate(formula);
            //Assert
            Assert.IsFalse(isValid);
        }

        [TestMethod]
        public void Validate_NotValidBracketTest()
        {
            //Arrange
            var formula = new ServerFormula() { Name = "a", FormulaString = "(12 + 10)) * (5)" };
            //Act
            var isValid = validator.Validate(formula);
            //Assert
            Assert.IsFalse(isValid);
        }

        [TestMethod]
        public void Validate_NotValidSequenceTest()
        {
            //Arrange
            var formula = new ServerFormula() { Name = "a", FormulaString = "12 + - 10 * (5)" };
            //Act
            var isValid = validator.Validate(formula);
            //Assert
            Assert.IsFalse(isValid);
        }
        [TestMethod]
        public void Validate_WithNullFormulaTest()
        {
            //Arrange
            //Act
            var isValid = validator.Validate(null);
            //Assert
            Assert.IsFalse(isValid);
        }
        [TestMethod]
        public void Validate_WithNullStringTest()
        {
            //Arrange
            var formula = new ServerFormula();
            //Act
            var isValid = validator.Validate(formula);
            //Assert
            Assert.IsFalse(isValid);
        }
        [TestMethod]
        public void Validate_WithUnknownTokenTest()
        {
            //Arrange
            var formula = new ServerFormula() { Name = "a", FormulaString = "12 + - 10 * (5) + _a" };
            //Act
            var isValid = validator.Validate(formula);
            //Assert
            Assert.IsFalse(isValid);
        }
    }
}
