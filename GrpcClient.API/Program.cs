using System;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using GrpcClient.API;

class Program
{
    static async Task Main(string[] args)
    {
        string serverUrl = "https://localhost:7193"; // Update with your server HTTPS address

        using var channel = GrpcChannel.ForAddress(serverUrl);

        var client = new PrimeNoService.PrimeNoServiceClient(channel);

        while (true)
        {
            Console.Write("Enter a number (1-1000) to check if it's prime: ");
            if (int.TryParse(Console.ReadLine(), out int number) && number >= 1 && number <= 1000)
            {
                var request = new PrimeNumber
                {
                    Id = 1,
                    Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                    Number = number
                };

                try
                {
                    var response =  client.CheckPrime(request);

                    Console.WriteLine($"Is Prime: {response.IsPrime}");
                    Console.WriteLine($"Round Trip Time (RTT): {response.Rtt}ms");
                }
                catch (RpcException ex)
                {
                    Console.WriteLine($"Error: {ex.Status}");
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a number between 1 and 1000.");
            }
        }
    }
}
