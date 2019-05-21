using KoeLib.Patterns.Railway.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace KoeLib.Patterns.Railway.Results
{
    [DebuggerStepThrough]
    public static class ResultExtension
    {
        public static TResult BindEither<TValueError, TResult>(this Result<TValueError, TValueError> target, Func<TValueError, TResult> func)
            where TResult : IResult
            => target.Bind(func, func);

        public static TResult Do<TResult>(this TResult result, Action action)
           where TResult : IResult
        {
            Args.ExceptionIfNull(action, nameof(action));
            action();
            return result;
        }

        public static Result<TValueError, TValueError> Either<TValueError>(this Result<TValueError, TValueError> target, Action<TValueError> func)
            => target.Either(func, func);

        public static Result<TNewValueError, TNewValueError> Either<TValueError, TNewValueError>(this Result<TValueError, TValueError> target, Func<TValueError, TNewValueError> func)
            => target.Either(func, func);

        public static TResult Keep<T, TResult>(this TResult target, Func<T> keepFunc, out T kept)
            where TResult: IResult
        {
            Args.ExceptionIfNull(keepFunc, nameof(keepFunc));
            kept = keepFunc();
            return target;
        }

        public static Result<TValueError, TValueError> KeepEither<T, TValueError>(this Result<TValueError, TValueError> target, Func<TValueError, T> keepFunc, out T kept)
        {
            target.KeepEither(keepFunc, keepFunc, out T bothCases);
            kept = bothCases;
            return target;
        }

        public static T MatchEither<T, TValueError>(this Result<TValueError, TValueError> target, Func<TValueError, T> func)
            => target.Match(func, func);

        public static TryCatchResult<T> Try<TResult, T>(this TResult result, Func<TResult, T> resultFunc)
           where TResult : IResult
        {
            Args.ExceptionIfNull(resultFunc, nameof(resultFunc));
            return new TryCatchResult<T>(() => resultFunc(result));
        }
    }
}
