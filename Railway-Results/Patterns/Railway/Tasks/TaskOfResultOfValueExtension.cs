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
    public static class TaskOfResultOfValueExtension
    {
        public static Task<Result> AsPlainResult<TValue>(this Task<Result<TValue>> target)
        {
            Args.ExceptionIfNull(target, nameof(target));
            return target.ContinueWith(task => task.Result.AsPlainResult());
        }        

        public static Task<Result> Bind<TValue>(this Task<Result<TValue>> target, Func<TValue, Result> onSuccess)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess));
            return target.ContinueWith(task => task.Result.Bind(onSuccess));
        }

        public static Task<Result<TNewValue>> Bind<TValue, TNewValue>(this Task<Result<TValue>> target, Func<TValue, Result<TNewValue>> onSuccess)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess));
            return target.ContinueWith(task => task.Result.Bind(onSuccess));
        }

        public static Task<TResult> Bind<TValue, TResult>(this Task<Result<TValue>> target, Func<TValue, TResult> onSuccess, Func<TResult> onError)
            where TResult : IResult
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess), onError, nameof(onError));
            return target.ContinueWith(task => task.Result.Bind(onSuccess, onError));
        }

        public static Task<Result<TValue>> BindOnError<TValue>(this Task<Result<TValue>> target, Func<Result<TValue>> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onError, nameof(onError));
            return target.ContinueWith(task => task.Result.BindOnError(onError));
        }
        public static Task<Result<TValue, TError>> BindOnError<TValue, TError>(this Task<Result<TValue>> target, Func<Result<TValue, TError>> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onError, nameof(onError));
            return target.ContinueWith(task => task.Result.BindOnError(onError));
        }

        public static Task<Result<TValue>> OnSuccess<TValue>(this Task<Result<TValue>> target, Action<TValue> onSuccess)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess));
            return target.ContinueWith(task => task.Result.OnSuccess(onSuccess));
        }

        public static Task<Result<TNewValue>> OnSuccess<TValue, TNewValue>(this Task<Result<TValue>> target, Func<TValue, TNewValue> onSuccess)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess));
            return target.ContinueWith(task => task.Result.OnSuccess(onSuccess));
        }

        public static Task<Result<TValue>> FixOnError<TValue>(this Task<Result<TValue>> target, Func<TValue> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onError, nameof(onError));
            return target.ContinueWith(task => task.Result.FixOnError(onError));
        }

        public static Task<Result<TValue>> OnError<TValue>(this Task<Result<TValue>> target, Action onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onError, nameof(onError));
            return target.ContinueWith(task => task.Result.OnError(onError));
        }

        public static Task<Result<TValue, TError>> OnError<TValue, TError>(this Task<Result<TValue>> target, Func<TError> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onError, nameof(onError));
            return target.ContinueWith(task => task.Result.OnError(onError));
        }        

        public static Task<Result<TValue>> Either<TValue>(this Task<Result<TValue>> target, Action<TValue> onSuccess, Action onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess), onError, nameof(onError));
            return target.ContinueWith(task => task.Result.Either(onSuccess, onError));
        }

        public static Task<Result<TNewValue>> Either<TValue, TNewValue>(this Task<Result<TValue>> target, Func<TValue, TNewValue> onSuccess, Action onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess), onError, nameof(onError));
            return target.ContinueWith(task => task.Result.Either(onSuccess, onError));
        }

        public static Task<Result<TValue, TNewError>> Either<TValue, TNewError>(this Task<Result<TValue>> target, Action<TValue> onSuccess, Func<TNewError> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess), onError, nameof(onError));
            return target.ContinueWith(task => task.Result.Either(onSuccess, onError));
        }

        public static Task<Result<TNewValue, TNewError>> Either<TValue, TNewValue, TNewError>(this Task<Result<TValue>> target, Func<TValue, TNewValue> onSuccess, Func<TNewError> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess), onError, nameof(onError));
            return target.ContinueWith(task => task.Result.Either(onSuccess, onError));
        }

        public static Task<Result<TValue>> Ensure<TValue>(this Task<Result<TValue>> target, Func<TValue, bool> condition)
        {
            Args.ExceptionIfNull(target, nameof(target), condition, nameof(condition));
            return target.ContinueWith(task => task.Result.Ensure(condition));
        }

        public static Task<T> Match<T, TValue>(this Task<Result<TValue>> target, Func<TValue, T> onSuccess, Func<T> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess), onError, nameof(onError));
            return target.ContinueWith(task => task.Result.Match(onSuccess, onError));
        }
    }
}
