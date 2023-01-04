using System;

namespace FunctionalThinking
{
    class Program
    {
        static void Main(string[] args)
        {
            //Side Effects
            WriteRedMessage("Side Effects");
            WriteMessage("Side Effects");

            //Immutability
            ImmutableStringExample();
            ImmutableStringFixedExample();
        }

        public static void WriteRedMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
        }

        public static void WriteMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
        }

        private static void ImmutableStringExample()
        {
            var bookTitle = "Reactive Extensions in Action";
            bookTitle.ToUpper();
            Console.WriteLine("Book Title: {0}", bookTitle);
        }

        private static void ImmutableStringFixedExample()
        {
            var bookTitle = "Reactive Extensions in Action";
            var uppercaseTitle = bookTitle.ToUpper();
            Console.WriteLine("Book Title: {0}", uppercaseTitle);
        }
    }
}
