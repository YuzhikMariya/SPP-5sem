using System;
using System.Threading;

//Создать на языке C# статический метод класса Parallel.WaitAll, который: 
//- принимает в параметрах массив делегатов;
//- выполняет все указанные делегаты параллельно с помощью пула потоков;
//- дожидается окончания выполнения всех делегатов.
//Реализовать простейший пример использования метода Parallel.WaitAll.

namespace ConsoleApp1
{
    delegate void ParallelDelegate();

    public class Counter
    {
        public int Value;
        public Counter(int value)
        {
            Value = value;
        }
    }

    static class Parallel
    {
        public static void WaitAll(ParallelDelegate[] delegates)
        {
            ManualResetEvent resetEvent = new ManualResetEvent(false);
            Counter threadsCount = new Counter(delegates.Length);
            foreach (var parallelDelegate in delegates)
            {
                var info = new Tuple<ParallelDelegate, Counter, ManualResetEvent>(parallelDelegate, threadsCount, resetEvent);
                ThreadPool.QueueUserWorkItem(DelegateExecute, info);
            }
            resetEvent.WaitOne();
        }

        private static void DelegateExecute(object info)
        {
            var tupleInfo = info as Tuple<ParallelDelegate, Counter, ManualResetEvent>;
            tupleInfo.Item1();
            if (Interlocked.Decrement(ref tupleInfo.Item2.Value) == 0)
            {
                tupleInfo.Item3.Set();
            }
        }
    }

    static class Program
    {
        private static void Sleep3()
        {
            Console.WriteLine("1 started.");
            Thread.Sleep(3000);
            Console.WriteLine("1 finished.");
        }

        private static void Sleep5()
        {
            Console.WriteLine("2 started.");
            Thread.Sleep(5000);
            Console.WriteLine("2 finished.");
        }
        private static void Sleep7()
        {
            Console.WriteLine("3 started.");
            Thread.Sleep(7000);
            Console.WriteLine("3 finished.");
        }

        static void Main(string[] args)
        {
            Parallel.WaitAll(new ParallelDelegate[] { Sleep7, Sleep3, Sleep5 });
            Console.WriteLine("Program: finished.");
            Console.ReadKey();
        }
    }

}