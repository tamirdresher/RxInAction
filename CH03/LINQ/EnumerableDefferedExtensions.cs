using System;
using System.Collections.Generic;

namespace LINQExamples
{
    static class EnumerableDefferedExtensions
    {
        public static IEnumerable<T> WhereWithLog<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            foreach (var item in source)
            {
                Console.WriteLine("Checking item {0}", item);
                if (predicate(item))
                {
                    yield return item;
                }
            }
        }
    }
}