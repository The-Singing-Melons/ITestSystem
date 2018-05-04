using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Itest.Data.Models;
using ITest.Data.Repository;
using ITest.Data.UnitOfWork;
using ITest.DTO;
using ITest.DTO.UserHome.Index;
using ITest.Infrastructure.Providers.Contracts;
using ITest.Services.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ITest.Services.Data.Tests.CategoryServiceTests
{
    [TestClass]
    public class GetAllCategoriesShould  // : CategoryServiceTest
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
                    Name = "C# .NET",
                    Tests = new List<Test>()
                },

                new Category()
                {
                    Name = "SQL",
                    Tests = new List<Test>()
                }
            };


            this.categoryRepoMock.Setup(x => x.All)
                .Returns(allCategoriesDomain.AsQueryable);

            var sut = new CategoryService(categoryRepoMock.Object, dataSaverMock.Object, mapperMock.Object);

            //Act

            var allCategories = sut.GetAllCategories("test");

            // Assert
            Assert.AreEqual(2, allCategories.Count());
        }

    }
}
