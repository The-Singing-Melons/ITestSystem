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
    }
}
