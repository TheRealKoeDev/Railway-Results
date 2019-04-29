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
        public static Task<Result> OnSuccess(this Task<Result> railsTask, Action onSuccess)
        {
            Args.ExceptionIfNull(railsTask, nameof(railsTask), onSuccess, nameof(onSuccess));
            return railsTask.ContinueWith(task => task.Result.OnSuccess(onSuccess));
        }

        public static Task<Result<TValue>> OnSuccess<TValue>(this Task<Result> railsTask, Func<TValue> onSuccess)
        {
            Args.ExceptionIfNull(railsTask, nameof(railsTask), onSuccess, nameof(onSuccess));
            return railsTask.ContinueWith((Task<Result> task) => task.Result.OnSuccess(onSuccess));
        }

        public static Task<Result> Bind(this Task<Result> railsTask, Func<Result> onSuccess)
        {
            Args.ExceptionIfNull(railsTask, nameof(railsTask), onSuccess, nameof(onSuccess));
            return railsTask.ContinueWith(task => task.Result.Bind(onSuccess));
        }

        public static Task<Result<TValue>> Bind<TValue>(this Task<Result> railsTask, Func<Result<TValue>> onSuccess)
        {
            Args.ExceptionIfNull(railsTask, nameof(railsTask), onSuccess, nameof(onSuccess));
            return railsTask.ContinueWith(task => task.Result.Bind(onSuccess));
        }

        public static Task<Result> OnError(this Task<Result> railsTask, Action onError)
        {
            Args.ExceptionIfNull(railsTask, nameof(railsTask), onError, nameof(onError));
            return railsTask.ContinueWith(task => task.Result.OnError(onError));
        }

        public static Task<ResultWithError<TError>> OnError<TError>(this Task<Result> railsTask, Func<TError> onError)
        {
            Args.ExceptionIfNull(railsTask, nameof(railsTask), onError, nameof(onError));
            return railsTask.ContinueWith(task => task.Result.OnError(onError));
        }

        public static Task<Result> BindOnError(this Task<Result> railsTask, Func<Result> onError)
        {
            Args.ExceptionIfNull(railsTask, nameof(railsTask), onError, nameof(onError));
            return railsTask.ContinueWith(task => task.Result.BindOnError(onError));
        }

        public static Task<ResultWithError<TError>> BindOnError<TError>(this Task<Result> railsTask, Func<ResultWithError<TError>> onError)
        {
            Args.ExceptionIfNull(railsTask, nameof(railsTask), onError, nameof(onError));
            return railsTask.ContinueWith(task => task.Result.BindOnError(onError));
        }

        public static Task<Result> Either(this Task<Result> railsTask, Action onSuccess, Action onError)
        {
            Args.ExceptionIfNull(railsTask, nameof(railsTask), onSuccess, nameof(onSuccess), onError, nameof(onError));
            return railsTask.ContinueWith(task => task.Result.OnError(onError));
        }

        public static Task<Result<TValue>> Either<TValue>(this Task<Result> railsTask, Func<TValue> onSuccess, Action onError)
        {
            Args.ExceptionIfNull(railsTask, nameof(railsTask), onSuccess, nameof(onSuccess), onError, nameof(onError));
            return railsTask.ContinueWith(((Task<Result> task) => task.Result.Either(onSuccess, onError)));
        }

        public static Task<ResultWithError<TError>> Either<TError>(this Task<Result> railsTask, Action onSuccess, Func<TError> onError)
        {
            Args.ExceptionIfNull(railsTask, nameof(railsTask), onSuccess, nameof(onSuccess), onError, nameof(onError));
            return railsTask.ContinueWith(task => task.Result.Either(onSuccess, onError));
        }

        public static Task<Result<TValue, TError>> Either<TValue, TError>(this Task<Result> railsTask, Func<TValue> onSuccess, Func<TError> onError)
        {
            Args.ExceptionIfNull(railsTask, nameof(railsTask), onSuccess, nameof(onSuccess), onError, nameof(onError));
            return railsTask.ContinueWith(((Task<Result> task) => task.Result.Either(onSuccess, onError)));
        }


        public static Task<T> Match<T>(this Task<Result> railsTask, Func<T> onSuccess, Func<T> onError)
        {
            Args.ExceptionIfNull(railsTask, nameof(railsTask), onSuccess, nameof(onSuccess), onError, nameof(onError));
            return railsTask.ContinueWith(task => task.Result.Match(onSuccess, onError));
        }
    }
}
