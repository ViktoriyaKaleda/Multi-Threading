using Sum.Core;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Sum.ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Number: ");
            long number = Convert.ToInt64(Console.ReadLine());
            Console.WriteLine("Calculating... (you can enter a new number)");

            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;

            bool isCompleted = false;

            var keyBoardTask = Task.Run(() =>
            {
                while (!isCompleted)
                {
                    Console.WriteLine("New number: ");
                    bool isParsed = Int64.TryParse(Console.ReadLine(), out number);
                    if (!isParsed)
                        break;

                    source.Cancel();

                    Task.Delay(100);
                }
            });

            while (!isCompleted)
            {
                try
                {
                    var sumTask = new SumHelper().GetSumAsync(number, source.Token);
                    var result = await sumTask;
                    Console.WriteLine();
                    Console.WriteLine($"Sum result = {result}");
                    isCompleted = true;
                }
                catch (OperationCanceledException e)
                {
                    Console.WriteLine("Calculation was canceled.");
                    Console.WriteLine($"Restarting calculation with number {number}.");
                    source.Dispose();
                    source = new CancellationTokenSource();
                }
            }

            await keyBoardTask;
        }
    }
}
