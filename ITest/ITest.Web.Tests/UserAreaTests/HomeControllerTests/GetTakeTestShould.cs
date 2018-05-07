using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using ITest.DTO;
using ITest.DTO.UserHome.Index;
using ITest.Infrastructure.Providers.Contracts;
using ITest.Services.Data.Contracts;
using ITest.Web.Areas.User.Controllers;
using ITest.Web.Areas.User.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ITest.Web.Tests.UserAreaTests.HomeControllerTests
{
    [TestClass]
    public class GETTakeTestShould
    {
        private Mock<IMappingProvider> mapperMock;
        private Mock<ITestService> testServiceMock;
        private Mock<IUserAnswerService> userAnswerServiceMock;
        private Mock<IUserManagerProvider> userManagerMock;
        private Mock<ICategoryService> categoryServiceMock;
        private Mock<IUserTestService> userTestServiceMock;
        private Mock<IQuestionService> questionServiceMock;

        [TestInitialize]
        public void TestInitialize()
        {
            this.mapperMock = new Mock<IMappingProvider>();
            this.testServiceMock = new Mock<ITestService>();
            this.userAnswerServiceMock = new Mock<IUserAnswerService>();
            this.userManagerMock = new Mock<IUserManagerProvider>();
            this.categoryServiceMock = new Mock<ICategoryService>();
            this.userTestServiceMock = new Mock<IUserTestService>();
            this.questionServiceMock = new Mock<IQuestionService>();
        }


        [TestMethod]
        public void ReturnCorrectRedirect_WhenUserHasCompletedTest()
        {

            var testId = new Guid().ToString();
            var testDto = new TestDto() { Id = testId };

            var fakeHttpContext = new Mock<HttpContext>();
            var fakeIdentity = new GenericIdentity("User");
            var principal = new GenericPrincipal(fakeIdentity, null);
            fakeHttpContext.Setup(t => t.User).Returns(principal);

            var context = new ControllerContext
            {
                HttpContext = fakeHttpContext.Object
            };

            this.userManagerMock.Setup(x => x.GetUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns(new Guid().ToString());

            this.userTestServiceMock.Setup(x =>
                x.UserHasCompletedTest(It.IsAny<String>(), It.IsAny<String>()))
                .Returns(true);


            this.testServiceMock.Setup(x => x.GetTestQuestionsWithAnswers(testId))
                .Returns(testDto).Verifiable();

            var sut = new HomeController(mapperMock.Object, testServiceMock.Object, userManagerMock.Object,
               questionServiceMock.Object, categoryServiceMock.Object, userTestServiceMock.Object,
               userAnswerServiceMock.Object)
            {

                //Set your controller ControllerContext with fake context
                ControllerContext = context
            };

            // Act
            var result = sut.TakeTest(testId) as RedirectToActionResult;

            // Assert
            Assert.AreEqual(result.ActionName, "Index");

        }

        [TestMethod]
        public void ReturnCorrectView_WhenCalledWitValidParameters()
        {
            // Arrange
            // http context mock
            var testId = new Guid().ToString();
            var testDto = new TestDto() { Id = testId };
            var testVM = new TestViewModel() { Id = testId };

            var fakeHttpContext = new Mock<HttpContext>();
            var fakeIdentity = new GenericIdentity("User");
            var principal = new GenericPrincipal(fakeIdentity, null);
            fakeHttpContext.Setup(t => t.User).Returns(principal);

            var context = new ControllerContext
            {
                HttpContext = fakeHttpContext.Object
            };

            this.userManagerMock.Setup(x => x.GetUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns(new Guid().ToString());

            this.testServiceMock.Setup(x => x.GetTestQuestionsWithAnswers(testId))
                .Returns(testDto).Verifiable();

            this.mapperMock.Setup(x => x.MapTo<TestViewModel>(testDto))
                .Returns(testVM);

            var sut = new HomeController(mapperMock.Object, testServiceMock.Object, userManagerMock.Object,
               questionServiceMock.Object, categoryServiceMock.Object, userTestServiceMock.Object,
               userAnswerServiceMock.Object)
            {

                //Set your controller ControllerContext with fake context
                ControllerContext = context
            };

            // Act
            var result = sut.TakeTest(testId) as ViewResult;
            var model = result.Model as TestViewModel;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.AreEqual(model.Id, testId);
        }


        [TestMethod]
        public void Throw_WhenCalledWithInvalidParameters()
        {
            // Arrange

            var sut = new HomeController(mapperMock.Object, testServiceMock.Object, userManagerMock.Object,
                questionServiceMock.Object, categoryServiceMock.Object, userTestServiceMock.Object,
                userAnswerServiceMock.Object);

            Assert.ThrowsException<ArgumentNullException>(() =>
                    sut.TakeTest(string.Empty));
        }

        [TestMethod]
        public void CorrectlyCallMapperMapToWithCorrectParameter()
        {
            // Arrange
            // http context mock
            var testId = new Guid().ToString();
            var testDto = new TestDto() { Id = testId };
            var testVM = new TestViewModel() { Id = testId };

            var fakeHttpContext = new Mock<HttpContext>();
            var fakeIdentity = new GenericIdentity("User");
            var principal = new GenericPrincipal(fakeIdentity, null);
            fakeHttpContext.Setup(t => t.User).Returns(principal);

            var context = new ControllerContext
            {
                HttpContext = fakeHttpContext.Object
            };

            this.userManagerMock.Setup(x => x.GetUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns(new Guid().ToString());

            this.testServiceMock.Setup(x => x.GetTestQuestionsWithAnswers(testId))
                .Returns(testDto).Verifiable();

            this.mapperMock.Setup(x => x.MapTo<TestViewModel>(testDto))
                .Returns(testVM);

            var sut = new HomeController(mapperMock.Object, testServiceMock.Object, userManagerMock.Object,
               questionServiceMock.Object, categoryServiceMock.Object, userTestServiceMock.Object,
               userAnswerServiceMock.Object)
            {

                //Set your controller ControllerContext with fake context
                ControllerContext = context
            };

            // Act
            sut.TakeTest(testId);

            // Assert
            this.mapperMock.Verify(x => x.MapTo<TestViewModel>(testDto), Times.Once);
        }


        [TestMethod]
        public void CorrectlyCallTestServiceGetTestWithQuestionsWithAnswers()
        {
            // Arrange
            // http context mock
            var testId = new Guid().ToString();
            var testDto = new TestDto() { Id = testId };
            var testVM = new TestViewModel() { Id = testId };

            var fakeHttpContext = new Mock<HttpContext>();
            var fakeIdentity = new GenericIdentity("User");
            var principal = new GenericPrincipal(fakeIdentity, null);
            fakeHttpContext.Setup(t => t.User).Returns(principal);

            var context = new ControllerContext
            {
                HttpContext = fakeHttpContext.Object
            };

            this.userManagerMock.Setup(x => x.GetUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns(new Guid().ToString());

            this.testServiceMock.Setup(x => x.GetTestQuestionsWithAnswers(testId))
                .Returns(testDto).Verifiable();

            this.mapperMock.Setup(x => x.MapTo<TestViewModel>(testDto))
                .Returns(testVM);

            var sut = new HomeController(mapperMock.Object, testServiceMock.Object, userManagerMock.Object,
               questionServiceMock.Object, categoryServiceMock.Object, userTestServiceMock.Object,
               userAnswerServiceMock.Object)
            {

                //Set your controller ControllerContext with fake context
                ControllerContext = context
            };

            // Act
            sut.TakeTest(testId);

            // Assert
            testServiceMock.Verify(x => x.GetTestQuestionsWithAnswers(testId), Times.Once);
        }

        [TestMethod]
        public void CorrectlyCallTestServiceShuffleTestWithCorrectArgument()
        {
            // Arrange
            // http context mock
            var testId = new Guid().ToString();
            var testDto = new TestDto() { Id = testId };
            var testVM = new TestViewModel() { Id = testId };

            var fakeHttpContext = new Mock<HttpContext>();
            var fakeIdentity = new GenericIdentity("User");
            var principal = new GenericPrincipal(fakeIdentity, null);
            fakeHttpContext.Setup(t => t.User).Returns(principal);

            var context = new ControllerContext
            {
                HttpContext = fakeHttpContext.Object
            };

            this.userManagerMock.Setup(x => x.GetUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns(new Guid().ToString());

            this.testServiceMock.Setup(x => x.GetTestQuestionsWithAnswers(testId))
                .Returns(testDto).Verifiable();

            this.mapperMock.Setup(x => x.MapTo<TestViewModel>(testDto))
                .Returns(testVM);

            this.testServiceMock.Setup(x => x.ShuffleTest(testDto)).Verifiable();


            var sut = new HomeController(mapperMock.Object, testServiceMock.Object, userManagerMock.Object,
               questionServiceMock.Object, categoryServiceMock.Object, userTestServiceMock.Object,
               userAnswerServiceMock.Object)
            {

                //Set your controller ControllerContext with fake context
                ControllerContext = context
            };

            // Act
            sut.TakeTest(testId);

            // Assert
            testServiceMock.Verify(x => x.ShuffleTest(testDto), Times.Once);
        }

    }
}