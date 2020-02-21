using System;
using System.Collections;
using System.Collections.Generic;

//Создать на языке C# обобщенный (generic-) класс DynamicList<T>, который:
//- реализует динамический массив с помощью обычного массива T[];
//- имеет свойство Count, показывающее количество элементов; 
//- имеет свойство Items для доступа к элементам по индексу; 
//- имеет методы Add, Remove, RemoveAt, Clear для соответственно добавления, удаления, удаления по индексу и удаления всех элементов;
//- реализует интерфейс IEnumerable<T>.
//Реализовать простейший пример использования класса DynamicList<T> на языке C#.

namespace ConsoleApp1
{
    class DynamicList<T> : IEnumerable<T>
    {
        private T[] arr;
        private int nextInd = 0;

        public void Add(T value)
        {
            SetLength();
            arr[nextInd] = value;
            nextInd++;
        }

        public void Remove(T value)
        {
            RemoveAt(Array.IndexOf(arr, value));
        }

        public void RemoveAt(int index)
        {
            var newContainer = new T[arr.Length - 1];
            Array.Copy(arr, newContainer, index);
            Array.Copy(arr, index + 1, newContainer, index, arr.Length - index - 1);
            arr = newContainer;
            nextInd--;
        }

        public void Clear()
        {
            arr = null;
            nextInd = 0;
        }

        public int Count
        {
            get { return nextInd; }
        }

        public T this[int index]
        {
            get { return arr[index]; }
            set { arr[index] = value; }
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < nextInd; i++)
            {
                yield return arr[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void SetLength()
        {
            if (arr == null)
            {
                arr = new T[16];
            }
            if (nextInd == arr.Length)
            {
                var newContainer = new T[arr.Length + 16];
                Array.Copy(arr, newContainer, arr.Length);
                arr = newContainer;
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            DynamicList<int> dynamicList = new DynamicList<int>();
            for (int i = 0; i <= 100; i++)
            {
                dynamicList.Add(i);
            }

            foreach (int element in dynamicList)
            {
                Console.Write(element + " ");
            }
            Console.WriteLine();


            Console.WriteLine("Current count = " + dynamicList.Count + "\n");

            for (int i = 20; i <= 60; i++)
            {
                dynamicList.Remove(i);
            }

            foreach (int element in dynamicList)
            {
                Console.Write(element + " ");
            }
            Console.WriteLine();
            
            Console.WriteLine("Current count = " + dynamicList.Count);

            Console.ReadKey();
        }
    }
}