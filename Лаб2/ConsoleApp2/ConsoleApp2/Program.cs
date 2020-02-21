using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

namespace ConsoleApp2
{
    public class CopyClass
    {
        public Tpool pool = new Tpool(7);

        public CopyClass()
        {
            Tpool pool = new Tpool(7);
        }

        public void CopyTask(string sourceFolder, string destFolder)
        {

            if (!Directory.Exists(destFolder))
                Directory.CreateDirectory(destFolder);

            foreach (string file in Directory.GetFiles(sourceFolder))
            {
               pool.EnqueueTask(() => File.Copy(file, Path.Combine(destFolder, Path.GetFileName(file))));
               // Console.WriteLine("copyFile");
            }
            foreach (string folder in Directory.GetDirectories(sourceFolder))
                CopyTask(folder, Path.Combine(destFolder, Path.GetFileName(folder)));
        }
    }

    class TaskQueue
    {

        static void Main(string[] args)
        {
            myPath.CountMethod();
            CopyClass k = new CopyClass();
            k.CopyTask(myPath.sourcePath, myPath.targetPath);
            //Console.WriteLine(myPath.count);
            Console.ReadLine();
        }
    }

    public static class myPath
    {
        public static string sourcePath = @"D:\Doc\СПП\a";
        public static string targetPath = @"D:\Doc\СПП\b";
        public static int k, count = 0;

        public static void CountMethod()
        {
            k = 0;
            System.IO.DirectoryInfo directoryInfo = new System.IO.DirectoryInfo(myPath.sourcePath);
            if (directoryInfo.Exists)
            {             
                k = directoryInfo.GetFiles("*.txt", System.IO.SearchOption.AllDirectories).Length;
            }
}
    }

    public class Tpool
    {
        public Object obj = new Object();
        public delegate void TaskDelegate();
        public Queue<TaskDelegate> que;
        
        public Tpool(int count)
        {
            que = new Queue<TaskDelegate>();
            for (int i = 0; i < count; i++)
            {
                Thread myThread = new Thread(new ThreadStart(Choice));
                myThread.Start();
            }
        }

        public void Choice()
        {
            while (myPath.count != myPath.k)
            {
                if (que.Count != 0)
                {
                    TaskDelegate task = null;
                    lock (obj)
                    {
                        if (que.Count != 0)
                        {
                            task = que.Dequeue();
                            myPath.count++;
                        }
                    }
                    if (task != null)
                    {
                        task();
                    }
                }
            }
            Console.WriteLine(myPath.count);
            
        }



        public void EnqueueTask(TaskDelegate task)
        {
            que.Enqueue(task);
        }
    }
}
