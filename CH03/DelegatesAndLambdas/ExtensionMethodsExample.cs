using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DelegatesAndLambdas;

namespace ExtensionMethodsExample
{
    static class IntExtensions
    {
        public static bool IsEven(this int number)
        {
            return number % 2 == 0;
        }
    }

    static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }
    }

    class ExtensionMethodsExample
    {
        public void ForEachExample()
        {
            var numbers = Enumerable.Range(1, 10);
            numbers.ForEach(x => Console.WriteLine(x));

        }

        public void WorkingWithNulls()
        {
            string str = null;
            Console.WriteLine("is str empty: {0}", string.IsNullOrEmpty(str));
            Console.WriteLine("is str empty: {0}", str.IsNullOrEmpty());
        }
    }
}
