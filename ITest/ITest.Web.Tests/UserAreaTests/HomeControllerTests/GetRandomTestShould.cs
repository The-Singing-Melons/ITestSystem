using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
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
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ITest.Web.Tests.UserAreaTests.HomeControllerTests
{
    [TestClass]
    public class GetRandomTestShould
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
        public void SucessfullyReturnJSON_WhenCalledForFirstTime()
        {
            // Arrange

            // http context mock
            var fakeHttpContext = new Mock<HttpContext>();
            var fakeIdentity = new GenericIdentity("User");
            var principal = new GenericPrincipal(fakeIdentity, null);
            fakeHttpContext.Setup(t => t.User).Returns(principal);

            var context = new ControllerContext
            {
                HttpContext = fakeHttpContext.Object,
            };

            this.userManagerMock.Setup(x => x.GetUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns(new Guid().ToString());

            var randomTestViewModelFake = new TestViewModel()
            {
                Id = new Guid().ToString()
            };

            this.testServiceMock.Setup(x => x.GetRandomTest(It.IsAny<string>()))
                .Returns(new TestDto());

            this.mapperMock.Setup(x => x.MapTo<TestViewModel>(It.IsAny<TestDto>()))
                .Returns(randomTestViewModelFake);

            var urlHelperMock = new Mock<IUrlHelper>();

            // in ASP.Core you cannot url.Action because it is an extension method
            //urlHelperMock.Setup(x => x.Action(It.IsAny<string>(), It.IsAny<string>()))
            //    .Returns("TakeTest" + randomTestViewModelFake.Id);

            Expression<Func<IUrlHelper, string>> urlSetup = url =>
            url.Action(It.Is<UrlActionContext>(uac => uac.Action == "TakeTest"));
            urlHelperMock.Setup(urlSetup).Returns("a/mock/url/for/testing").Verifiable();

            var sut = new HomeController(mapperMock.Object, testServiceMock.Object, userManagerMock.Object,
                questionServiceMock.Object, categoryServiceMock.Object, userTestServiceMock.Object,
                userAnswerServiceMock.Object)
            {

                //Set your controller ControllerContext with fake context
                ControllerContext = context,
                Url = urlHelperMock.Object

            };

            // Act
            var result = sut.GetRandomTest("test") as JsonResult;

            Assert.IsInstanceOfType(result, typeof(JsonResult));
        }

        [TestMethod]
        public void SucessfullyCallCheckForTestInProgress_WhenCalledForFirstTime()
        {
            // Arrange

            // http context mock
            var fakeHttpContext = new Mock<HttpContext>();
            var fakeIdentity = new GenericIdentity("User");
            var principal = new GenericPrincipal(fakeIdentity, null);
            fakeHttpContext.Setup(t => t.User).Returns(principal);

            var context = new ControllerContext
            {
                HttpContext = fakeHttpContext.Object,
            };

            this.userManagerMock.Setup(x => x.GetUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns(new Guid().ToString());

            var id = new Guid().ToString();

            var randomTestViewModelFake = new TestViewModel()
            {
                Id = id
            };

            this.testServiceMock.Setup(x => x.GetRandomTest(It.IsAny<string>()))
                .Returns(new TestDto());

            this.mapperMock.Setup(x => x.MapTo<TestViewModel>(It.IsAny<TestDto>()))
                .Returns(randomTestViewModelFake);

            this.userTestServiceMock.Setup(x => x.CheckForTestInProgressFromCategory(id, "test"))
                .Verifiable();

            var urlHelperMock = new Mock<IUrlHelper>();

            Expression<Func<IUrlHelper, string>> urlSetup = url =>
            url.Action(It.Is<UrlActionContext>(uac => uac.Action == "TakeTest"));
            urlHelperMock.Setup(urlSetup).Returns("a/mock/url/for/testing").Verifiable();

            var sut = new HomeController(mapperMock.Object, testServiceMock.Object, userManagerMock.Object,
                questionServiceMock.Object, categoryServiceMock.Object, userTestServiceMock.Object,
                userAnswerServiceMock.Object)
            {

                //Set your controller ControllerContext with fake context
                ControllerContext = context,
                Url = urlHelperMock.Object

            };

            // Act
            var result = sut.GetRandomTest("test");

            this.userTestServiceMock.Verify(x =>
                        x.CheckForTestInProgressFromCategory("test", id), Times.Once);
        }

        [TestMethod]
        public void SucessfullyCallGetRandomTest_WhenCalledForFirstTime()
        {
            // Arrange

            // http context mock
            var fakeHttpContext = new Mock<HttpContext>();
            var fakeIdentity = new GenericIdentity("User");
            var principal = new GenericPrincipal(fakeIdentity, null);
            fakeHttpContext.Setup(t => t.User).Returns(principal);

            var context = new ControllerContext
            {
                HttpContext = fakeHttpContext.Object,
            };

            this.userManagerMock.Setup(x => x.GetUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns(new Guid().ToString());

            var randomTestViewModelFake = new TestViewModel()
            {
                Id = new Guid().ToString()
            };

            this.testServiceMock.Setup(x => x.GetRandomTest(It.IsAny<string>()))
                .Returns(new TestDto());

            this.mapperMock.Setup(x => x.MapTo<TestViewModel>(It.IsAny<TestDto>()))
                .Returns(randomTestViewModelFake);

            var urlHelperMock = new Mock<IUrlHelper>();

            Expression<Func<IUrlHelper, string>> urlSetup = url =>
            url.Action(It.Is<UrlActionContext>(uac => uac.Action == "TakeTest"));
            urlHelperMock.Setup(urlSetup).Returns("a/mock/url/for/testing").Verifiable();

            var sut = new HomeController(mapperMock.Object, testServiceMock.Object, userManagerMock.Object,
                questionServiceMock.Object, categoryServiceMock.Object, userTestServiceMock.Object,
                userAnswerServiceMock.Object)
            {

                //Set your controller ControllerContext with fake context
                ControllerContext = context,
                Url = urlHelperMock.Object

            };

            // Act
            var result = sut.GetRandomTest("test");

            this.testServiceMock.Verify(x => x.GetRandomTest("test"), Times.Once);
        }

        [TestMethod]
        public void Throw_WhenCalledWithInvalidParameters()
        {
            // Arrange

            var sut = new HomeController(mapperMock.Object, testServiceMock.Object, userManagerMock.Object,
                questionServiceMock.Object, categoryServiceMock.Object, userTestServiceMock.Object,
                userAnswerServiceMock.Object);

            Assert.ThrowsException<ArgumentNullException>(() =>
                    sut.GetRandomTest(null));
        }
    }
}
