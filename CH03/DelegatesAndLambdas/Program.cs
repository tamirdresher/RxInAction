using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using ExtensionMethodsExample;
namespace DelegatesAndLambdas
{
    class Program
    {

        public delegate bool ComparisonTest(string first, string second);
        public delegate void ActionDelegate();


        static void Main(string[] args)
        {
            int meaningOfLife = 42;

            Console.WriteLine("is the meaning of life even:{0}", meaningOfLife.IsEven());
            UsingDelegates();
            MethodAsParameter();
            AnonymousMethod();
            CapturedVariables();
            TrickyCapturedVariables();
            IntroducingLambdas();
            UsingFuncAndAction();
            MethodWithAction();
            ComparerExample.BetterIComparable();
            FluentInterfacesExample.StringBuilderExample();
        }




        private static void MethodWithAction()
        {
            var oddNumbers = new[] { 1, 3, 5, 7, 9 };
            Tools.ForEachInt(oddNumbers, n => Console.WriteLine(n));

            Tools.ForEach(new[] { 1, 2, 3 }, n => Console.WriteLine(n));
            Tools.ForEach(new[] { "a", "b", "c" }, n => Console.WriteLine(n));
            Tools.ForEach(new[] { ConsoleColor.Red, ConsoleColor.Green, ConsoleColor.Blue }, n => Console.WriteLine(n));
        }


        private static void MethodWithFunc()
        {
            var numbers = Enumerable.Range(1, 10);
            Tools.ForEach(numbers, n => Console.WriteLine(n), n => (n % 2 == 0));


        }


        private static void IntroducingLambdas()
        {
            ComparisonTest x = (s1, s2) => s1 == s2;
            ComparisonTest y = delegate (string s1, string s2) { return s1 == s2; };


        }

        private static void MethodAsParameter()
        {
            string[] cities, friends;

            //Passing method as parameter
            cities = new[] { "London", "Madrid", "TelAviv" };
            friends = new[] { "Minnie", "Goofey", "MickeyM" };
            Console.WriteLine("Are friendss and cities similar? {0}", AreSimilar(friends, cities, StringComparators.CompareLength));
        }

        private static void UsingDelegates()
        {
            string s1 = "Hello";
            string s2 = "World";
            var comparators = new StringComparators();

            ComparisonTest test = new ComparisonTest(comparators.CompareContent);
            Console.WriteLine("CompareContent returned: {0}", test(s1, s2));

            test = new ComparisonTest(StringComparators.CompareLength);
            Console.WriteLine("CompareLength returned: {0}", test(s1, s2));

            ComparisonTest test2 = comparators.CompareContent;
        }

        public delegate bool NameValidator(string name);
        public delegate bool EmailValidator(string email);
        private static void SameDelegateDifferentName()
        {
            NameValidator nameValidator = (name) => name.Length > 3;
            EmailValidator emailValidator = (email) => email.Length > 3;
            emailValidator = (email) => email.Contains("@");

            //wont work
            //nameValidator = emailValidator;

        }

        private static void UsingFuncAndAction()
        {
            string s1 = "Hello";
            string s2 = "World";
            var comparators = new StringComparators();

            Func<string, string, bool> test = comparators.CompareContent;
            Console.WriteLine("CompareContent returned: {0}", test(s1, s2));

            test = StringComparators.CompareLength;
            Console.WriteLine("CompareLength returned: {0}", test(s1, s2));

            test = (input1, input2) => (input1.Length % 2) == (input1.Length % 2);
            Console.WriteLine("Modulo compare with lambda returned: {0}", test(s1, s2));

            Action<string, string> actionExample = (input1, input2) => Console.WriteLine("input1: {0} input2: {1}", input1, input2);
            actionExample(s1, s2);
        }


        private static void AnonymousMethod()
        {
            string[] cities = new[] { "London", "Madrid", "TelAviv" };
            string[] friends = new[] { "Minnie", "Goofey", "MickeyM" };

            //Anonymous Method
            ComparisonTest lengthComparer = delegate (string first, string second)
            {
                return first.Length == second.Length;
            };
            Console.WriteLine("anonymous method returned: {0}", lengthComparer("Hello", "World"));
            PassingAnonymousMethodAsArgument(cities, friends);
        }

        private static void TrickyCapturedVariables()
        {
            //Captured Variables are tricky
            var actions = new List<ActionDelegate>();
            for (var i = 0; i < 5; i++)
            {
                actions.Add(delegate () { Console.WriteLine(i); });
            }
            foreach (var act in actions) act();
        }

        private static void CapturedVariables()
        {
            ComparisonTest comparer;
            {
                int moduloBase = 2;
                comparer = delegate (string s1, string s2)
                {
                    Console.WriteLine("the modulo base is: {0}", moduloBase);
                    return ((s1.Length % moduloBase) == (s2.Length % moduloBase));
                };
                moduloBase = 3;
            }
            var similarByMod = AreSimilar(new[] { "AB" }, new[] { "ABCD" }, comparer);

            Console.WriteLine("Similar by modulo: {0}", similarByMod);
        }

        private static bool PassingAnonymousMethodAsArgument(string[] cities, string[] friends)
        {
            //Passing anonymous method as argument
            AreSimilar(friends, cities, delegate (string s1, string s2) { return s1 == s2; });

            int moduloBase = 2;
            var similarByMod = AreSimilar(friends, cities, delegate (string str1, string str2)
            {
                return ((str1.Length % moduloBase) == (str2.Length % moduloBase));
            });
            Console.WriteLine("Similar by modulo: {0}", similarByMod);
            return similarByMod;
        }

        static bool AreSimilar(string[] leftItems, string[] rightItems, ComparisonTest tester)
        {
            if (leftItems.Length != rightItems.Length)
                return false;

            for (int i = 0; i < leftItems.Length; i++)
            {
                if (tester(leftItems[i], rightItems[i]) == false)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
