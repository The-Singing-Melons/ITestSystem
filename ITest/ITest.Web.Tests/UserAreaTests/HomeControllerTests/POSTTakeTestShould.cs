using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using ITest.DTO;
using ITest.DTO.TakeTest;
using ITest.DTO.UserHome.Index;
using ITest.Infrastructure.Providers.Contracts;
using ITest.Services.Data.Contracts;
using ITest.Web.Areas.User.Controllers;
using ITest.Web.Areas.User.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ITest.Web.Tests.UserAreaTests.HomeControllerTests
{
    [TestClass]
    public class POSTTakeTestShould
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
        public void POSTTakeTestCorrectlyRedirectsToIndex_WhenTestIsOverdue()
        {
            // Arrange

            // http context mock
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
                x.CheckForOverdueTestInProgress(It.IsAny<String>()))
                .Returns(true);

            var sut = new HomeController(mapperMock.Object, testServiceMock.Object, userManagerMock.Object,
                questionServiceMock.Object, categoryServiceMock.Object, userTestServiceMock.Object,
                userAnswerServiceMock.Object)
            {

                //Set your controller ControllerContext with fake context
                ControllerContext = context
            };

            // Act
            var result = sut.TakeTest(new TestRequestViewModel()) as RedirectToActionResult;

            // Assert
            Assert.AreEqual(result.ActionName, "Index");
        }

        [TestMethod]
        public void POSTTakeTestReturnsRedirectToAction_WhenModelStateIsValid()
        {

            var fakeHttpContext = new Mock<HttpContext>();
            var fakeIdentity = new GenericIdentity("User");
            var principal = new GenericPrincipal(fakeIdentity, null);
            fakeHttpContext.Setup(t => t.User).Returns(principal);

            var tempDataMock = new Mock<ITempDataDictionary>();

            tempDataMock.Setup(x => x["Success - Message"])
                .Returns("Your test was submited!");


            var context = new ControllerContext
            {
                HttpContext = fakeHttpContext.Object
            };


            var id = new Guid().ToString();
            var takeTestRequestVM = new TestRequestViewModel()
            {
                CategoryName = "test",
                Id = id,
                Questions = new List<QuestionResponseModel>()
            };

            var testRequestDto = new TestRequestDto();
            this.mapperMock.Setup(x =>
                        x.MapTo<TestRequestDto>(takeTestRequestVM))
                        .Returns(testRequestDto);


            var sut = new HomeController(mapperMock.Object, testServiceMock.Object, userManagerMock.Object,
              questionServiceMock.Object, categoryServiceMock.Object, userTestServiceMock.Object,
              userAnswerServiceMock.Object)
            {

                //Set your controller ControllerContext with fake context
                ControllerContext = context,
                TempData = tempDataMock.Object
            };

            // Act
            var result = sut.TakeTest(takeTestRequestVM) as RedirectToActionResult;

            // Assert
            Assert.AreEqual(result.ActionName, "Index");

            //var model = result.Model as TestRequestViewModel;


            //Assert.IsInstanceOfType(result, typeof(ViewResult));
            //Assert.IsNotNull(result);
            //Assert.AreEqual(model.Id, id);
        }

        [TestMethod]
        public void POSTTakeTestCallsUserTestServiceSubmitTest_WhenModelStateIsValid()
        {

            var fakeHttpContext = new Mock<HttpContext>();
            var fakeIdentity = new GenericIdentity("User");
            var principal = new GenericPrincipal(fakeIdentity, null);
            fakeHttpContext.Setup(t => t.User).Returns(principal);

            var tempDataMock = new Mock<ITempDataDictionary>();

            tempDataMock.Setup(x => x["Success - Message"])
                .Returns("Your test was submited!");


            var context = new ControllerContext
            {
                HttpContext = fakeHttpContext.Object
            };


            var id = new Guid().ToString();
            var takeTestRequestVM = new TestRequestViewModel()
            {
                CategoryName = "test",
                Id = id,
                Questions = new List<QuestionResponseModel>()
            };

            var testRequestDto = new TestRequestDto();
            this.mapperMock.Setup(x =>
                        x.MapTo<TestRequestDto>(takeTestRequestVM))
                        .Returns(testRequestDto);

            this.userTestServiceMock.Setup(
                x => x.SubmitUserTest(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                .Verifiable();


            var sut = new HomeController(mapperMock.Object, testServiceMock.Object, userManagerMock.Object,
              questionServiceMock.Object, categoryServiceMock.Object, userTestServiceMock.Object,
              userAnswerServiceMock.Object)
            {

                //Set your controller ControllerContext with fake context
                ControllerContext = context,
                TempData = tempDataMock.Object
            };

            // Act
            var result = sut.TakeTest(takeTestRequestVM);

            // Assert
            this.userTestServiceMock.Verify(
               x => x.SubmitUserTest(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()), Times.Once);

        }
    }
}
