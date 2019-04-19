using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiThreadingTask
{
    public class MultiThreadingTasks
    {
        public void ArrayOfTasks()
        {
            Action<object> action = (object obj) =>
            {
                int i = (int)obj;
                Console.WriteLine($"Task #{Task.CurrentId} - {i}");
            };

            var tasks = new List<Task>();
            for (int i = 0; i < 100; i++)
            {
                int index = i;
                tasks.Add(Task.Factory.StartNew(action, index));
            }

            Task.WaitAll(tasks.ToArray());
        }
    }
}
