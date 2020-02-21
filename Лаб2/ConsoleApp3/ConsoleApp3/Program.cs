using System;
using System.IO;
using System.Threading;

//Реализовать консольную программу на языке C#, которая: 
//- принимает в параметре командной строки путь к исходному и целевому каталогам на диске;
//- выполняет параллельное копирование всех файлов из исходного  каталога в целевой каталог;
//- выполняет операции копирования параллельно с помощью пула потоков;
//- дожидается окончания всех операций копирования и выводит в консоль информацию о количестве скопированных файлов.


namespace ConsoleApp3
{
    static class Program
    {
        static void Main(string[] args)
        {
            const string targetFolder = @"D:\Doc\СПП\a";
            const string destinationFolder = @"D:\Doc\СПП\b";
            FolderCopier.Perform(targetFolder, destinationFolder);

            Console.ReadKey();
        }
    }

    static class FolderCopier
    {
        private static int _threadCount = 0;
        private static readonly ManualResetEvent ResetEvent = new ManualResetEvent(false);

        public static void Perform(string targetFolder, string destinationFolder)
        {
            var filesPath = Directory.GetFiles(targetFolder, "*.*", SearchOption.AllDirectories);
            Interlocked.Exchange(ref _threadCount, filesPath.Length);
            foreach (var filePath in filesPath)
            {
                var files = new Tuple<string, string>(filePath, filePath.Replace(targetFolder, destinationFolder));
                ThreadPool.QueueUserWorkItem(CopyFile, files);
            }

            ResetEvent.WaitOne();
            ResetEvent.Close();
            Console.WriteLine("Files: " + filesPath.Length);
        }

        private static void CopyFile(object files)
        {
            var filesTuple = files as Tuple<string, string>;
            File.Copy(filesTuple.Item1, filesTuple.Item2, true);
            if (Interlocked.Decrement(ref _threadCount) == 0)
            {
                ResetEvent.Set();
            }
        }
    }
}