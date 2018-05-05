using System;

namespace ITest.Infrastructure.Providers
{
    public class TimeProvider
    {
        public virtual DateTime Now { get => DateTime.Now; }
    }
}
