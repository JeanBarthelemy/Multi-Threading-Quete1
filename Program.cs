using System.Threading;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Multi_Threading_Quete1
{
    class Program
    {
        private Int32 SharedInteger { get; set; }
        private static Mutex mut = new Mutex();
        private List<Thread> threadPlayers = new List<Thread>();

        public static void Main()
        {
            var program = new Program();
            program.Run();
        }

        public void Run()
        {
            var threadStartDelegate = new ThreadStart(OnThreadStart);
            var threads = new List<Thread> 
            {
                    new Thread(threadStartDelegate),
                    new Thread(threadStartDelegate),
                    new Thread(threadStartDelegate)
            };
          
            foreach (var t in threads)
            {
                t.Start(); // We launch the thread
                
            }

            
            // Do something while threads are executing ...

            foreach (var t in threads)
            {
                t.Join(); // We wait until all threads are finished
            }

            Console.WriteLine("Threads are joined");
            Console.WriteLine();

            Console.WriteLine("The winner is thread " + threadPlayers.First().ManagedThreadId);

            Console.ReadKey();
        }

        private void OnThreadStart()
        {
            
            var random = new Random();
            var executionTime = random.Next(500, 800);
            var timeSpan = TimeSpan.FromMilliseconds(executionTime);
            
            mut.WaitOne();
            SharedInteger += timeSpan.Milliseconds;
            Thread.Sleep(timeSpan); // Simulate computing by waiting a random period of time
            Console.WriteLine("Time consuming of thread " + Thread.CurrentThread.ManagedThreadId + " : " + SharedInteger + " milliseconds");
            mut.ReleaseMutex();

            threadPlayers.Add(Thread.CurrentThread);

        }
    }
}

