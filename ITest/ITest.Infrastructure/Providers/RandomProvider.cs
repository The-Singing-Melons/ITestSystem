using System;
using System.Collections.Generic;
using System.Text;
using ITest.Infrastructure.Providers.Contracts;

namespace ITest.Infrastructure.Providers
{

    public class RandomProvider : IRandomProvider
    {
        private readonly Random random;

        public RandomProvider(Random random)
        {
            this.random = random ?? throw new ArgumentNullException(nameof(random));
        }

        public int Next(int maxValue)
        {
            return this.random.Next(maxValue);
        }
    }
}
