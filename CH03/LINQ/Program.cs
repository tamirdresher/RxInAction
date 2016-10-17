using System;
using System.Collections.Generic;
using System.Linq;

namespace LINQExamples
{

    class Program
    {
        static void Main(string[] args)
        {
            IntroToLinq.Run();
            Console.ReadLine();
        }
    }

    class IntroToLinq
    {
        public static void Run()
        {
            SimpleQueryOnList();
            SimpleQueryOnListAsQueryExpression();
            NestedQueryExample();
            AnonynmousTypeExample();
            QueryWithAnonymousType();
            AnonymousTypeVsTupleExample();
            DeferredExecutionExample();
            UnderstandingYieldExample();
            FibonacciWithYieldExample();
            ModifiedWhereToExplainDeferedExecutionExample();
            DynamicLINQQueryExample();
        }

        private static void DynamicLINQQueryExample()
        {
            var numbers = new[] { 1, 2, 3, 4, 5, 6 };
            var query = numbers.Where(x => x % 2 == 0);
            if (/*some condition*/true)
            {
                query = query.Where(x => x > 5);
            }
            if (/*another condition*/true)
            {
                query = query.Where(x => x > 7);
            }
            foreach (var item in query)
            {
                Console.WriteLine(item);
            }
        }


        private static void ModifiedWhereToExplainDeferedExecutionExample()
        {
            Console.WriteLine("ModifiedWhereToExplainDeferedExecutionExample");
            var numbers = new[] { 1, 2, 3, 4, 5, 6 };
            var evenNumbers = numbers.WhereWithLog(x => x % 2 == 0);
            Console.WriteLine("before foreach");
            foreach (var number in evenNumbers)
            {
                Console.WriteLine("evenNumber:{0}", number);
            }
        }

        static IEnumerable<int> GenerateFibonacci()
        {
            int a = 0;
            int b = 1;
            yield return a;
            yield return b;
            while (true)
            {
                b = a + b;
                a = b - a;
                yield return b;
            }
        }
        private static void FibonacciWithYieldExample()
        {
            Console.WriteLine("10 Fibonacci items");
            foreach (var item in GenerateFibonacci().Take(10))
            {
                Console.WriteLine(item);
            }
        }

        static IEnumerable<string> GetGreetings()
        {
            yield return "Hello";
            yield return "Hi";
        }
        private static void UnderstandingYieldExample()
        {
            foreach (var greeting in GetGreetings())
            {
                Console.WriteLine(greeting);
            }
        }

        private static void DeferredExecutionExample()
        {
            var numbers = new List<int> { 1, 2, 3, 4 };
            var evenNumbers =
                from number in numbers
                where number % 2 == 0
                select number;

            numbers.Add(6);

            foreach (var number in evenNumbers)
            {
                Console.WriteLine(number);
            }
            Console.WriteLine();
        }

        private static void AnonymousTypeVsTupleExample()
        {
            var anonObj = new { Name = "Bugs Bunny", Birthday = DateTime.Today };
            //Tuple<string,DateTime> tuple=new Tuple<string, DateTime>("Bugs Bunny", DateTime.Today);
            Tuple<string, DateTime> tuple = Tuple.Create("Bugs Bunny", DateTime.Today);

            var theDateTime = tuple.Item2;
        }

        private static void QueryWithAnonymousType()
        {
            var authors = new[] { new Author(1, "Tamir Dresher"), new Author(2, "John Skeet"), };
            var books = new[] { new Book("Rx in Action", 1), new Book("C# in Depth", 2), new Book("Real-World Functional Programming", 2), };

            var authorsBooks =
                from author in authors
                from book in books
                where book.AuthorID == author.ID
                select new { author, book };

            foreach (var authorBook in authorsBooks)
            {
                Console.WriteLine("{0} wrote the book: {1}", authorBook.author.Name, authorBook.book.Name);
            }
        }

        private static void AnonynmousTypeExample()
        {
            var anonObj = new { Name = "Bugs Bunny", Birthday = DateTime.Today };
            var anonObj2 = new { Name = "Bugs Bunny", Birthday = DateTime.Today };
            if (anonObj.GetType() == anonObj2.GetType())
            {
                Console.WriteLine("anonObj type == anonObj2 type");

            }
        }

        private static void NestedQueryExample()
        {
            var authors = new[] { new Author(1, "Tamir Dresher"), new Author(2, "John Skeet"), };
            var books = new[] { new Book("Rx in Action", 1), new Book("C# in Depth", 2), new Book("Real-World Functional Programming", 2), };

            var authorsBooks =
                from author in authors
                from book in books
                where book.AuthorID == author.ID
                select author.Name + " wrote the book: " + book.Name;

            foreach (var authorBooks in authorsBooks)
            {
                Console.WriteLine(authorBooks);
            }


        }



        private static void SimpleQueryOnListAsQueryExpression()
        {
            var numbers = new List<int> { 1, 35, 22, 6, 10, 11 };
            var result =
                from number in numbers
                where number % 2 == 1
                where number > 10
                orderby number
                select number + 2;

            var distinct = result.Distinct();

            foreach (var number in distinct)
            {
                Console.Write("{0}", number);
            }
            Console.WriteLine();
        }

        private static void SimpleQueryOnList()
        {
            var numbers = new List<int> { 1, 35, 22, 6, 10, 11 };
            var result = numbers.Where(x => x % 2 == 1)
                .Where(x => x > 10)
                .Select(x => x + 2)
                .Distinct()
                .OrderBy(x => x);

            foreach (var number in result)
            {
                Console.Write("{0}", number);
            }
            Console.WriteLine();
        }
    }
}
