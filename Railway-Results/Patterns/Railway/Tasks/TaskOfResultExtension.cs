using KoeLib.Patterns.Railway.Results;
using KoeLib.Patterns.Railway.Tools;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace KoeLib.Patterns.Railway.Tasks
{
    [DebuggerStepThrough]
    public static class TaskOfResultExtension
    {
        public static Task<Result> Bind(this Task<Result> target, Func<Result> onSuccess)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess));
            return target.ContinueWith(task => task.Result.Bind(onSuccess));
        }

        public static Task<Result<TValue>> Bind<TValue>(this Task<Result> target, Func<Result<TValue>> onSuccess)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess));
            return target.ContinueWith(task => task.Result.Bind(onSuccess));
        }
        public static Task<Result> BindOnError(this Task<Result> target, Func<Result> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onError, nameof(onError));
            return target.ContinueWith(task => task.Result.BindOnError(onError));
        }

        public static Task<ResultWithError<TError>> BindOnError<TError>(this Task<Result> target, Func<ResultWithError<TError>> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onError, nameof(onError));
            return target.ContinueWith(task => task.Result.BindOnError(onError));
        }

        public static Task<Result> OnSuccess(this Task<Result> target, Action onSuccess)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess));
            return target.ContinueWith(task => task.Result.OnSuccess(onSuccess));
        }

        public static Task<Result<TValue>> OnSuccess<TValue>(this Task<Result> target, Func<TValue> onSuccess)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess));
            return target.ContinueWith((Task<Result> task) => task.Result.OnSuccess(onSuccess));
        }

        public static Task<Result> OnError(this Task<Result> target, Action onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onError, nameof(onError));
            return target.ContinueWith(task => task.Result.OnError(onError));
        }

        public static Task<ResultWithError<TError>> OnError<TError>(this Task<Result> target, Func<TError> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onError, nameof(onError));
            return target.ContinueWith(task => task.Result.OnError(onError));
        }

        public static Task<Result> Either(this Task<Result> target, Action onSuccess, Action onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess), onError, nameof(onError));
            return target.ContinueWith(task => task.Result.OnError(onError));
        }

        public static Task<Result<TValue>> Either<TValue>(this Task<Result> target, Func<TValue> onSuccess, Action onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess), onError, nameof(onError));
            return target.ContinueWith(((Task<Result> task) => task.Result.Either(onSuccess, onError)));
        }

        public static Task<ResultWithError<TError>> Either<TError>(this Task<Result> target, Action onSuccess, Func<TError> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess), onError, nameof(onError));
            return target.ContinueWith(task => task.Result.Either(onSuccess, onError));
        }

        public static Task<Result<TValue, TError>> Either<TValue, TError>(this Task<Result> target, Func<TValue> onSuccess, Func<TError> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess), onError, nameof(onError));
            return target.ContinueWith(((Task<Result> task) => task.Result.Either(onSuccess, onError)));
        }

        public static Task<Result> Ensure(this Task<Result> target, Func<bool> condition)
        {
            Args.ExceptionIfNull(target, nameof(target), condition, nameof(condition));
            return target.ContinueWith(task => task.Result.Ensure(condition));
        }

        public static Task<T> Match<T>(this Task<Result> target, Func<T> onSuccess, Func<T> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess), onError, nameof(onError));
            return target.ContinueWith(task => task.Result.Match(onSuccess, onError));
        }
    }
}
