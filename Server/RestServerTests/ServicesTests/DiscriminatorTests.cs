using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
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
    public class DiscriminatorTests
    {
        [TestMethod]
        [DataRow("(2+3)", 5)]
        [DataRow("( 0/ 3)", 5)]
        [DataRow(" 0 *3 )", 4)]
        [DataRow("( 0 * 3 ))", 6)]
        [DataRow("((( 13 - 3 + 10) - 19 ) *( 3 - 10,2 )) + ( 45 - 15 ) / 2", 27)]
        public void ConstructTokensTest(string expression, int countToken)
        {
            //Arrange
            var logger = new Mock<ILog>();
            logger.Setup(log => log.Error(It.IsAny<string>()));

            ITokenDiscriminator discriminator = new StandartTokenDiscriminator(logger.Object);
            //Act
            var tokens = discriminator.ConstructTokens(expression);
            //Assert
            Assert.AreEqual(countToken, tokens.Count);
            Assert.AreSame(tokens, discriminator.Tokens);
        }
        [TestMethod]
        public void ConstructEmptyExpression()
        {
            //Arrange
            var logger = new Mock<ILog>();
            logger.Setup(log => log.Error(It.IsAny<string>()));

            ITokenDiscriminator discriminator = new StandartTokenDiscriminator(logger.Object);
            //Act
            var tokens = discriminator.ConstructTokens(null);
            //Assert
            logger.Verify();
            Assert.IsNull(tokens);
        }
    }
}
