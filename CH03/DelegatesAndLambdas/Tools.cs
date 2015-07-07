using System;
using System.Collections.Generic;

namespace DelegatesAndLambdas
{
    static class Tools
    {
        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            //ForEach implementation
            foreach (var item in collection)
            {
                action(item);
            }
        }
       
        public static void ForEachInt(IEnumerable<int> collection, Action<int> action)
        {
            foreach (var item in collection)
            {
                action(item);
            }
        }

        public static void ForEach<T>(IEnumerable<T> collection, Action<T> action, Func<T, bool> predicate)
        {
            foreach (var item in collection)
            {
                if (predicate(item))
                {
                    action(item);
                }
            }
        }


    }
}