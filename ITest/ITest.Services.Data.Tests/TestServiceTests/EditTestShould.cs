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
namespace ITest.Services.Data.Tests.TestServiceTests
{
    [TestClass]
    public class EditTestShould
    {
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
        public void ThrowArgumentNullException_WhenCalledWithInvalidParameter()
        {
            var sut = new TestService(
               testRepoMock.Object, questionRepoMock.Object, answerRepoMock.Object, dataSaverMock.Object,
               mapperMock.Object, categoryRepoMock.Object, randomMock.Object, shufflerMock.Object);

            Assert.ThrowsException<ArgumentNullException>(() => sut.EditTest(null));
        }


        [TestMethod]
        // currently does not pass 
        public void CallDataSaverSaveChangesSucessfully()
        {
            // Arrange
            var testId = new Guid();
            var category = new Category() { Name = "JAVA" };

            var questionAnswers = new List<Answer>()
            {
                new Answer()
                {
                    IsDeleted = false
                }
            };

            var testQuestions = new List<Question>()
            {
                new Question()
                {
                    Answers = questionAnswers,
                    IsDeleted = false
                }
            };

            var testStub = new Test()
            {
                Id = testId,
                Questions = testQuestions,
                Category = category
            };

            var testList = new List<Test>() { testStub };
            var testDtoArgument = new ManageTestDto()
            { Id = testId.ToString(), CategoryName = category.Name };

            this.testRepoMock.Setup(x => x.All)
                .Returns(testList.AsQueryable());

            this.categoryRepoMock.Setup(x => x.All)
                .Returns(new List<Category>() { category }.AsQueryable());

            var sut = new TestService(
              testRepoMock.Object, questionRepoMock.Object, answerRepoMock.Object, dataSaverMock.Object,
              mapperMock.Object, categoryRepoMock.Object, randomMock.Object, shufflerMock.Object);

            // Act

            sut.EditTest(testDtoArgument);

            // Assert

            this.dataSaverMock.Verify(x => x.SaveChanges(), Times.Once);
        }
    }
}
