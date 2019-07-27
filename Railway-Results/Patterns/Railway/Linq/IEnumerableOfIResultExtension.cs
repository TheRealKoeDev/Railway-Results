using KoeLib.Patterns.Railway.Results;
using KoeLib.Patterns.Railway.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace KoeLib.Patterns.Railway.Linq
{
    [DebuggerStepThrough]
    public static class IEnumerableOfIResultExtension
    {
        public static IEnumerable<TResult> Then<TResult>(this IEnumerable<TResult> target, Action action)
           where TResult : IResult
        {
            Args.ExceptionIfNull(action, nameof(action));
            action();
            using (IEnumerator<TResult> enumerator = target.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current.Then(action);
                }
            }
        }

        public static IEnumerable<TNewResult> Then<TResult, TNewResult>(this IEnumerable<TResult> target, Func<TNewResult> nextResult)
           where TResult : IResult
           where TNewResult : IResult
        {
            Args.ExceptionIfNull(nextResult, nameof(nextResult));
            using (IEnumerator<TResult> enumerator = target.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current.Then(nextResult);
                }
            }
        }

        public static IEnumerable<TNewResult> Then<TResult, TNewResult>(this IEnumerable<TResult> target, Func<TResult, TNewResult> newResultFunc)
            where TResult : IResult
            where TNewResult : IResult
        {
            Args.ExceptionIfNull(newResultFunc, nameof(newResultFunc));
            using (IEnumerator<TResult> enumerator = target.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current.Then(newResultFunc);
                }
            }
        }

        public static IEnumerable<TryCatchResult<T>> Try<TResult, T>(this IEnumerable<TResult> target, Func<TResult, T> resultFunc)
           where TResult : IResult
        {
            Args.ExceptionIfNull(resultFunc, nameof(resultFunc));
            using (IEnumerator<TResult> enumerator = target.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return  enumerator.Current.Try(resultFunc);
                }
            }           
        }

        public static IEnumerable<T> Catch<T>(this IEnumerable<TryCatchResult<T>> target, Func<Exception, T> onException)
        {
            Args.ExceptionIfNull(onException, nameof(onException));
            using (IEnumerator<TryCatchResult<T>> enumerator = target.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current.Catch(onException);
                }
            }
        }
    }
}
