using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace ConsoleApp1
{

    class TaskQueue
    {

        static void Main(string[] args)
        {
            Tpool pool = new Tpool();
            pool.Del(myPath.targetPath);
            pool.CopyTask(myPath.sourcePath, myPath.targetPath);
            pool.StartPool(7);
            Console.WriteLine(myPath.count);
            Console.ReadLine();
        }
    }

    public static class myPath
    {
        public static string sourcePath = @"D:\Doc\НИНАДА";
        public static string targetPath = @"D:\Doc\СПП\b";
        public static int count = 0;
    }

    public class Tpool
    {
        public Object obj = new Object();
        public delegate void TaskDelegate();
        public Queue<TaskDelegate> que;

        public Tpool()
        {
            que = new Queue<TaskDelegate>();
        }

        public void StartPool(int count)
        {          
            for (int i = 0; i < count; i++)
            {
                Thread myThread = new Thread(new ThreadStart(Choice));
                myThread.Start();
                //myThread.Join();
            }
        }

        public void Del(string path)
        {
            System.IO.DirectoryInfo di = new DirectoryInfo(path);
            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete(); 
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                 dir.Delete(true); 
            }
        }
                        

        public void CopyTask(string sourceFolder, string destFolder)
        {
            if (!Directory.Exists(destFolder))
                Directory.CreateDirectory(destFolder);

            foreach (string file in Directory.GetFiles(sourceFolder))
            {
                
                this.EnqueueTask(() => File.Copy(file, Path.Combine(destFolder, Path.GetFileName(file))));
            }
            foreach (string folder in Directory.GetDirectories(sourceFolder))
                CopyTask(folder, Path.Combine(destFolder, Path.GetFileName(folder)));
        }

        public void Choice()
        {
            while(true)
            {
                if (que.Count != 0)
                {
                    TaskDelegate task = null;
                    lock (obj)
                    {
                        if (que.Count != 0)
                        {
                            task = que.Dequeue();
                            
                        }
                    }
                    if (task != null)
                    {
                        task();
                        Interlocked.Increment(ref myPath.count);
                    }
                }
                else
                {
                    
                    break;
                }
                    
            }
        }



        public void EnqueueTask(TaskDelegate task)
        {
            que.Enqueue(task);
        }
    }
}
