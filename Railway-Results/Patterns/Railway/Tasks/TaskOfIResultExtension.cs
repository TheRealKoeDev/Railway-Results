using KoeLib.Patterns.Railway.Results;
using KoeLib.Patterns.Railway.Tools;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace KoeLib.Patterns.Railway.Tasks
{
    [DebuggerStepThrough]
    public static class TaskOfIResultExtension
    {
        public static Task<TResult> Async<TResult>(this TResult result)
           where TResult : IResult
           => Task.FromResult(result);

        public static Task<TResult> Then<TResult>(this Task<TResult> target, Action action)
           where TResult : IResult
        {
            Args.ExceptionIfNull(action, nameof(action));
            return target.ContinueWith(result => result.Result.Then(action));
        }

        public static Task<TNewResult> Then<TResult, TNewResult>(this Task<TResult> target, Func<TNewResult> nextResult)
           where TResult : IResult
           where TNewResult : IResult
        {
            Args.ExceptionIfNull(nextResult, nameof(nextResult));
            return target.ContinueWith(result => result.Result.Then(nextResult));
        }

        public static Task<TNewResult> Then<TResult, TNewResult>(this Task<TResult> target, Func<TResult, TNewResult> newResultFunc)
            where TResult : IResult
            where TNewResult : IResult
        {
            Args.ExceptionIfNull(newResultFunc, nameof(newResultFunc));
            return target.ContinueWith(result => result.Result.Then(newResultFunc));
        }

        public static Task<TryCatchResult<T>> Try<TResult, T>(this Task<TResult> target, Func<TResult, T> resultFunc)
           where TResult : IResult
        {
            Args.ExceptionIfNull(resultFunc, nameof(resultFunc));
            return target.ContinueWith(result => result.Result.Try(resultFunc));
        }

        public static Task<T> Catch<T>(this Task<TryCatchResult<T>> target, Func<Exception, T> onException)
        {
            Args.ExceptionIfNull(onException, nameof(onException));
            return target.ContinueWith(task => task.Result.Catch(onException));
        }
    }
}
