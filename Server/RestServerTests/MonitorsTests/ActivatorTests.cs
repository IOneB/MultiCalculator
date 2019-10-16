using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestServer.Models.Monitors.Activators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RestServerTests.MonitorsTests
{
    [TestClass]
    public class MonitorActivatorTests
    {
        [TestMethod]
        public void ActivateTest()
        {
            //Arrange
            MonitorActivator activator;
            var dueTime = 100;
            bool beforeActivateValue;
            //Act
            activator = new MonitorActivator(dueTime, false, 10_000);
            beforeActivateValue = activator.IsActive.Value;
            Thread.Sleep(200);
            //Assert
            Assert.IsFalse(beforeActivateValue);
            Assert.IsTrue(activator.IsActive.Value);
        }
    }
}
