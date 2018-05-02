using System;
using System.Collections.Generic;
using System.Text;
using ITest.Infrastructure.Providers.Contracts;

namespace ITest.Infrastructure.Providers
{
    public class ShuffleProvider : IShuffleProvider
    {
        private readonly IRandomProvider random;

        public ShuffleProvider(IRandomProvider random)
        {
            this.random = random;
        }

        public IList<T> Shuffle<T>(IList<T> list)
        {
            var listCount = list.Count;

            for (int i = 0; i < list.Count; i++)
            {
                var randomIndex = i + random.Next(listCount - i);
                var temp = list[randomIndex];
                list[randomIndex] = list[i];
                list[i] = temp;
            }
            return list;
        }
    }
}
