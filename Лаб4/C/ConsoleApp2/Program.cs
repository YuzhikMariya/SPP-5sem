using System;
using System.Reflection;
using System.Linq;
using ClassLibrary1;

namespace ConsoleApp2
{


    [ExportClass]
    public class PublicClass1 { }
    [ExportClass]
    public class PublicClass2 { }

    public enum PublicEnum { };

    internal class InternalClass { }

    class Program
    {
        static void Main(string[] args)
        {
        }
    }
}
