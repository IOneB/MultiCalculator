using CommonLibrary.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestServer.Models;
using RestServer.Services.Implementations;
using RestServer.Services.Interface;
using System;

namespace RestServerTests.ServicesTests
{
    [TestClass]
    public class FormulaStatusManagerTests
    {
        [TestMethod]
        public void ChangeStateTest()
        {
            //Arrange
            IFormulaStatusManager statusManager = new FormulaStatusManager();
            var formula = new ServerFormula() { Status = FormulaStatus.Success };
            var newStatus = FormulaStatus.Deleted;
            //Act
            var notifier = statusManager.ChangeStatus(formula, newStatus);
            //Assert
            Assert.AreEqual(newStatus, formula.Status);
            Assert.IsNotNull(notifier);
        }
        [TestMethod]
        public void ChangeStatus_NullFormulaTest()
        {
            //Arrange
            IFormulaStatusManager statusManager = new FormulaStatusManager();
            //Act
            //Assert
            Assert.ThrowsException<ArgumentNullException>(() => statusManager.ChangeStatus(null, FormulaStatus.Deleted));

        }
    }
}
