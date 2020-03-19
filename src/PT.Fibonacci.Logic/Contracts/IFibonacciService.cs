using System.Numerics;

namespace PT.Fibonacci.Logic.Contracts
{
    public interface IFibonacciService
    {
        BigInteger GetNext(BigInteger previousNumber, BigInteger currentNumber);
    }
}
