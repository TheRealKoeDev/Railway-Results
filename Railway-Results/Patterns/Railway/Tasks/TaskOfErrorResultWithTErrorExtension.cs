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
    public static class TaskOfErrorResultWithTErrorExtension
    {
        public static Task<ResultWithError<TError>> OnSuccess<TError>(this Task<ResultWithError<TError>> railsTask, Action onSuccess)
        {
            Args.ExceptionIfNull(railsTask, nameof(railsTask), onSuccess, nameof(onSuccess));
            return railsTask.ContinueWith(task => task.Result.OnSuccess(onSuccess));
        }

        public static Task<Result<TValue, TError>> OnSuccess<TValue, TError>(this Task<ResultWithError<TError>> railsTask, Func<TValue> onSuccess)
        {
            Args.ExceptionIfNull(railsTask, nameof(railsTask), onSuccess, nameof(onSuccess));
            return railsTask.ContinueWith(task => task.Result.OnSuccess(onSuccess));
        }

        public static Task<ResultWithError<TError>> Bind<TError>(this Task<ResultWithError<TError>> railsTask, Func<ResultWithError<TError>> onSuccess)
        {
            Args.ExceptionIfNull(railsTask, nameof(railsTask), onSuccess, nameof(onSuccess));
            return railsTask.ContinueWith(task => task.Result.Bind(onSuccess));
        }

        public static Task<Result<TValue, TError>> Bind<TValue, TError>(this Task<ResultWithError<TError>> railsTask, Func<Result<TValue, TError>> onSuccess)
        {
            Args.ExceptionIfNull(railsTask, nameof(railsTask), onSuccess, nameof(onSuccess));
            return railsTask.ContinueWith(task => task.Result.Bind(onSuccess));
        }

        public static Task<ResultWithError<TError>> OnError<TError>(this Task<ResultWithError<TError>> railsTask, Action<TError> onError)
        {
            Args.ExceptionIfNull(railsTask, nameof(railsTask), onError, nameof(onError));
            return railsTask.ContinueWith(task => task.Result.OnError(onError));
        }

        public static Task<ResultWithError<TNewError>> OnError<TError, TNewError>(this Task<ResultWithError<TError>> railsTask, Func<TError, TNewError> onError)
        {
            Args.ExceptionIfNull(railsTask, nameof(railsTask), onError, nameof(onError));
            return railsTask.ContinueWith(task => task.Result.OnError(onError));
        }

        public static Task<ResultWithError<TNewError>> BindOnError<TError, TNewError>(this Task<ResultWithError<TError>> railsTask, Func<TError, ResultWithError<TNewError>> onError)
        {
            Args.ExceptionIfNull(railsTask, nameof(railsTask), onError, nameof(onError));
            return railsTask.ContinueWith(task => task.Result.BindOnError(onError));
        }

        public static Task<Result> BindOnError<TError>(this Task<ResultWithError<TError>> railsTask, Func<TError, Result> onError)
        {
            Args.ExceptionIfNull(railsTask, nameof(railsTask), onError, nameof(onError));
            return railsTask.ContinueWith(task => task.Result.BindOnError(onError));
        }
        public static Task<ResultWithError<TError>> Either<TError>(this Task<ResultWithError<TError>> railsTask, Action onSuccess, Action<TError> onError)
        {
            Args.ExceptionIfNull(railsTask, nameof(railsTask), onSuccess, nameof(onSuccess), onError, nameof(onError));
            return railsTask.ContinueWith(task => task.Result.Either(onSuccess, onError));
        }

        public static Task<Result<TValue, TError>> Either<TValue, TError>(this Task<ResultWithError<TError>> railsTask, Func<TValue> onSuccess, Action<TError> onError)
        {
            Args.ExceptionIfNull(railsTask, nameof(railsTask), onSuccess, nameof(onSuccess), onError, nameof(onError));
            return railsTask.ContinueWith(task => task.Result.Either(onSuccess, onError));
        }
        public static Task<ResultWithError<TNewError>> Either<TError, TNewError>(this Task<ResultWithError<TError>> railsTask, Action onSuccess, Func<TError, TNewError> onError)
        {
            Args.ExceptionIfNull(railsTask, nameof(railsTask), onSuccess, nameof(onSuccess), onError, nameof(onError));
            return railsTask.ContinueWith(task => task.Result.Either(onSuccess, onError));
        }

        public static Task<Result<TValue, TNewError>> Either<TValue, TError, TNewError>(this Task<ResultWithError<TError>> railsTask, Func<TValue> onSuccess, Func<TError, TNewError> onError)
        {
            Args.ExceptionIfNull(railsTask, nameof(railsTask), onSuccess, nameof(onSuccess), onError, nameof(onError));
            return railsTask.ContinueWith(task => task.Result.Either(onSuccess, onError));
        }

        public static Task<T> Match<T, TError>(this Task<ResultWithError<TError>> railsTask, Func<T> onSuccess, Func<TError, T> onError)
        {
            Args.ExceptionIfNull(railsTask, nameof(railsTask), onSuccess, nameof(onSuccess), onError, nameof(onError));
            return railsTask.ContinueWith(task => task.Result.Match(onSuccess, onError));
        }
    }
}
