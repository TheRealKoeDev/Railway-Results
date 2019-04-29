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
    public static class TaskOfResultWithTValueExtension
    {
        public static Task<Result<TValue>> OnSuccess<TValue>(this Task<Result<TValue>> railsTask, Action<TValue> onSuccess)
        {
            Args.ExceptionIfNull(railsTask, nameof(railsTask), onSuccess, nameof(onSuccess));
            return railsTask.ContinueWith(task => task.Result.OnSuccess(onSuccess));
        }

        public static Task<Result<TNewValue>> OnSuccess<TValue, TNewValue>(this Task<Result<TValue>> railsTask, Func<TValue, TNewValue> onSuccess)
        {
            Args.ExceptionIfNull(railsTask, nameof(railsTask), onSuccess, nameof(onSuccess));
            return railsTask.ContinueWith(task => task.Result.OnSuccess(onSuccess));
        }

        public static Task<Result> OnSuccess<TValue>(this Task<Result<TValue>> railsTask, Func<TValue, Result> onSuccess)
        {
            Args.ExceptionIfNull(railsTask, nameof(railsTask), onSuccess, nameof(onSuccess));
            return railsTask.ContinueWith(task => task.Result.OnSuccess(onSuccess));
        }
        public static Task<Result<TNewValue>> OnSuccess<TValue, TNewValue>(this Task<Result<TValue>> railsTask, Func<TValue, Result<TNewValue>> onSuccess)
        {
            Args.ExceptionIfNull(railsTask, nameof(railsTask), onSuccess, nameof(onSuccess));
            return railsTask.ContinueWith(task => task.Result.OnSuccess(onSuccess));
        }

        public static Task<Result<TValue>> OnError<TValue>(this Task<Result<TValue>> railsTask, Action onError)
        {
            Args.ExceptionIfNull(railsTask, nameof(railsTask), onError, nameof(onError));
            return railsTask.ContinueWith(task => task.Result.OnError(onError));
        }
        public static Task<Result<TValue, TError>> OnError<TValue, TError>(this Task<Result<TValue>> railsTask, Func<TError> onError)
        {
            Args.ExceptionIfNull(railsTask, nameof(railsTask), onError, nameof(onError));
            return railsTask.ContinueWith(task => task.Result.OnError(onError));
        }

        public static Task<Result<TValue>> OnError<TValue>(this Task<Result<TValue>> railsTask, Func<Result<TValue>> onError)
        {
            Args.ExceptionIfNull(railsTask, nameof(railsTask), onError, nameof(onError));
            return railsTask.ContinueWith(task => task.Result.OnError(onError));
        }
        public static Task<Result<TValue, TError>> OnError<TValue, TError>(this Task<Result<TValue>> railsTask, Func<Result<TValue, TError>> onError)
        {
            Args.ExceptionIfNull(railsTask, nameof(railsTask), onError, nameof(onError));
            return railsTask.ContinueWith(task => task.Result.OnError(onError));
        }

        public static Task<Result<TValue>> Either<TValue>(this Task<Result<TValue>> railsTask, Action<TValue> onSuccess, Action onError)
        {
            Args.ExceptionIfNull(railsTask, nameof(railsTask), onSuccess, nameof(onSuccess), onError, nameof(onError));
            return railsTask.ContinueWith(task => task.Result.OnError(onError));
        }

        public static Task<Result<TNewValue>> Either<TValue, TNewValue>(this Task<Result<TValue>> railsTask, Func<TValue, TNewValue> onSuccess, Action onError)
        {
            Args.ExceptionIfNull(railsTask, nameof(railsTask), onSuccess, nameof(onSuccess), onError, nameof(onError));
            return railsTask.ContinueWith(task => task.Result.Either(onSuccess, onError));
        }
        public static Task<Result<TValue, TNewError>> Either<TValue, TNewError>(this Task<Result<TValue>> railsTask, Action<TValue> onSuccess, Func<TNewError> onError)
        {
            Args.ExceptionIfNull(railsTask, nameof(railsTask), onSuccess, nameof(onSuccess), onError, nameof(onError));
            return railsTask.ContinueWith(task => task.Result.Either(onSuccess, onError));
        }

        public static Task<Result<TNewValue, TNewError>> Either<TValue, TNewValue, TNewError>(this Task<Result<TValue>> railsTask, Func<TValue, TNewValue> onSuccess, Func<TNewError> onError)
        {
            Args.ExceptionIfNull(railsTask, nameof(railsTask), onSuccess, nameof(onSuccess), onError, nameof(onError));
            return railsTask.ContinueWith(task => task.Result.Either(onSuccess, onError));
        }

        public static Task<T> Match<T, TValue>(this Task<Result<TValue>> railsTask, Func<TValue, T> onSuccess, Func<T> onError)
        {
            Args.ExceptionIfNull(railsTask, nameof(railsTask), onSuccess, nameof(onSuccess), onError, nameof(onError));
            return railsTask.ContinueWith(task => task.Result.Match(onSuccess, onError));
        }
    }
}
