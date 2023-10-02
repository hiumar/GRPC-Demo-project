using Grpc.Core;
using GrpcServiceStreamer.Protos;

namespace GrpcServiceStreamer.Services
{
    public class PrimeNumberService: PrimeNoService.PrimeNoServiceBase
    {
       

        private readonly ILogger<PrimeNumberService> _logger;
        public PrimeNumberService(ILogger<PrimeNumberService> logger)
        {
            _logger = logger;
        }
        public override Task<PrimeNumberResponse> CheckPrime(PrimeNumber request, ServerCallContext context)
        {
            if (request != null && request.Id != 0 && request.Number != 0)
            {
                // Implement prime number verification logic here
                bool isPrime = IsPrime(request.Number);

                // Calculate RTT
                long rtt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - request.Timestamp;

                _logger.LogInformation($"Received request: ID={request.Id}, Number={request.Number}, IsPrime={isPrime}, RTT={rtt}ms");

                // Create and return response
                return Task.FromResult(new PrimeNumberResponse
                {
                    IsPrime = isPrime,
                    Rtt = rtt
                });
            }
            else
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid headers"));
            }
        }


        private bool IsPrime(long number)
        {
            if (number <= 1) return false;
            if (number <= 3) return true;
            if (number % 2 == 0 || number % 3 == 0) return false;

            for (long i = 5; i * i <= number; i += 6)
            {
                if (number % i == 0 || number % (i + 2) == 0) return false;
            }

            return true;
        }
    }
}
