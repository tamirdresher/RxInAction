using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            string bookTitle = "Reactive Extensions in Action";
            bookTitle.ToUpper();
            Console.WriteLine("Book Title: {0}", bookTitle);
        }
        private static void ImmutableStringFixedExample()
        {
            string bookTitle = "Reactive Extensions in Action";
            string uppercaseTitle = bookTitle.ToUpper();
            Console.WriteLine("Book Title: {0}", uppercaseTitle);
        }

    }
}
