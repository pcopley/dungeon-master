using System;
using System.Threading.Tasks;
using DM.Dice.Core.Protos;
using Grpc.Core;
using Grpc.Net.Client;

namespace DM.Dice.API.TestClient
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            Console.WriteLine("Enter your roll request: ");
            var request = Console.ReadLine();
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");

            var client = new DiceRoller.DiceRollerClient(channel);

            var rolls = client.Roll(new RollRequest
            {
                Request = request
            });

            await foreach (var roll in rolls.ResponseStream.ReadAllAsync())
            {
                var crit = roll.Result == 1 || roll.Result == roll.Type;
                var succ = crit && roll.Result != 1;
                Console.Write($"Rolled a {roll.Result}/{roll.Type} ");

                if (crit || succ)
                {
                    Console.Write("CRITICAL ");

                    Console.Write(succ ? "SUCCESS" : "FAILURE");
                }

                Console.Write("\r\n");
            }

            Console.WriteLine("Press any key to exit ...");
            Console.ReadKey(true);
        }
    }
}