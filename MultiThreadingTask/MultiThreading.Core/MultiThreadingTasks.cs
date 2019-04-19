using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Core
{
    public class MultiThreadingTasks
    {
        // Task #1
        // Write a program, which creates an array of 100 Tasks, runs them and wait all of them are not finished. 
        // Each Task should iterate from 1 to 1000 and print into the console 
        // the following string: “Task #0 – {iteration number}”.
        public void ArrayOfTasks()
        {
            Action action = () =>
            {
                for (int i = 0; i < 1000; i++)
                {
                    Console.WriteLine($"Task #{Task.CurrentId} - {i}");
                }                
            };

            var tasks = new List<Task>();
            for (int i = 0; i < 100; i++)
            {
                int index = i;
                //tasks.Add(Task.Factory.StartNew(action));
                tasks.Add(Task.Run(action));
            }

            Task.WaitAll(tasks.ToArray());
        }

        // Task #2
        // Write a program, which creates a chain of four Tasks. 
        // First Task – creates an array of 10 random integer. 
        // Second Task – multiplies this array with another random integer. 
        // Third Task – sorts this array by ascending. 
        // Fourth Task – calculates the average value. All this tasks should print the values to console
        public void ChainOfTasks()
        {
            Task<int[]> task1 = Task.Run(() =>
            {
                Console.WriteLine("Task #1");
                var array = new int[10];
                var random = new Random();
                for (int i = 0; i < 10; i++)
                {
                    array[i] = random.Next(20);
                }
                PrintEnumerableToConsole(array);
                return array;
            });

            Task<int[]> task2 = task1.ContinueWith((t) =>
            {
                Console.WriteLine("Task #2");
                var array = t.Result;
                var randomNumber = new Random().Next(10);
                Console.WriteLine($"Random number: {randomNumber}");
                for (int i = 0; i < 10; i++)
                {
                    array[i] *= randomNumber;
                }
                PrintEnumerableToConsole(array);
                return array;
            });

            Task<int[]> task3 = task2.ContinueWith((t) =>
            {
                Console.WriteLine("Task #3");
                var result = t.Result.OrderBy(i => i).ToArray();
                PrintEnumerableToConsole(result);
                return result;
            });

            Task<double> task4 = task3.ContinueWith((t) =>
            {
                Console.WriteLine("Task #4");
                double result = t.Result.Average();
                Console.WriteLine($"The average value: {result}");
                return result;
            });

            task4.Wait();
        }
        
        private void PrintEnumerableToConsole<T>(IEnumerable<T> array)
        {
            Console.WriteLine("Array items: ");
            foreach (var item in array)
            {
                Console.Write($"{item}  ");
            }
            Console.WriteLine();
        }

        // Task #3
        // Write a program, which multiplies two matrices and uses class Parallel.
        public int[,] Multiply(int[,] firstArray, int[,] secondArray)
        {
            int firstArrayRows = firstArray.GetLength(0);
            int firstArrayColumns = firstArray.GetLength(1);
            int secondArrayRows = secondArray.GetLength(0);
            int secondArrayColumns = secondArray.GetLength(1);

            if (firstArrayColumns != secondArrayRows)
            {
                throw new ArgumentException("The number of columns in first array should be equals with the number of rows in second array.");
            }

            var resultArray = new int[firstArrayRows, secondArrayColumns];

            Parallel.For(0, firstArrayRows, i =>
                {
                    for (int j = 0; j < secondArrayColumns; j++)
                    {
                        for (int k = 0; k < secondArrayRows; k++)
                        {
                            resultArray[i, j] += firstArray[i, k] * secondArray[k, j];
                        }
                    }
                }
            );

            return resultArray;
        }

        // Task #4
        // Write a program which recursively creates 10 threads. Each thread should be with 
        // the same body and receive a state with integer number, decrement it, print 
        // and pass as a state into the newly created thread. 
        // Use Thread class for this task and Join for waiting threads.
        public void RecursiveThreads()
        {
            var thread = new Thread(() => RecursiveThreadsTaskBody(10));
            thread.Start();
            thread.Join();
        }

        private void RecursiveThreadsTaskBody(int state)
        {
            Console.WriteLine($"Thread #{Thread.CurrentThread.ManagedThreadId} - {state--}");
            if (state == 0)
                return;

            var thread = new Thread(() => RecursiveThreadsTaskBody(state));
            thread.Start();
            thread.Join();
        }

        private static Semaphore Semaphore { get; } = new Semaphore(1, 1);

        // Task #5
        // Write a program which recursively creates 10 threads. Each thread should be with 
        // the same body and receive a state with integer number, decrement it, print and pass
        // as a state into the newly created thread. 
        // Use ThreadPool class for this task and Semaphore for waiting threads.
        public void RecursiveThreadsInThreadPool()
        {
            ThreadPool.QueueUserWorkItem(RecursiveThreadsInThreadPoolTaskBody, 10);
            Semaphore.WaitOne();
        }

        private void RecursiveThreadsInThreadPoolTaskBody(object state)
        {
            int currentState = (int)state;
            Console.WriteLine($"Thread #{Thread.CurrentThread.ManagedThreadId} - {currentState--}");
            if (currentState > 0)
            {
                ThreadPool.QueueUserWorkItem(RecursiveThreadsInThreadPoolTaskBody, currentState);
                Semaphore.WaitOne();
            }

            Semaphore.Release();
        }

        // Task #6
        // Write a program which creates two threads and a shared collection: 
        // the first one should add 10 elements into the collection and 
        // the second should print all elements in the collection after each adding. 
        // Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.
        public void SharedCollection()
        {
            var collection = new ConcurrentBag<int>();
            var eventWaitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);

            var task1 = Task.Run(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    collection.Add(i);
                    eventWaitHandle.Set();
                    Thread.Sleep(10);
                }
            });

            var task2 = Task.Run(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    eventWaitHandle.WaitOne();
                    PrintEnumerableToConsole(collection);
                }
            });

            Task.WaitAll(task1, task2);
        }

        // Task #7
        public void TaskContinuations()
        {
            var task1 = Task.Run(() =>
            {
                return ParentTask(1);
            });

            // a. Continuation task should be executed regardless of the result of the parent task.
            var firstContinuation = task1.ContinueWith((t) =>
            {
                var result = t.Result;
                if (result > 0)
                {
                    TaskContinuation(1, "The result is positive.");
                }
                else
                {
                    // ...
                }
                return true;
            });

            var task2 = Task.Run(() =>
            {
                ParentTask(2);
                throw new Exception("Exception of the second task.");
            });

            // b. Continuation task should be executed when the parent task finished without success.
            var secondContinuation = task2.ContinueWith((t) => 
                TaskContinuation(2, "Parent task finished without success."), TaskContinuationOptions.OnlyOnFaulted);

            var task3 = Task.Run(() =>
            {
                ParentTask(3);
            });

            // c. Continuation task should be executed when the parent task would be finished with fail 
            // and parent task thread should be reused for continuation
            var thirdContinuation = task3.ContinueWith((t) => 
                TaskContinuation(3, "Executing in the same thread."), TaskContinuationOptions.ExecuteSynchronously);

            // or
            var task3_2 = Task.Run(() =>
            {
                var result = ParentTask(3);
                TaskContinuation(result, "Executing in the same thread. v2.");
            });

            var task4 = Task.Run(() =>
            {
                ParentTask(4);
            });

            // d. Continuation task should be executed outside of the thread pool when the parent task would be canceled
            var fourthContinuation = task4.ContinueWith((t) => 
                TaskContinuation(4, "Parent task is completed."), TaskContinuationOptions.OnlyOnRanToCompletion);

            var fourthContinuationOnCancelled = task4.ContinueWith((t) => Console.WriteLine("Parent task was canceled."), 
                TaskContinuationOptions.OnlyOnCanceled | TaskContinuationOptions.LongRunning);

            try
            {
                Task.WaitAll(task1, task2, task3, task3_2, task4);
            }
            catch (AggregateException e)
            {
                foreach (var innerException in e.InnerExceptions)
                    Console.WriteLine(innerException.Message);
            }            
        }

        private int ParentTask(int number)
        {
            Console.WriteLine($"Parent task {number}");
            return 3;
        }

        private void TaskContinuation(int number, string message)
        {            
            Console.WriteLine($"Task continuation #{number}. {message}");
        }
    }
}
