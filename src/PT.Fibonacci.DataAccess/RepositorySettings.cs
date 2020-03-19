using System;

namespace PT.Fibonacci.DataAccess
{
    public class RepositorySettings
    {
        public string ConnectionString { get; private set; }
        public TimeSpan ConnectionTimeout { get; private set; }
    }
}
