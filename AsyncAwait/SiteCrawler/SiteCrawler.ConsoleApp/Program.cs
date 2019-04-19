using SiteCrawler.Core;
using SiteCrawler.Core.Services;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SiteCrawler.ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //Console.WriteLine("Enter a file path: ");
            //string filePath = Console.ReadLine();
            //Console.WriteLine("Enter link (or links through the space): ");
            //var links = Console.ReadLine().Split();

            string filePath = @"C:\crawler\";
            var links = new List<string> { @"https://github.com/", @"https://habr.com/en/" };

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = cancellationTokenSource.Token;

            var keyBoardTask = Task.Run(() =>
            {
                Console.WriteLine("Press any key to cancel");
                Console.ReadKey();
                cancellationTokenSource.Cancel();
            });

            try
            {
                await new Crawler(new FileStorageService()).DownloadAsync(filePath, links, token);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("Error occurred during HTTP connection.");
            }
            catch (OperationCanceledException e)
            {
                Console.WriteLine("Downloading was canceled.");
            }
        }
    }
}
