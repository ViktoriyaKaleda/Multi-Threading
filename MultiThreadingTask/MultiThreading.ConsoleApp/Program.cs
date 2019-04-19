using MultiThreading.Core;
using System;

namespace MultiThreading.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var tasks = new MultiThreadingTasks();
            //tasks.ArrayOfTasks();
            //tasks.ChainOfTasks();
            //var r = tasks.Multiply(new int[3, 2] { { 1, -1 }, { 2, 0 }, { 3, 0 } }, new int[2, 2] { { 1, 1 }, { 2, 0 } });
            //tasks.RecursiveThreads();
            //tasks.RecursiveThreadsInThreadPool();
            //tasks.SharedCollection();
            //tasks.TaskContinuations();
            Console.WriteLine("finished");
        }
    }
}
