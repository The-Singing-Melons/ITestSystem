using Itest.Data.Models;
using ITest.Data.Repository;
using ITest.Data.UnitOfWork;
using ITest.Infrastructure.Providers.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ITest.Services.Data.Tests.CategoryServiceTests
{
    [TestClass]
    public class CategoryServiceTest
    {
        private Mock<IDataRepository<Category>> categoryRepoMock;
        private Mock<IMappingProvider> mapperMock;
        private Mock<IDataSaver> dataSaverMock;
        private Mock<IRandomProvider> randomMock;

        [TestInitialize]
        public void TestInitilize()
        {
            this.CategoryRepoMock = new Mock<IDataRepository<Category>>();
            this.MapperMock = new Mock<IMappingProvider>();
            this.DataSaverMock = new Mock<IDataSaver>();
            this.RandomMock = new Mock<IRandomProvider>();
        }


        public Mock<IDataRepository<Category>> CategoryRepoMock { get => categoryRepoMock; set => categoryRepoMock = value; }
        public Mock<IMappingProvider> MapperMock { get => mapperMock; set => mapperMock = value; }
        public Mock<IDataSaver> DataSaverMock { get => dataSaverMock; set => dataSaverMock = value; }
        public Mock<IRandomProvider> RandomMock { get => randomMock; set => randomMock = value; }
    }
}
