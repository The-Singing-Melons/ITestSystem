﻿using System;
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
using Microsoft.Extensions.Caching.Memory;

namespace ITest.Services.Data.Tests.TestServiceTests
{
    [TestClass]
    public class GetRandomTestShould
    {
        private Mock<IDataRepository<Test>> testRepoMock;
        private Mock<IDataRepository<Question>> questionRepoMock;
        private Mock<IDataRepository<Answer>> answerRepoMock;
        private Mock<IDataRepository<Category>> categoryRepoMock;
        private Mock<IDataSaver> dataSaverMock;
        private Mock<IMappingProvider> mapperMock;
        private Mock<IRandomProvider> randomMock;
        private Mock<IShuffleProvider> shufflerMock;
        private Mock<IMemoryCache> memoryCacheMock;

        [TestInitialize]
        public void TestInitialize()
        {
            this.testRepoMock = new Mock<IDataRepository<Test>>();
            this.questionRepoMock = new Mock<IDataRepository<Question>>();
            this.answerRepoMock = new Mock<IDataRepository<Answer>>();
            this.categoryRepoMock = new Mock<IDataRepository<Category>>();
            this.dataSaverMock = new Mock<IDataSaver>();
            this.mapperMock = new Mock<IMappingProvider>();
            this.randomMock = new Mock<IRandomProvider>();
            this.shufflerMock = new Mock<IShuffleProvider>();
            this.memoryCacheMock = new Mock<IMemoryCache>();
        }

        [TestMethod]
        public void ReturnCorrectTestFromCategory()
        {
            // Arrange
            var testId = new Guid();
            var testName = "Random test";
            var testCategory = new Category() { Name = "JAVA" };

            var testsDomain = new List<Test>()
            {
                new Test()
                {
                    Id = testId,
                    Name = testName,
                    IsPublished = true,
                    Category = testCategory
                }
            };

            var testDtoToReturn = new TestDto() { Id = testId.ToString() };

            this.testRepoMock.Setup(x => x.All)
                .Returns(testsDomain.AsQueryable());

            this.randomMock.Setup(x => x.Next(It.IsAny<int>()))
                .Returns(0);

            this.mapperMock.Setup(x => x.MapTo<TestDto>(It.IsAny<Test>()))
                .Returns(testDtoToReturn);


            // Act
            var sut = new TestService(testRepoMock.Object, questionRepoMock.Object,
               answerRepoMock.Object, dataSaverMock.Object, mapperMock.Object, categoryRepoMock.Object,
               randomMock.Object, shufflerMock.Object, memoryCacheMock.Object);

            var result = sut.GetRandomTest(testCategory.Name);

            // Assert
            Assert.AreEqual(testDtoToReturn.Id, result.Id);
        }

        [TestMethod]
        public void ThrowWhenNoTestInCategoryFound()
        {
            // Arrange
            var testId = new Guid();
            var testName = "Random test";
            var testCategory = new Category() { Name = "JAVA" };

            var testsDomain = new List<Test>()
            {
                new Test()
                {
                    Id = testId,
                    Name = testName,
                    IsPublished = true,
                    Category = testCategory
                }
            };

            this.testRepoMock.Setup(x => x.All)
                .Returns(testsDomain.AsQueryable());


            var sut = new TestService(testRepoMock.Object, questionRepoMock.Object,
               answerRepoMock.Object, dataSaverMock.Object, mapperMock.Object, categoryRepoMock.Object,
               randomMock.Object, shufflerMock.Object, memoryCacheMock.Object);

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => sut.GetRandomTest("SQL"));
        }

        [TestMethod]
        public void ThrowWhenCalledWithInvalidParameters()
        {
            // Arrange
            var sut = new TestService(testRepoMock.Object, questionRepoMock.Object,
             answerRepoMock.Object, dataSaverMock.Object, mapperMock.Object, categoryRepoMock.Object,
             randomMock.Object, shufflerMock.Object, memoryCacheMock.Object);

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => sut.GetRandomTest(null));
        }

        [TestMethod]
        public void CallRandomNextSuccesfully()
        {
            // Arrange
            var testId = new Guid();
            var testName = "Random test";
            var testCategory = new Category() { Name = "JAVA" };

            var testsDomain = new List<Test>()
            {
                new Test()
                {
                    Id = testId,
                    Name = testName,
                    IsPublished = true,
                    Category = testCategory
                }
            };

            this.testRepoMock.Setup(x => x.All)
                                            .Returns(testsDomain.AsQueryable());

            this.randomMock.Setup(x => x.Next(It.IsAny<int>()))
                .Returns(0);



            var sut = new TestService(testRepoMock.Object, questionRepoMock.Object,
               answerRepoMock.Object, dataSaverMock.Object, mapperMock.Object, categoryRepoMock.Object,
               randomMock.Object, shufflerMock.Object, memoryCacheMock.Object);

            // Act
            var result = sut.GetRandomTest(testCategory.Name);

            // Assert
            randomMock.Verify(x => x.Next(It.IsAny<int>()), Times.Once);
        }

        [TestMethod]
        public void CallMapperMapToMethodSucessfully()
        {
            // Arrange
            var testId = new Guid();
            var testName = "Random test";
            var testCategory = new Category() { Name = "JAVA" };

            var testsDomain = new List<Test>()
            {
                new Test()
                {
                    Id = testId,
                    Name = testName,
                    IsPublished = true,
                    Category = testCategory
                }
            };

            var testDtoToReturn = new TestDto() { Id = testId.ToString() };

            this.testRepoMock.Setup(x => x.All)
                                            .Returns(testsDomain.AsQueryable());

            this.randomMock.Setup(x => x.Next(It.IsAny<int>()))
                .Returns(0);

            this.mapperMock.Setup(x => x.MapTo<TestDto>(It.IsAny<Test>()))
               .Returns(testDtoToReturn);


            var sut = new TestService(testRepoMock.Object, questionRepoMock.Object,
               answerRepoMock.Object, dataSaverMock.Object, mapperMock.Object, categoryRepoMock.Object,
               randomMock.Object, shufflerMock.Object, memoryCacheMock.Object);

            // Act
            var result = sut.GetRandomTest(testCategory.Name);

            // Assert
            mapperMock.Verify(x => x.MapTo<TestDto>(It.IsAny<Test>()), Times.Once);
        }

    }
}