﻿using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using NATS.Client;
using PT.Fibonacci.DataAccess.Contracts;

namespace PT.Fibonacci.DataAccess
{
    internal class NatsRepository : IRepository
    {
        private readonly RepositorySettings _settings;

        public NatsRepository(IOptions<RepositorySettings> settings)
        {
            _settings = settings.Value;
        }

        public Task WriteFibonacciAsync(string correlationId, BigInteger next, CancellationToken token)
        {
            var factory = new ConnectionFactory();
            using (var connection = factory.CreateConnection(_settings.ConnectionString))
            {
                var data = next.ToByteArray();
                connection.Publish(correlationId, data);
            }

            return Task.CompletedTask;
        }

        public IEnumerable<BigInteger> ReadFibonacciAsync(string correlationId, CancellationToken token)
        {
            var factory = new ConnectionFactory();
            var connection = factory.CreateConnection(_settings.ConnectionString);
            var subscription = connection.SubscribeSync(correlationId);

            return ReadFibonacciAsync212(connection, subscription, token);
        }

        public IEnumerable<BigInteger> ReadFibonacciAsync212(IConnection connection, ISyncSubscription subscription, CancellationToken token)
        {
            var timeout = (int)_settings.ConnectionTimeout.TotalMilliseconds;
            using (connection)
            {
                
                while (!token.IsCancellationRequested)
                {
                    var message = subscription.NextMessage(timeout);
                    var data = new BigInteger(message.Data);

                    yield return data;
                }

                subscription.Dispose();
            }
        }
    }
}
