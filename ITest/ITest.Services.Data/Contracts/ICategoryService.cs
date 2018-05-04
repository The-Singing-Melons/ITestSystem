using System;
using System.Collections.Generic;
using System.Text;
using ITest.DTO;
using ITest.DTO.UserHome.Index;

namespace ITest.Services.Data.Contracts
{
    public interface ICategoryService
    {
        IEnumerable<CategoryIndexDto> GetAllCategories(string userId);

        IEnumerable<string> GetAllCategoriesNames();
    }
}
