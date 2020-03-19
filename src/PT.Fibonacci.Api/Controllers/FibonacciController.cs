using System;
using System.Net;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PT.Fibonacci.Contracts;
using PT.Fibonacci.DataAccess.Contracts;
using PT.Fibonacci.Logic.Contracts;

namespace PT.Fibonacci.Api.Controllers
{
    [Controller]
    public class FibonacciController : ControllerBase
    {
        private readonly IFibonacciService _fibonacciService;
        private readonly IRepository _repository;

        public FibonacciController(IFibonacciService fibonacciService, IRepository repository)
        {
            _fibonacciService = fibonacciService;
            _repository = repository;
        }

        [HttpPut]
        [Route("api/v1/calculate")]
        public async Task Calculate([FromBody] FibonacciRequest request)
        {
            try
            {
                var condition1 = BigInteger.TryParse(request?.PreviousNumber, out var previousNumber);
                var condition2 = BigInteger.TryParse(request?.CurrentNumber, out var currentNumber);
                if (condition1 && condition2)
                {
                    var token = new CancellationTokenSource(TimeSpan.FromSeconds(3)).Token;
                    var next = _fibonacciService.GetNext(previousNumber, currentNumber);
                    await _repository.WriteFibonacciAsync(request.CorrelationId, next, token);
                    Response.StatusCode = (int)HttpStatusCode.Created;
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    //TODO log + return description of error 
                }
            }
            catch
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                //TODO log + return description of exception 
                throw;
            }
        }
    }
}
