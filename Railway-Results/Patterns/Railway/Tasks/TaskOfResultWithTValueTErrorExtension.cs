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
    public static class TaskOfResultWithTValueTErrorExtension
    {
        public static Task<Result<TValue, TError>> OnSuccess<TValue, TError>(this Task<Result<TValue, TError>> railsTask, Action<TValue> onSuccess)
        {
            Args.ExceptionIfNull(railsTask, nameof(railsTask), onSuccess, nameof(onSuccess));
            return railsTask.ContinueWith(task => task.Result.OnSuccess(onSuccess));
        }

        public static Task<Result<TNewValue, TError>> OnSuccess<TValue, TError, TNewValue>(this Task<Result<TValue, TError>> railsTask, Func<TValue, TNewValue> onSuccess)
        {
            Args.ExceptionIfNull(railsTask, nameof(railsTask), onSuccess, nameof(onSuccess));
            return railsTask.ContinueWith(task => task.Result.OnSuccess(onSuccess));
        }
        public static Task<Result<TNewValue, TError>> OnSuccess<TValue, TError, TNewValue>(this Task<Result<TValue, TError>> railsTask, Func<TValue, Result<TNewValue, TError>> onSuccess)
        {
            Args.ExceptionIfNull(railsTask, nameof(railsTask), onSuccess, nameof(onSuccess));
            return railsTask.ContinueWith(task => task.Result.OnSuccess(onSuccess));
        }
        public static Task<ResultWithError<TError>> OnSuccess<TValue, TError>(this Task<Result<TValue, TError>> railsTask, Func<TValue, ResultWithError<TError>> onSuccess)
        {
            Args.ExceptionIfNull(railsTask, nameof(railsTask), onSuccess, nameof(onSuccess));
            return railsTask.ContinueWith(task => task.Result.OnSuccess(onSuccess));
        }

        public static Task<Result<TValue, TError>> OnError<TValue, TError>(this Task<Result<TValue, TError>> railsTask, Action<TError> onError)
        {
            Args.ExceptionIfNull(railsTask, nameof(railsTask), onError, nameof(onError));
            return railsTask.ContinueWith(task => task.Result.OnError(onError));
        }

        public static Task<Result<TValue, TNewError>> OnError<TValue, TError, TNewError>(this Task<Result<TValue, TError>> railsTask, Func<TError, TNewError> onError)
        {
            Args.ExceptionIfNull(railsTask, nameof(railsTask), onError, nameof(onError));
            return railsTask.ContinueWith(task => task.Result.OnError(onError));
        }

        public static Task<Result<TValue, TNewError>> OnError<TValue, TError, TNewError>(this Task<Result<TValue, TError>> railsTask, Func<TError, Result<TValue, TNewError>> onError)
        {
            Args.ExceptionIfNull(railsTask, nameof(railsTask), onError, nameof(onError));
            return railsTask.ContinueWith(task => task.Result.OnError(onError));
        }

        public static Task<Result<TValue>> OnError<TValue, TError>(this Task<Result<TValue, TError>> railsTask, Func<TError, Result<TValue>> onError)
        {
            Args.ExceptionIfNull(railsTask, nameof(railsTask), onError, nameof(onError));
            return railsTask.ContinueWith(task => task.Result.OnError(onError));
        }

        public static Task<Result<TValue, TError>> Either<TValue, TError>(this Task<Result<TValue, TError>> railsTask, Action<TValue> onSuccess, Action<TError> onError)
        {
            Args.ExceptionIfNull(railsTask, nameof(railsTask), onSuccess, nameof(onSuccess), onError, nameof(onError));
            return railsTask.ContinueWith(task => task.Result.Either(onSuccess, onError));
        }

        public static Task<Result<TNewValue, TError>> Either<TValue, TError, TNewValue>(this Task<Result<TValue, TError>> railsTask, Func<TValue, TNewValue> onSuccess, Action<TError> onError)
        {
            Args.ExceptionIfNull(railsTask, nameof(railsTask), onSuccess, nameof(onSuccess), onError, nameof(onError));
            return railsTask.ContinueWith(task => task.Result.Either(onSuccess, onError));
        }

        public static Task<Result<TValue, TNewError>> Either<TValue, TError, TNewError>(this Task<Result<TValue, TError>> railsTask, Action<TValue> onSuccess, Func<TError, TNewError> onError)
        {
            Args.ExceptionIfNull(railsTask, nameof(railsTask), onSuccess, nameof(onSuccess), onError, nameof(onError));
            return railsTask.ContinueWith(task => task.Result.Either(onSuccess, onError));
        }

        public static Task<Result<TNewValue, TNewError>> Either<TValue, TError, TNewValue, TNewError>(this Task<Result<TValue, TError>> railsTask, Func<TValue, TNewValue> onSuccess, Func<TError, TNewError> onError)
        {
            Args.ExceptionIfNull(railsTask, nameof(railsTask), onSuccess, nameof(onSuccess), onError, nameof(onError));
            return railsTask.ContinueWith(task => task.Result.Either(onSuccess, onError));
        }

        public static Task<T> Match<T, TValue, TError>(this Task<Result<TValue, TError>> railsTask, Func<TValue, T> onSuccess, Func<TError, T> onError)
        {
            Args.ExceptionIfNull(railsTask, nameof(railsTask), onSuccess, nameof(onSuccess), onError, nameof(onError));
            return railsTask.ContinueWith(task => task.Result.Match(onSuccess, onError));
        }
    }
}
