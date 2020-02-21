using System;
using System.Threading;

namespace Program
{
    class Mutex
    {
        private int _Id = -1;

        public void Lock()
        {
            int myId = Thread.CurrentThread.ManagedThreadId;
            while (Interlocked.CompareExchange(ref _Id, myId, -1) != -1)
            {
                Thread.Sleep(10);
            }
        }

        public void Unlock()
        {
            int myId = Thread.CurrentThread.ManagedThreadId;
            Interlocked.CompareExchange(ref _Id, -1, myId);
        }
    }

    static class Program
    {
        static void Main(string[] args)
        {
            var mutex = new Mutex();
            for (var i = 0; i < 10; i++)
            {
                new Thread(() =>
                {
                    mutex.Lock();
                    Console.WriteLine("Thread #" + Thread.CurrentThread.ManagedThreadId + " locked mutex.");
                    Thread.Sleep(400);
                    Console.WriteLine("Thread #" + Thread.CurrentThread.ManagedThreadId + " unlocked mutex.");
                    mutex.Unlock();
                }).Start();
            }
            Console.ReadLine();
        }
    }
}
