using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ITest.Models;

namespace ITest.Infrastructure.Providers.Contracts
{
    public interface IUserManagerProvider
    {
        string GetUserId(System.Security.Claims.ClaimsPrincipal principal);
        IQueryable<ApplicationUser> Users { get; }
    }
}
