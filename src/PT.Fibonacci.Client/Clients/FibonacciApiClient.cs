using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using RestSharp;
using PT.Fibonacci.Contracts;
using System.Numerics;

namespace PT.Fibonacci.Client.Clients
{
    public class FibonacciApiClient
    {
        private readonly RestClient _client;

        public FibonacciApiClient(IConfiguration configuration)
        {
            var clientUrl = configuration.GetValue<string>("Api");
            _client = new RestClient(clientUrl);
        }

        public async Task SendFibonacci(BigInteger previousNumber, BigInteger currentNumber, string corralationId)
        {
            var request = new RestRequest("/api/v1/calculate", Method.PUT, DataFormat.Json);
            var body = new FibonacciRequest
            {
                PreviousNumber = previousNumber.ToString(),
                CurrentNumber = currentNumber.ToString(),
                CorrelationId = corralationId
            };

            request
                .AddJsonBody(body)
                .AddHeader("Corralation-Id", corralationId);

            var response = await _client.ExecuteAsync(request);
            if (response.StatusCode != System.Net.HttpStatusCode.Created)
            {
                //TODO add specific exception
                throw new Exception($"Response contains isn't available code: {response.StatusCode}");
            }
        }
    }
}
