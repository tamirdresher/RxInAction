using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using Helpers;

namespace Joins
{
    class Program
    {
        static void Main(string[] args)
        {
            Join();
            GroupJoin();
            Console.WriteLine("Press any key to continue");
            Console.ReadLine();
        }
        
        private static void GroupJoin()
        {
            Demo.DisplayHeader("The GroupJoin operator - correlates elements from two observables based on overlapping duration windows and put them in a correlation group");

            Subject<DoorOpened> doorOpenedSubject = new Subject<DoorOpened>();
            IObservable<DoorOpened> doorOpened = doorOpenedSubject.AsObservable();

            var enterences = doorOpened.Where(o => o.Direction == OpenDirection.Entering);
            var maleEntering = enterences.Where(x => x.Gender == Gender.Male);
            var femaleEntering = enterences.Where(x => x.Gender == Gender.Female);

            var exits = doorOpened.Where(o => o.Direction == OpenDirection.Leaving);
            var maleExiting = exits.Where(x => x.Gender == Gender.Male);
            var femaleExiting = exits.Where(x => x.Gender == Gender.Female);

            var malesAcquaintances =
                maleEntering
                    .GroupJoin(femaleEntering,
                        male => maleExiting.Where(exit => exit.Name == male.Name),
                        female => femaleExiting.Where(exit => female.Name == exit.Name),
                        (m, females) => new { Male = m.Name, Females = females });

            var amountPerUser =
                from acquinteces in malesAcquaintances
                from cnt in acquinteces.Females.Scan(0, (acc, curr) => acc + 1)
                select new { acquinteces.Male, cnt };

            amountPerUser.SubscribeConsole("Amount of meetings per User");

            //
            // Using Query Syntax GroupJoin clause
            //
            var malesAcquaintances2 =
            from male in maleEntering
            join female in femaleEntering on maleExiting.Where(exit => exit.Name == male.Name) equals
                femaleExiting.Where(exit => female.Name == exit.Name)
                into females
            select new { Male = male.Name, Females = females };
            var amountPerUser2 =
               from acquinteces in malesAcquaintances2
               from cnt in acquinteces.Females.Scan(0, (acc, curr) => acc + 1)
               select new { acquinteces.Male, cnt };

            //amountPerUser2.SubscribeConsole("Amount of meetings per User (query syntax)");

            //This is the sequence you see in Figure 9.8
            doorOpenedSubject.OnNext(new DoorOpened("Bob", Gender.Male, OpenDirection.Entering));
            doorOpenedSubject.OnNext(new DoorOpened("Sara", Gender.Female, OpenDirection.Entering));
            doorOpenedSubject.OnNext(new DoorOpened("John", Gender.Male, OpenDirection.Entering));
            doorOpenedSubject.OnNext(new DoorOpened("Sara", Gender.Female, OpenDirection.Leaving));
            doorOpenedSubject.OnNext(new DoorOpened("Fibi", Gender.Female, OpenDirection.Entering));
            doorOpenedSubject.OnNext(new DoorOpened("Bob", Gender.Male, OpenDirection.Leaving));
            doorOpenedSubject.OnNext(new DoorOpened("Dan", Gender.Male, OpenDirection.Entering));
            doorOpenedSubject.OnNext(new DoorOpened("Fibi", Gender.Female, OpenDirection.Leaving));
            doorOpenedSubject.OnNext(new DoorOpened("John", Gender.Male, OpenDirection.Leaving));
            doorOpenedSubject.OnNext(new DoorOpened("Dan", Gender.Male, OpenDirection.Leaving));


        }

        private static void Join()
        {
            Demo.DisplayHeader("The Join operator - ...");

            Subject<DoorOpened> doorOpenedSubject = new Subject<DoorOpened>();
            IObservable<DoorOpened> doorOpened = doorOpenedSubject.AsObservable();

            var enterences = doorOpened.Where(o => o.Direction == OpenDirection.Entering);
            var maleEntering = enterences.Where(x => x.Gender == Gender.Male);
            var femaleEntering = enterences.Where(x => x.Gender == Gender.Female);

            var exits = doorOpened.Where(o => o.Direction == OpenDirection.Leaving);
            var maleExiting = exits.Where(x => x.Gender == Gender.Male);
            var femaleExiting = exits.Where(x => x.Gender == Gender.Female);


            //Using Method Chaining apporach
            maleEntering
                .Join(femaleEntering,
                    male => maleExiting.Where(exit => exit.Name == male.Name),
                    female => femaleExiting.Where(exit => female.Name == exit.Name),
                    (m, f) => new { Male = m.Name, Female = f.Name })
                .SubscribeConsole("Together At Room");

            //
            //Using Query Syntax Join clause
            //
            var test =
                from male in maleEntering
                join female in femaleEntering on maleExiting.Where(exit => exit.Name == male.Name) equals
                    femaleExiting.Where(exit => female.Name == exit.Name)
                select new { MName = male.Name, FName = female.Name };
            test.SubscribeConsole("Together At Room (query syntax)");

            //This is the sequence you see in Figure 9.8
            doorOpenedSubject.OnNext(new DoorOpened("Bob", Gender.Male, OpenDirection.Entering));
            doorOpenedSubject.OnNext(new DoorOpened("Sara", Gender.Female, OpenDirection.Entering));
            doorOpenedSubject.OnNext(new DoorOpened("John", Gender.Male, OpenDirection.Entering));
            doorOpenedSubject.OnNext(new DoorOpened("Sara", Gender.Female, OpenDirection.Leaving));
            doorOpenedSubject.OnNext(new DoorOpened("Fibi", Gender.Female, OpenDirection.Entering));
            doorOpenedSubject.OnNext(new DoorOpened("Bob", Gender.Male, OpenDirection.Leaving));
            doorOpenedSubject.OnNext(new DoorOpened("Dan", Gender.Male, OpenDirection.Entering));
            doorOpenedSubject.OnNext(new DoorOpened("Fibi", Gender.Female, OpenDirection.Leaving));
            doorOpenedSubject.OnNext(new DoorOpened("John", Gender.Male, OpenDirection.Leaving));
            doorOpenedSubject.OnNext(new DoorOpened("Dan", Gender.Male, OpenDirection.Leaving));

        }

    }
}
