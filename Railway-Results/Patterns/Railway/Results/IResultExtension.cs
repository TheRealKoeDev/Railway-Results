using KoeLib.Patterns.Railway.Tools;
using System;
using System.Diagnostics;

namespace KoeLib.Patterns.Railway.Results
{
    [DebuggerStepThrough]
    public static class IResultExtension
    {
        public static TResult Then<TResult>(this TResult target, Action action)
           where TResult : IResult
        {
            Args.ExceptionIfNull(action, nameof(action));
            action();
            return target;
        }

        public static TNewResult Then<TResult, TNewResult>(this TResult _, Func<TNewResult> nextResult)
           where TResult : IResult
           where TNewResult : IResult
        {
            Args.ExceptionIfNull(nextResult, nameof(nextResult));
            return nextResult();
        }


        public static TNewResult Then<TResult, TNewResult>(this TResult target, Func<TResult, TNewResult> func)
            where TResult : IResult
            where TNewResult : IResult
        {
            Args.ExceptionIfNull(func, nameof(func));
            return func(target);
        }


        public static TResult Keep<T, TResult>(this TResult target, Func<T> keepFunc, out T kept)
            where TResult: IResult
        {
            Args.ExceptionIfNull(keepFunc, nameof(keepFunc));
            kept = keepFunc();
            return target;
        }

        public static TryCatchResult<T> Try<TResult, T>(this TResult result, Func<TResult, T> resultFunc)
           where TResult : IResult
        {
            Args.ExceptionIfNull(resultFunc, nameof(resultFunc));
            return new TryCatchResult<T>(() => resultFunc(result));
        }
    }
}
