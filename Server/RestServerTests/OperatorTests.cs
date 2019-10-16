using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestServer.Models;

namespace RestServerTests
{
    [TestClass]
    public class OperatorTests
    {
        [TestMethod]
        public void PriorityPositiveTest()
        {
            //Arrange
            var @operator = "+";
            //Act
            var priority = Operator.Priority(@operator);
            //Assert
            Assert.AreEqual(1, priority);
        }

        [TestMethod]
        [DataRow("")]
        [DataRow(null)]
        [DataRow("symbols")]
        public void PriorityNegativeTest(string unknownOperator)
        {
            //Arrange
            string @operator = unknownOperator;
            //Act
            var priority = Operator.Priority(@operator);
            //Assert
            Assert.AreEqual(-1, priority);
        }
    }
}
