using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Itest.Data.Models;
using ITest.Data.Repository;
using ITest.Data.UnitOfWork;
using ITest.DTO;
using ITest.Infrastructure.Providers.Contracts;
using ITest.Services.Data.Contracts;

namespace ITest.Services.Data
{
    public class CategoryService : ICategoryService
    {
        private readonly IDataRepository<Category> categoryRepo;
        private readonly IDataSaver dataSaver;
        private readonly IMappingProvider mapper;

        public CategoryService(IDataRepository<Category> categoryRepo, IDataSaver dataSaver, IMappingProvider mapper)
        {
            this.categoryRepo = categoryRepo;
            this.dataSaver = dataSaver;
            this.mapper = mapper;
        }

        public IList<CategoryDto> GetAllCategories()
        {
            var allCategoriesDto = this.mapper.ProjectTo<CategoryDto>(
                this.categoryRepo.All);

            return allCategoriesDto.ToList();
        }

        public IEnumerable<string> GetAllCategoriesNames()
        {
            return this.categoryRepo.All.Select(c => c.Name);
        }
    }
}
