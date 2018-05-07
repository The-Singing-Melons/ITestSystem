using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
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
    public class IndexShould
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
        public void Index_CorrectlyRedirectsWhenTestIsOverdue()
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
            var result = sut.Index() as RedirectToActionResult;

            // Assert
            Assert.AreEqual(result.ActionName, "Index");

        }

        [TestMethod]
        public void Index_ReturnsAViewResult_WithAListOfCategoryViewModels()
        {
            // Arrange
            var categories = new List<CategoryViewModel>()
            {
                new CategoryViewModel(),
                new CategoryViewModel()
            };

            // http context mock
            var fakeHttpContext = new Mock<HttpContext>();
            var fakeIdentity = new GenericIdentity("User");
            var principal = new GenericPrincipal(fakeIdentity, null);
            fakeHttpContext.Setup(t => t.User).Returns(principal);
            // in ASP.Core cannot mock Controller context 
            //   controllerContext.Setup(t => t.HttpContext).Returns(fakeHttpContext.Object);
            // because controller context is not virtual
            var context = new ControllerContext
            {
                HttpContext = fakeHttpContext.Object
            };


            this.userManagerMock.Setup(x => x.GetUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns(new Guid().ToString());

            this.mapperMock.Setup(x =>
                x.EnumerableProjectTo<CategoryIndexDto, CategoryViewModel>
                (It.IsAny<IEnumerable<CategoryIndexDto>>()))
                .Returns(categories);

            var sut = new HomeController(mapperMock.Object, testServiceMock.Object, userManagerMock.Object,
                questionServiceMock.Object, categoryServiceMock.Object, userTestServiceMock.Object,
                userAnswerServiceMock.Object)
            {

                //Set your controller ControllerContext with fake context
                ControllerContext = context
            };

            // Act
            var result = sut.Index() as ViewResult;
            var model = result.Model as List<CategoryViewModel>;

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.IsNotNull(result);
            Assert.AreEqual(2, model.Count);
        }

    }
}

