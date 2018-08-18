using DelegatesAndLambdas;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExtensionMethodsExample {
    static class IntExtensions {
        public static bool IsEven(this int number) {
            return number % 2 == 0;
        }
    }

    static class StringExtensions {
        public static bool IsNullOrEmpty(this string str) {
            return String.IsNullOrEmpty(str);
        }
    }

    class ExtensionMethodsExample {
        public void ForEachExample() {
            IEnumerable<int> numbers = Enumerable.Range(1, 10);
            numbers.ForEach(x => Console.WriteLine(x));
        }

        public void WorkingWithNulls() {
            string str = null;
            Console.WriteLine("is str empty: {0}", String.IsNullOrEmpty(str));
            Console.WriteLine("is str empty: {0}", str.IsNullOrEmpty());
        }
    }
}
