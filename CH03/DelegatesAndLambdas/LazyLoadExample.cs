using System;

namespace DelegatesAndLambdas
{
    class LazyLoadExample
    {
        class HeavyClass
        {
            //this is a heavy class that take long time to create
        }

        class ThinClass
        {
            private HeavyClass _heavy;
            public HeavyClass TheHeavy
            {
                get
                {
                    if (_heavy == null)
                    {
                        _heavy = new HeavyClass();
                    }
                    return _heavy;
                }
            }

            public void SomeMethod()
            {
                var myHeavy = TheHeavy;

                //Rest of code the use myHeavy
            }
        }

        class ClassWithLazy
        {
            Lazy<HeavyClass> _lazyHeavyClass = new Lazy<HeavyClass>(() =>
              {
                  var heavy = new HeavyClass();

                  //code that initialize the heavy object

                  return heavy;
              });

            public void SomeMethod()
            {
                var myHeavy = _lazyHeavyClass.Value;

                //Rest of code the use myHeavy
            }
        }
    }
}