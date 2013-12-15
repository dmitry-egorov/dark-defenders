using System;

namespace Infrastructure.Util
{
    public static class ActionObserverExtensions
    {
        public static IDisposable Subscribe<T>(this IObservable<T> observable, Action<T> action)
        {
            return observable.Subscribe(new ActionObserver<T>(action));
        }
    }
}