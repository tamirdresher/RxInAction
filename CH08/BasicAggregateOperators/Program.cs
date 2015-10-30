using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using Helpers;

namespace BasicAggregateOperators
{
    class Program
    {
        static void Main(string[] args)
        {
            Sum();
            SumWithNulls();
            Count();
            CountWithPredicate();
            Average();
            MaxAndMin();
            MaxWithSelector();
            MaxByAndMinBy();
            AggregateAndScan();
            SecondLargestItemWithAggregate();
            Console.ReadLine();
        }

        private static void SecondLargestItemWithAggregate()
        {
            Demo.DisplayHeader("The Aggregate operators - the resultSelector function transforms the final accumulator value into the result value");

Subject<int> numbers = new Subject<int>();

numbers.Aggregate(
    new SortedSet<int>(),
    (largest, item) =>
    {
        largest.Add(item);
        if (largest.Count > 2)
        {
            largest.Remove(largest.First()); //keeping only the first two largest items
        }
        return largest;
    },
    largest => largest.FirstOrDefault()) //since the collection is sorted and contain two items at most, the first items is the second largest one 
    .SubscribeConsole();

numbers.OnNext(3);
numbers.OnNext(1);
numbers.OnNext(4);
numbers.OnNext(2);
numbers.OnCompleted();

        }

        private static void AggregateAndScan()
        {
            Demo.DisplayHeader("The Aggregate and Scan operators - apply a function to each item emitted by an Observable, sequentially, and emit the final value (aggregate) or each computed value (scan) ");

            Observable.Range(1, 5)
                .Aggregate(1,
                          (accumelate, currItem) => accumelate * currItem)
                .SubscribeConsole("Aggregate");

            Observable.Range(1, 5)
                .Scan(1,
                      (accumelate, currItem) => accumelate * currItem)
                .SubscribeConsole("Scan");
        }

        private static void MaxByAndMinBy()
        {
            Demo.DisplayHeader("The MaxBy and MinBy operators - the provided selector will determine the maximal/minimal object based on the selected value");

            Subject<StudentGrade> grades = new Subject<StudentGrade>();
            grades.MaxBy(s => s.Grade)
                .SelectMany(max => max)
                .SubscribeConsole("Maximal object by grade");

            grades.OnNext(new StudentGrade() { Id = "1", Name = "A", Grade = 85.0 });
            grades.OnNext(new StudentGrade() { Id = "2", Name = "B", Grade = 90.0 });
            grades.OnNext(new StudentGrade() { Id = "3", Name = "C", Grade = 80.0 });
            grades.OnCompleted();
        }

        private static void MaxWithSelector()
        {
            Demo.DisplayHeader("The Max and Min operators - passing a selector will emit the maximal/minimal from the values produced by the selector");

            Subject<StudentGrade> grades = new Subject<StudentGrade>();
            grades.Max(g => g.Grade)
                .SubscribeConsole("Maximal grade");

            grades.OnNext(new StudentGrade() { Id = "1", Name = "A", Grade = 85.0 });
            grades.OnNext(new StudentGrade() { Id = "2", Name = "B", Grade = 90.0 });
            grades.OnNext(new StudentGrade() { Id = "3", Name = "C", Grade = 80.0 });
            grades.OnCompleted();
        }

        private static void MaxAndMin()
        {
            Demo.DisplayHeader("The Max and Min operators");

            Observable.Range(1, 5)
                .Max()
                .SubscribeConsole("Max");
            Observable.Range(1, 5)
               .Min()
               .SubscribeConsole("Min");
        }

        private static void Average()
        {
            Demo.DisplayHeader("The Average operator");

            Observable.Range(1, 5)
                .Average()
                .SubscribeConsole("Average");
        }

        private static void CountWithPredicate()
        {
            Demo.DisplayHeader("The Count operator - you can also specify a predicate that will filter some of values from being counted");

            Observable.Range(1, 5)
                .Count(x => x % 2 == 0)
                .SubscribeConsole("Count of even numbers");
        }

        private static void Count()
        {
            Demo.DisplayHeader("The Count operator");

            Observable.Range(1, 5)
                .Count()
                .SubscribeConsole("Count");
        }

        private static void SumWithNulls()
        {
            Demo.DisplayHeader("The Sum operator - works on nullable types as well");

            var numbers = new Subject<int?>();
            numbers
                .Sum()
                .SubscribeConsole("SumWithNull");

            numbers.OnNext(1);
            numbers.OnNext(2);
            numbers.OnNext(null);
            numbers.OnNext(3);
            numbers.OnNext(null);
            numbers.OnNext(4);
            numbers.OnNext(5);
            numbers.OnCompleted();


        }

        private static void Sum()
        {
            Demo.DisplayHeader("The Sum operator");

            Observable.Range(1, 5)
                .Sum()
                .SubscribeConsole("Sum");
        }

    }
}
