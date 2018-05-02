using System;
using System.Collections.Generic;
using System.Text;
using ITest.Infrastructure.Providers.Contracts;

namespace ITest.Infrastructure.Providers
{

    public class RandomProvider : IRandomProvider
    {
        private readonly Random random;

        public RandomProvider()
        {
            this.random = new Random();
        }

        public int Next(int maxValue)
        {
            return this.random.Next(maxValue);
        }
    }
}
