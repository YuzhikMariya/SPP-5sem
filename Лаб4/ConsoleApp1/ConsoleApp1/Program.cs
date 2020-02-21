using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using A;
using ClassLibrary1;

namespace B
{
    [ExportClass]
    public class PublicClass1 { }
    [ExportClass]
    public class PublicClass2 { }

    public enum PublicEnum { };

    internal class InternalClass { }
}

namespace A
{
    static class Program
    {
        static void ListTypesInAssembly(Assembly assembly)
        {
            var types = assembly.GetTypes().Where(t => t.IsPublic).OrderBy(tp => tp.FullName);     
              
            foreach (Type type in types)
            {
                
                Console.WriteLine(type.FullName);
            }
        }

        static void Main(string[] args)
        {
            Assembly assembly = Assembly.LoadFrom(@"C:\Windows\Microsoft.NET\assembly\GAC_32\mscorlib\v4.0_4.0.0.0__b77a5c561934e089\mscorlib.dll");
            ListTypesInAssembly(assembly);
            Console.ReadKey();
        }
    }



    [ExportClass]
    public class PublicClass1 { }
    public class PublicClass2 { }

    public enum PublicEnum { };

    internal class InternalClass { }
}