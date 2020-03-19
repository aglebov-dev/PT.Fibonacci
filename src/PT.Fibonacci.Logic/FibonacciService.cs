using PT.Fibonacci.Logic.Contracts;
using System.Numerics;

namespace PT.Fibonacci.Logic
{
    public class FibonacciService: IFibonacciService
    {
        public BigInteger GetNext(BigInteger previousNumber, BigInteger currentNumber)
        {
            return previousNumber + currentNumber;
        }

        public bool IsFibonacci(long number)
        {
            BigInteger j = 1;
            BigInteger i = 1;
            for (; i < number; i += j)
            {
                j = i - j;
            }

            return i == number;
        }

        public BigInteger GetNext(long number)
        {
            BigInteger j = 1;
            BigInteger i = 1;
            for (; i <= number; i += j)
            {
                j = i - j;
            }

            return i;
        }
    }
}
