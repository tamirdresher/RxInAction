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
                    if (this._heavy == null)
                    {
                        this._heavy = new HeavyClass();
                    }
                    return this._heavy;
                }
            }

            public void SomeMethod()
            {
                HeavyClass myHeavy = this.TheHeavy;

                //Rest of code the use myHeavy
            }
        }

        class ClassWithLazy
        {
            readonly Lazy<HeavyClass> _lazyHeavyClass = new Lazy<HeavyClass>(() => {
                var heavy = new HeavyClass();

                //code that initialize the heavy object

                return heavy;
            });

            public void SomeMethod()
            {
                HeavyClass myHeavy = this._lazyHeavyClass.Value;

                //Rest of code the use myHeavy
            }
        }
    }
}
