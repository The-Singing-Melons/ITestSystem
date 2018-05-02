using System;
using System.Collections.Generic;
using System.Text;

namespace ITest.Infrastructure.Providers.Contracts
{
    public interface IShuffleProvider
    {
        IList<T> Shuffle<T>(IList<T> list);
    }
}
