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
    public static class TaskOfResultOfErrorExtension
    {
        public static Task<Result> AsPlainResult<TError>(this Task<ResultOrError<TError>> target)
        {
            Args.ExceptionIfNull(target, nameof(target));
            return target.ContinueWith(task => task.Result.AsPlainResult());
        }       

        public static Task<ResultOrError<TError>> Bind<TError>(this Task<ResultOrError<TError>> target, Func<ResultOrError<TError>> onSuccess)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess));
            return target.ContinueWith(task => task.Result.Bind(onSuccess));
        }

        public static Task<Result<TValue, TError>> Bind<TValue, TError>(this Task<ResultOrError<TError>> target, Func<Result<TValue, TError>> onSuccess)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess));
            return target.ContinueWith(task => task.Result.Bind(onSuccess));
        }

        public static Task<TResult> Bind<TError, TResult>(this Task<ResultOrError<TError>> target, Func<TResult> onSuccess, Func<TError, TResult> onError)
            where TResult : IResult
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess), onError, nameof(onError));
            return target.ContinueWith(task => task.Result.Bind(onSuccess, onError));
        }

        public static Task<ResultOrError<TNewError>> BindOnError<TError, TNewError>(this Task<ResultOrError<TError>> target, Func<TError, ResultOrError<TNewError>> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onError, nameof(onError));
            return target.ContinueWith(task => task.Result.BindOnError(onError));
        }

        public static Task<Result> BindOnError<TError>(this Task<ResultOrError<TError>> target, Func<TError, Result> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onError, nameof(onError));
            return target.ContinueWith(task => task.Result.BindOnError(onError));
        }

        public static Task<ResultOrError<TError>> OnSuccess<TError>(this Task<ResultOrError<TError>> target, Action onSuccess)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess));
            return target.ContinueWith(task => task.Result.OnSuccess(onSuccess));
        }

        public static Task<Result<TValue, TError>> OnSuccess<TValue, TError>(this Task<ResultOrError<TError>> target, Func<TValue> onSuccess)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess));
            return target.ContinueWith(task => task.Result.OnSuccess(onSuccess));
        }

        public static Task<ResultOrError<TError>> OnError<TError>(this Task<ResultOrError<TError>> target, Action<TError> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onError, nameof(onError));
            return target.ContinueWith(task => task.Result.OnError(onError));
        }

        public static Task<ResultOrError<TNewError>> OnError<TError, TNewError>(this Task<ResultOrError<TError>> target, Func<TError, TNewError> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onError, nameof(onError));
            return target.ContinueWith(task => task.Result.OnError(onError));
        }
        
        public static Task<ResultOrError<TError>> Either<TError>(this Task<ResultOrError<TError>> target, Action onSuccess, Action<TError> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess), onError, nameof(onError));
            return target.ContinueWith(task => task.Result.Either(onSuccess, onError));
        }

        public static Task<Result<TValue, TError>> Either<TValue, TError>(this Task<ResultOrError<TError>> target, Func<TValue> onSuccess, Action<TError> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess), onError, nameof(onError));
            return target.ContinueWith(task => task.Result.Either(onSuccess, onError));
        }

        public static Task<ResultOrError<TNewError>> Either<TError, TNewError>(this Task<ResultOrError<TError>> target, Action onSuccess, Func<TError, TNewError> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess), onError, nameof(onError));
            return target.ContinueWith(task => task.Result.Either(onSuccess, onError));
        }

        public static Task<Result<TValue, TNewError>> Either<TValue, TError, TNewError>(this Task<ResultOrError<TError>> target, Func<TValue> onSuccess, Func<TError, TNewError> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess), onError, nameof(onError));
            return target.ContinueWith(task => task.Result.Either(onSuccess, onError));
        }

        public static Task<ResultOrError<TError>> Ensure<TError>(this Task<ResultOrError<TError>> target, Func<bool> condition, Func<TError> errorFunc)
        {
            Args.ExceptionIfNull(target, nameof(target), condition, nameof(condition), errorFunc, nameof(errorFunc));
            return target.ContinueWith(task => task.Result.Ensure(condition, errorFunc));
        }

        public static Task<T> Match<T, TError>(this Task<ResultOrError<TError>> target, Func<T> onSuccess, Func<TError, T> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess), onError, nameof(onError));
            return target.ContinueWith(task => task.Result.Match(onSuccess, onError));
        }
    }
}
