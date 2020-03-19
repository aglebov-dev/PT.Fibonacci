using Microsoft.Extensions.Logging;
using PT.Fibonacci.Client.Clients;
using PT.Fibonacci.DataAccess.Contracts;
using PT.Fibonacci.Logic.Contracts;
using System;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

namespace PT.Fibonacci.Client.Job
{
    internal class ProcessesJob
    {
        private readonly CancellationTokenSource _lifetime;
        private readonly IFibonacciService _fibonacciService;
        private readonly IRepository _repository;
        private readonly ILogger<ProcessesJob> _logger;
        private readonly FibonacciApiClient _client;

        public ProcessesJob(
            CancellationTokenSource lifetime,
            IFibonacciService fibonacciService,
            IRepository repository,
            ILogger<ProcessesJob> logger,
            FibonacciApiClient client)
        {
            _lifetime = lifetime;
            _fibonacciService = fibonacciService;
            _repository = repository;
            _logger = logger;
            _client = client;
        }

        public async Task StartAsync(CancellationToken token)
        {
            var args = Environment.GetCommandLineArgs();
            if (args?.Length == 2 && int.TryParse(args[1], out var count))
            {
                var tasks = Enumerable.Range(0, count).Select(x => CreateTask(x, token));
                await Task.WhenAll(tasks);
                _lifetime.Cancel();
            }
            else
            {
                throw new Exception("Invalid command line parameter. Enter a number");
            }
        }

        private async Task CreateTask(int taskId, CancellationToken token)
        {
            BigInteger previousNumber = 0;
            BigInteger currentNumber = 1;
            var corralationId = Guid.NewGuid().ToString("N");
            var sequence = _repository.ReadFibonacciAsync(corralationId, token);

            await Send();

            try
            {
                foreach (var number in sequence)
                {
                    var msg = $"[{taskId}] Receive number = {number}, corralationId = {corralationId}";
                    _logger.LogInformation(msg);

                    previousNumber = number;
                    currentNumber = _fibonacciService.GetNext(currentNumber, number);

                    await Send();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw;
            }

            async Task Send()
            {
                _logger.LogInformation($"[{taskId}] Sending value = {currentNumber}, corralationId = {corralationId}");
                await _client.SendFibonacci(previousNumber, currentNumber, corralationId);
            }
        }

        public Task StopAsync(CancellationToken token) => Task.CompletedTask;
    }
}
