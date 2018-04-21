using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using ITest.DTO;

namespace ITest.Services.Data.Contracts
{
    public interface IUserService
    {
        IEnumerable<TestDto> GetUserTest();
        string GetLoggedUserId(ClaimsPrincipal user);
    }
}
