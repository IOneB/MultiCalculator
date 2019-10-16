using ClientsLibrary.ClientBase.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;

namespace ClientLibraryTests
{
    [TestClass]
    public class QueryTests
    {
        [TestMethod]
        public void WithTest()
        {
            //Arrange
            Query query = new Query("address", null);

            //Act
            var queryWithContent = query.With(new StringContent("aa"));

            //Assert
            Assert.IsNotNull(queryWithContent.Content);
            Assert.AreEqual(query.Address, queryWithContent.Address);
        }
    }
}
