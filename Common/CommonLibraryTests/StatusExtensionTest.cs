using CommonLibrary.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLibraryTests
{
    [TestClass]
    public class StatusExtensionTest
    {
        [TestMethod]
        public void GetDisplayNameTest()
        {
            //Arrange
            var status = FormulaStatus.Success;

            //Act
            var actual = status.GetDisplayName();

            //Assert
            Assert.AreEqual("Завершено", actual);
        }
    }
}
