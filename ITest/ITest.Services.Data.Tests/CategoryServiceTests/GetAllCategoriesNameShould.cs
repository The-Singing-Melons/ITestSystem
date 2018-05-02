using System.Collections.Generic;
using System.Linq;
using Itest.Data.Models;
using ITest.Data.Repository;
using ITest.Data.UnitOfWork;
using ITest.Infrastructure.Providers.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ITest.Services.Data.Tests.CategoryServiceTests
{
    [TestClass]
    public class GetAllCategoriesNameShould  // : CategoryServiceTest
    {
        private Mock<IDataRepository<Category>> categoryRepoMock;
        private Mock<IMappingProvider> mapperMock;
        private Mock<IDataSaver> dataSaverMock;
        private Mock<IRandomProvider> randomMock;

        [TestInitialize]
        public void TestInitilize()
        {
            this.categoryRepoMock = new Mock<IDataRepository<Category>>();
            this.mapperMock = new Mock<IMappingProvider>();
            this.dataSaverMock = new Mock<IDataSaver>();
            this.randomMock = new Mock<IRandomProvider>();
        }

        [TestMethod]
        public void ReturnCorrectDataWhenInvoked()
        {
            // Arrange

            var allCategoriesDomain = new List<Category>()
            {
                new Category()
                {
                    Name = "C# .NET"
                },

                new Category()
                {
                    Name = "SQL"
                }
            };

            this.categoryRepoMock.Setup(x => x.All)
               .Returns(allCategoriesDomain.AsQueryable());

            // Act
            var sut = new CategoryService(categoryRepoMock.Object, dataSaverMock.Object, mapperMock.Object);

            var actualResult = sut.GetAllCategoriesNames();

            // Assert
            Assert.AreEqual(allCategoriesDomain.Count(), actualResult.Count());
        }
    }
}