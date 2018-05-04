using System;
using System.Collections.Generic;
using System.Linq;
using Itest.Data.Models;
using ITest.Data.Repository;
using ITest.Data.UnitOfWork;
using ITest.DTO;
using ITest.Infrastructure.Providers.Contracts;
using ITest.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
namespace ITest.Services.Data.Tests.TestServiceTests
{
    [TestClass]
    public class CreateTestShould
    {
        private Mock<IDataRepository<ApplicationUser>> userRepoMock;
        private Mock<IDataRepository<Test>> testRepoMock;
        private Mock<IDataRepository<Question>> questionRepoMock;
        private Mock<IDataRepository<Answer>> answerRepoMock;
        private Mock<IDataRepository<Category>> categoryRepoMock;
        private Mock<IDataSaver> dataSaverMock;
        private Mock<IMappingProvider> mapperMock;
        private Mock<IRandomProvider> randomMock;
        private Mock<IShuffleProvider> shufflerMock;

        [TestInitialize]
        public void TestInitialize()
        {
            this.userRepoMock = new Mock<IDataRepository<ApplicationUser>>();
            this.testRepoMock = new Mock<IDataRepository<Test>>();
            this.questionRepoMock = new Mock<IDataRepository<Question>>();
            this.answerRepoMock = new Mock<IDataRepository<Answer>>();
            this.categoryRepoMock = new Mock<IDataRepository<Category>>();
            this.dataSaverMock = new Mock<IDataSaver>();
            this.mapperMock = new Mock<IMappingProvider>();
            this.randomMock = new Mock<IRandomProvider>();
            this.shufflerMock = new Mock<IShuffleProvider>();
        }

        [TestMethod]
        public void CallRepoAdd_WithCorrectParameter()
        {
            var testDtoArgument = new ManageTestDto() { CategoryName = "Java" };
            var testToAdd = new Test();
            this.mapperMock.Setup(x => x.MapTo<Test>(It.IsAny<ManageTestDto>()))
                .Returns(testToAdd);

            var categories = new List<Category>()
            {
                new Category()
                {
                    Name = "Java"
                }
            };

            this.categoryRepoMock.Setup(x => x.All)
                .Returns(categories.AsQueryable());

            var sut = new TestService(userRepoMock.Object, testRepoMock.Object, questionRepoMock.Object,
                answerRepoMock.Object, dataSaverMock.Object, mapperMock.Object, categoryRepoMock.Object,
                randomMock.Object, shufflerMock.Object);

            sut.CreateTest(testDtoArgument);

            this.testRepoMock.Verify(x => x.Add(testToAdd), Times.Once);
        }

        [TestMethod]
        public void CallDataSaverAdd()
        {
            var testDtoArgument = new ManageTestDto() { CategoryName = "Java" };
            var testToAdd = new Test();
            this.mapperMock.Setup(x => x.MapTo<Test>(It.IsAny<ManageTestDto>()))
                .Returns(testToAdd);

            var categories = new List<Category>()
            {
                new Category()
                {
                    Name = "Java"
                }
            };

            this.categoryRepoMock.Setup(x => x.All)
                .Returns(categories.AsQueryable());

            var sut = new TestService(userRepoMock.Object, testRepoMock.Object, questionRepoMock.Object,
                answerRepoMock.Object, dataSaverMock.Object, mapperMock.Object, categoryRepoMock.Object,
                randomMock.Object, shufflerMock.Object);

            sut.CreateTest(testDtoArgument);

            this.dataSaverMock.Verify(x => x.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void CallMapperMapTo_WithCorrectParameters()
        {
            // Arrange
            var testDtoArgument = new ManageTestDto() { CategoryName = "Java" };
            var testToAdd = new Test();
            this.mapperMock.Setup(x => x.MapTo<Test>(It.IsAny<ManageTestDto>()))
                .Returns(testToAdd);

            var categories = new List<Category>()
            {
                new Category()
                {
                    Name = "Java"
                }
            };

            this.categoryRepoMock.Setup(x => x.All)
                .Returns(categories.AsQueryable());

            var sut = new TestService(userRepoMock.Object, testRepoMock.Object, questionRepoMock.Object,
                answerRepoMock.Object, dataSaverMock.Object, mapperMock.Object, categoryRepoMock.Object,
                randomMock.Object, shufflerMock.Object);

            // Act
            sut.CreateTest(testDtoArgument);

            // Assert
            this.mapperMock.Verify(x => x.MapTo<Test>(testDtoArgument), Times.Once);
        }

        [TestMethod]
        public void ThrowArgumentNull_WhenCalledWithInvalidParameter()
        {
            // Arrange
            var sut = new TestService(userRepoMock.Object, testRepoMock.Object, questionRepoMock.Object,
            answerRepoMock.Object, dataSaverMock.Object, mapperMock.Object, categoryRepoMock.Object,
            randomMock.Object, shufflerMock.Object);

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => sut.CreateTest(null));
        }
    }
}
