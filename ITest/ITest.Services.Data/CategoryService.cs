﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Itest.Data.Models;
using ITest.Data.Repository;
using ITest.Data.UnitOfWork;
using ITest.DTO;
using ITest.DTO.UserHome.Index;
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
            this.categoryRepo = categoryRepo ?? throw new ArgumentNullException(nameof(categoryRepo));
            this.dataSaver = dataSaver ?? throw new ArgumentNullException(nameof(dataSaver));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public IEnumerable<CategoryIndexDto> GetAllCategories(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException("userId cannot be null or empty", nameof(userId));
            }

            var categoryIndexDto = this.categoryRepo.All.Select(c => new CategoryIndexDto()
            {
                Name = c.Name,
                HasUserTakenTestForThisCategory = c.Tests
                        .SelectMany(t => t.UserTests)
                        .Any(ut => ut.UserId == userId && ut.IsSubmited == true)
            });

            return categoryIndexDto;
        }

        public IEnumerable<string> GetAllCategoriesNames()
        {
            return this.categoryRepo.All.Select(c => c.Name);
        }
    }
}
