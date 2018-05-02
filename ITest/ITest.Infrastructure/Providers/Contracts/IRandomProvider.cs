using System;
using System.Collections.Generic;
using System.Text;

namespace ITest.Infrastructure.Providers.Contracts
{
    public interface IRandomProvider
    {

        int Next(int maxValue);
    }
}
