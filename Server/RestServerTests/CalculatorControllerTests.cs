using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RestServer.Controllers;
using RestServer.Models;
using RestServer.Services.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace RestServerTests
{
    [TestClass]
    public class CalculatorControllerTests
    {
        private CalculatorController controller;
        private Mock<IRepository<ServerFormula>> repository;
        private Mock<IFormulaMonitor> monitor;

        [TestInitialize]
        public void InitController()
        {
            repository = new Mock<IRepository<ServerFormula>>();
            monitor = new Mock<IFormulaMonitor>();
        }

        [TestMethod]
        public void GetTest()
        {
            //Arrange
            IEnumerable<ServerFormula> formulas = new List<ServerFormula>
            {
                new ServerFormula(),
                new ServerFormula()
            };
            repository.Setup(r => r.GetAllAsync(null, false, false, false)).Returns(Task.FromResult(formulas));
            controller = new CalculatorController(repository.Object, monitor.Object);
            //Act
            var result = controller.Get().Result as JsonResult<IEnumerable<ServerFormula>>;
            var model = result.Content;
            //Assert
            repository.Verify();
            Assert.IsNotNull(result);
            Assert.AreSame(formulas, model);
        }

        [TestMethod]
        public void GetWithNameTest()
        {
            //Arrange
            const string searchingName = "A";
            var formula = new ServerFormula() { Name = searchingName };
            repository.Setup(r => r.GetByNameAsync(searchingName)).Returns(Task.FromResult(formula));
            controller = new CalculatorController(repository.Object, monitor.Object);
            //Act
            var result = controller.Get(searchingName).Result as JsonResult<ServerFormula>;
            var model = result.Content;
            //Assert
            repository.Verify();
            Assert.IsNotNull(result);
            Assert.AreSame(formula, model);
        }
        [TestMethod]
        public void GetWithName_NotFoundTest()
        {
            //Arrange
            const string searchingName = "A";
            repository.Setup(r => r.GetByNameAsync(searchingName)).Returns(Task.FromResult<ServerFormula>(null));
            controller = new CalculatorController(repository.Object, monitor.Object);
            //Act
            var result = controller.Get(searchingName).Result as NotFoundResult;
            //Assert
            repository.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void GetWithIdTest()
        {
            //Arrange
            const int searchingId = 1;
            var formula = new ServerFormula() { FormulaId = searchingId };
            repository.Setup(r => r.GetByIdAsync(searchingId)).Returns(Task.FromResult(formula));
            controller = new CalculatorController(repository.Object, monitor.Object);
            //Act
            var result = controller.Get(searchingId).Result as JsonResult<ServerFormula>;
            var model = result.Content;
            //Assert
            repository.Verify();
            Assert.IsNotNull(result);
            Assert.AreSame(formula, model);
        }
        [TestMethod]
        public void GetWithId_NotFoundTest()
        {
            //Arrange
            const int searchingId = 1;
            repository.Setup(r => r.GetByIdAsync(searchingId)).Returns(Task.FromResult<ServerFormula>(null));
            controller = new CalculatorController(repository.Object, monitor.Object);
            //Act
            var result = controller.Get(searchingId).Result;
            //Assert
            repository.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void AddTest()
        {
            //Arrange
            ServerFormula formula = new ServerFormula { Name = "A", FormulaString = "5 + 5" };
            repository.Setup(r => r.CreateAsync(formula)).Returns(Task.FromResult(true));
            monitor.Setup(m => m.Created());
            controller = new CalculatorController(repository.Object, monitor.Object);
            //Act
            var result = controller.Post(formula).Result as CreatedNegotiatedContentResult<string>;
            //Assert
            repository.Verify();
            Assert.IsNotNull(result);
            Assert.IsFalse(string.IsNullOrEmpty(result.Content));
        }
        [TestMethod]
        public void Add_ValidationErrorTest()
        {
            //Arrange
            ServerFormula formula = new ServerFormula { FormulaString = null };
            controller = new CalculatorController(repository.Object, monitor.Object);
            //Act
            var result = controller.Post(formula).Result as ExceptionResult;
            //Assert
            Assert.IsNotNull(result);
        }
        [TestMethod]
        public void Add_AlreadyExistsTest()
        {
            //Arrange
            ServerFormula formula = new ServerFormula { Name = "A", FormulaString = "5 + 5" };
            repository.Setup(r => r.GetByNameAsync(It.IsAny<string>())).Returns(Task.FromResult(formula));
            controller = new CalculatorController(repository.Object, monitor.Object);
            //Act
            var result = controller.Post(formula).Result as NegotiatedContentResult<string>;
            //Assert
            repository.Verify();
            Assert.IsNotNull(result);
        }
        [TestMethod]
        public void Add_ErrorSaveTest()
        {
            //Arrange
            ServerFormula formula = new ServerFormula { Name = "A", FormulaString = "5 + 5" };
            repository.Setup(r => r.GetByNameAsync(It.IsAny<string>())).Returns(Task.FromResult<ServerFormula>(null));
            repository.Setup(r => r.CreateAsync(formula)).Returns(Task.FromResult(false));
            controller = new CalculatorController(repository.Object, monitor.Object);
            //Act
            var result = controller.Post(formula).Result as ExceptionResult;
            //Assert
            repository.Verify();
            Assert.IsNotNull(result);
        }
        [TestMethod]
        public void UpdateTest()
        {
            //Arrange
            ServerFormula formula = new ServerFormula { Name = "A", FormulaString = "5 + 5" };
            repository.Setup(r => r.GetByNameAsync("A")).Returns(Task.FromResult(formula));
            repository.Setup(r => r.UpdateAsync(formula)).Returns(Task.FromResult(true));
            monitor.Setup(m => m.Updated());
            controller = new CalculatorController(repository.Object, monitor.Object);
            //Act
            var result = controller.Update(formula).Result as NegotiatedContentResult<string>;
            //Assert
            repository.Verify();
            Assert.IsNotNull(result);
            Assert.IsFalse(string.IsNullOrEmpty(result.Content));
        }
        [TestMethod]
        public void Update_NotFoundTest()
        {
            //Arrange
            ServerFormula formula = new ServerFormula { FormulaString = null };
            controller = new CalculatorController(repository.Object, monitor.Object);
            //Act
            var result = controller.Update(formula).Result as NotFoundResult;
            //Assert
            Assert.IsNotNull(result);
        }
        [TestMethod]
        public void Update_ErrorSaveTest()
        {
            //Arrange
            ServerFormula formula = new ServerFormula { Name = "A", FormulaString = "5 + 5" };
            repository.Setup(r => r.GetByNameAsync("A")).Returns(Task.FromResult(formula));
            repository.Setup(r => r.CreateAsync(formula)).Returns(Task.FromResult(false));
            controller = new CalculatorController(repository.Object, monitor.Object);
            //Act
            var result = controller.Update(formula).Result as ExceptionResult;
            //Assert
            repository.Verify();
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void DeleteTest()
        {
            //Arrange
            ServerFormula formula = new ServerFormula { Name = "A", FormulaString = "5 + 5" };
            repository.Setup(r => r.GetByNameAsync("A")).Returns(Task.FromResult(formula));
            repository.Setup(r => r.DeleteAsync(formula)).Returns(Task.FromResult(true));
            monitor.Setup(m => m.Deleted());
            controller = new CalculatorController(repository.Object, monitor.Object);
            //Act
            var result = controller.Delete(formula.Name).Result as NegotiatedContentResult<string>;
            //Assert
            repository.Verify();
            Assert.IsNotNull(result);
            Assert.IsFalse(string.IsNullOrEmpty(result.Content));
        }

        [TestMethod]
        public void Delete_NotFoundTest()
        {
            //Arrange
            ServerFormula formula = new ServerFormula { FormulaString = null };
            controller = new CalculatorController(repository.Object, monitor.Object);
            //Act
            var result = controller.Update(formula).Result as NotFoundResult;
            //Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Delete_ErrorSaveTest()
        {
            //Arrange
            ServerFormula formula = new ServerFormula { Name = "A", FormulaString = "5 + 5" };
            repository.Setup(r => r.GetByNameAsync("A")).Returns(Task.FromResult(formula));
            repository.Setup(r => r.CreateAsync(formula)).Returns(Task.FromResult(false));
            controller = new CalculatorController(repository.Object, monitor.Object);
            //Act
            var result = controller.Delete(formula.Name).Result as ExceptionResult;
            //Assert
            repository.Verify();
            Assert.IsNotNull(result);
        }
    }
}
