using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ExtensionMethodsExample
{
    class FluentInterfacesExample
    {
        public static void StringBuilderExample()
        {
            StringBuilder sbuilder = new StringBuilder();
            var result = sbuilder
                .AppendLine("Fluent")
                .AppendLine("Interfaces")
                .AppendLine("Are")
                .AppendLine("Awesome")
                .ToString();
            Console.WriteLine(result);
        }

        public static void StandardAddToList()
        {
            var words = new List<string>();
            words.Add("This");
            words.Add("Feels");
            words.Add("Weird");


        }
        public static void FluentAddToList()
        {
            var words = new List<string>();
            words.AddItem("This")
                .AddItem("Feels")
                .AddItem("Weird");


        }
        public static void FluentAddToAllLists()
        {
            ICollection<string> words = new List<string>();
            words = new Collection<string>();
            words = new HashSet<string>();
            words.AddItem("This")
                .AddItem("Feels")
                .AddItem("Weird");
        }

    }

    public static class ListExtensions
    {
        public static List<T> AddItem<T>(this List<T> list, T item)
        {
            list.Add(item);
            return list;
        }

public static ICollection<T> AddItem<T>(this ICollection<T> list, T item)
{
    list.Add(item);
    return list;
}
    }
}