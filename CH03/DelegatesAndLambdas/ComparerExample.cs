using System;
using System.Collections.Generic;

namespace DelegatesAndLambdas
{
    public class ComparerExample
    {
        private class LengthComparer : IComparer<string>
        {
            public int Compare(string x, string y)
            {
                if (x.Length == y.Length)
                {
                    return 0;
                }
                return (x.Length > y.Length) ? 1 : -1;
            }
        }

        private class GenricComparer<T> : IComparer<T>
        {
            private Func<T, T, int> CompareFunc { get; set; }

            public GenricComparer(Func<T, T, int> compareFunc)
            {
                CompareFunc = compareFunc;
            }

            public int Compare(T x, T y)
            {
                return CompareFunc(x, y);
            }
        }

        public static void BetterIComparable()
        {
            var numbers = new List<string> { "ab", "a", "aabb", "abc" };
            numbers.Sort(new LengthComparer());
            Tools.ForEach(numbers, Console.WriteLine);

            numbers = new List<string> { "ab", "a", "aabb", "abc" };
            numbers.Sort(new GenricComparer<string>((x, y) =>
                (x.Length == y.Length)
                    ? 0
                    : (x.Length > y.Length) ? 1 : -1));
            Tools.ForEach(numbers, Console.WriteLine);


        }
    }
}