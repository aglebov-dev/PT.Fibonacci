using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

namespace PT.Fibonacci.DataAccess.Contracts
{
    public interface IRepository
    {
        Task WriteFibonacciAsync(string correlationId, BigInteger next, CancellationToken token);
        IEnumerable<BigInteger> ReadFibonacciAsync(string correlationId, CancellationToken token);
    }
}
