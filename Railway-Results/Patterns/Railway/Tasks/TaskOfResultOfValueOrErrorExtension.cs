using KoeLib.Patterns.Railway.Results;
using KoeLib.Patterns.Railway.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace KoeLib.Patterns.Railway.Tasks
{
    [DebuggerStepThrough]
    public static class TaskOfResultOfValueOrErrorExtension
    {
        public static Task<Result> AsPlainResult<TValue, TError>(this Task<Result<TValue, TError>> target)
        {
            Args.ExceptionIfNull(target, nameof(target));
            return target.ContinueWith(task => task.Result.AsPlainResult());
        }

        public static Task<Result<TValue>> AsResultOfValue<TValue, TError>(this Task<Result<TValue, TError>> target)
        {
            Args.ExceptionIfNull(target, nameof(target));
            return target.ContinueWith(task => task.Result.AsResultOfValue());
        }

        public static Task<ResultOrError<TError>> AsResultOrError<TValue, TError>(this Task<Result<TValue, TError>> target)
        {
            Args.ExceptionIfNull(target, nameof(target));
            return target.ContinueWith(task => task.Result.AsResultOrError());
        }

        public static Task<Result<TValue, TError>> OnSuccess<TValue, TError>(this Task<Result<TValue, TError>> target, Action<TValue> onSuccess)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess));
            return target.ContinueWith(task => task.Result.OnSuccess(onSuccess));
        }

        public static Task<Result<TNewValue, TError>> OnSuccess<TValue, TError, TNewValue>(this Task<Result<TValue, TError>> target, Func<TValue, TNewValue> onSuccess)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess));
            return target.ContinueWith(task => task.Result.OnSuccess(onSuccess));
        }

        public static Task<Result<TNewValue, TError>> Bind<TValue, TError, TNewValue>(this Task<Result<TValue, TError>> target, Func<TValue, Result<TNewValue, TError>> onSuccess)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess));
            return target.ContinueWith(task => task.Result.Bind(onSuccess));
        }

        public static Task<ResultOrError<TError>> Bind<TValue, TError>(this Task<Result<TValue, TError>> target, Func<TValue, ResultOrError<TError>> onSuccess)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess));
            return target.ContinueWith(task => task.Result.Bind(onSuccess));
        }

        public static Task<TResult> Bind<TValue, TError, TResult>(this Task<Result<TValue, TError>> target, Func<TValue, TResult> onSuccess, Func<TError, TResult> onError)
            where TResult : IResult
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess), onError, nameof(onError));
            return target.ContinueWith(task => task.Result.Bind(onSuccess, onError));
        }

        public static Task<Result<TValue, TNewError>> BindOnError<TValue, TError, TNewError>(this Task<Result<TValue, TError>> target, Func<TError, Result<TValue, TNewError>> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onError, nameof(onError));
            return target.ContinueWith(task => task.Result.BindOnError(onError));
        }

        public static Task<Result<TValue>> BindOnError<TValue, TError>(this Task<Result<TValue, TError>> target, Func<TError, Result<TValue>> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onError, nameof(onError));
            return target.ContinueWith(task => task.Result.BindOnError(onError));
        }

        public static Task<Result<TValue, TError>> FixOnError<TValue, TError>(this Task<Result<TValue, TError>> target, Func<TError, TValue> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onError, nameof(onError));
            return target.ContinueWith(task => task.Result.FixOnError(onError));
        }

        public static Task<Result<TValue, TError>> OnError<TValue, TError>(this Task<Result<TValue, TError>> target, Action<TError> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onError, nameof(onError));
            return target.ContinueWith(task => task.Result.OnError(onError));
        }

        public static Task<Result<TValue, TNewError>> OnError<TValue, TError, TNewError>(this Task<Result<TValue, TError>> target, Func<TError, TNewError> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onError, nameof(onError));
            return target.ContinueWith(task => task.Result.OnError(onError));
        }        

        public static Task<Result<TValue, TError>> Either<TValue, TError>(this Task<Result<TValue, TError>> target, Action<TValue> onSuccess, Action<TError> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess), onError, nameof(onError));
            return target.ContinueWith(task => task.Result.Either(onSuccess, onError));
        }

        public static Task<Result<TNewValue, TError>> Either<TValue, TError, TNewValue>(this Task<Result<TValue, TError>> target, Func<TValue, TNewValue> onSuccess, Action<TError> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess), onError, nameof(onError));
            return target.ContinueWith(task => task.Result.Either(onSuccess, onError));
        }

        public static Task<Result<TValue, TNewError>> Either<TValue, TError, TNewError>(this Task<Result<TValue, TError>> target, Action<TValue> onSuccess, Func<TError, TNewError> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess), onError, nameof(onError));
            return target.ContinueWith(task => task.Result.Either(onSuccess, onError));
        }

        public static Task<Result<TNewValue, TNewError>> Either<TValue, TError, TNewValue, TNewError>(this Task<Result<TValue, TError>> target, Func<TValue, TNewValue> onSuccess, Func<TError, TNewError> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess), onError, nameof(onError));
            return target.ContinueWith(task => task.Result.Either(onSuccess, onError));
        }

        public static Task<Result<TValue, TError>> Ensure<TValue, TError>(this Task<Result<TValue, TError>> target, Func<TValue, bool> condition, Func<TError> errorFunc)
        {
            Args.ExceptionIfNull(target, nameof(target), condition, nameof(condition), errorFunc, nameof(errorFunc));
            return target.ContinueWith(task => task.Result.Ensure(condition, errorFunc));
        }

        public static Task<T> Match<T, TValue, TError>(this Task<Result<TValue, TError>> target, Func<TValue, T> onSuccess, Func<TError, T> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess), onError, nameof(onError));
            return target.ContinueWith(task => task.Result.Match(onSuccess, onError));
        }
    }
}
