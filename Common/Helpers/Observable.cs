namespace System.Reactive.Linq
{
    public static partial class ObservableExtensionsHelpers
    {
        public static IObservable<T> FromValues<T>(params T[] values)
        {
            return values.ToObservable();
        }
    }
}
