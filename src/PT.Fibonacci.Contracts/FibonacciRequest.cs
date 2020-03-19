namespace PT.Fibonacci.Contracts
{
    public class FibonacciRequest
    {
        public string CorrelationId { get; set; }
        public string CurrentNumber { get; set; }
        public string PreviousNumber { get; set; }
    }
}
