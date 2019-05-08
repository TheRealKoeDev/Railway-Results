using KoeLib.Patterns.Railway.Results;
using KoeLib.Patterns.Railway.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace KoeLib.Patterns.Railway.Linq
{
    [DebuggerStepThrough]
    public static class IEnumerableOfErrorResultOfTErrorExtension
    {
        public static IEnumerable<ResultWithError<TError>> Successes<TError>(this IEnumerable<ResultWithError<TError>> target)
        {
            Args.ExceptionIfNull(target, nameof(target));
            foreach (ResultWithError<TError> result in target)
            {
                if (result.Match(() => true, error => false))
                {
                    yield return result;
                }
            }
        }

        public static IEnumerable<ResultWithError<TError>> Errors<TError>(this IEnumerable<ResultWithError<TError>> target)
        {
            Args.ExceptionIfNull(target, nameof(target));
            foreach (ResultWithError<TError> result in target)
            {
                if (result.Match(() => false, error => true))
                {
                    yield return result;
                }
            }
        }

        public static IEnumerable<ResultWithError<TError>> OnSuccess<TError>(this IEnumerable<ResultWithError<TError>> target, Action onSuccess)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess));
            foreach (ResultWithError<TError> result in target)
            {
                yield return result.OnSuccess(onSuccess);
            }
        }

        public static IEnumerable<Result<TValue, TError>> OnSuccess<TValue, TError>(this IEnumerable<ResultWithError<TError>> target, Func<TValue> onSuccess)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess));
            foreach (ResultWithError<TError> result in target)
            {
                yield return result.OnSuccess(onSuccess);
            }
        }

        public static IEnumerable<ResultWithError<TError>> Bind<TError>(this IEnumerable<ResultWithError<TError>> target, Func<ResultWithError<TError>> onSuccess)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess));
            foreach (ResultWithError<TError> result in target)
            {
                yield return result.Bind(onSuccess);
            }
        }

        public static IEnumerable<Result<TValue, TError>> Bind<TValue, TError>(this IEnumerable<ResultWithError<TError>> target, Func<Result<TValue, TError>> onSuccess)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess));
            foreach (ResultWithError<TError> result in target)
            {
                yield return result.Bind(onSuccess);
            }
        }

        public static IEnumerable<ResultWithError<TError>> OnError<TError>(this IEnumerable<ResultWithError<TError>> target, Action<TError> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onError, nameof(onError));
            foreach (ResultWithError<TError> result in target)
            {
                yield return result.OnError(onError);
            }
        }

        public static IEnumerable<ResultWithError<TNewError>> OnError<TError, TNewError>(this IEnumerable<ResultWithError<TError>> target, Func<TError, TNewError> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onError, nameof(onError));
            foreach (ResultWithError<TError> result in target)
            {
                yield return result.OnError(onError);
            }
        }

        public static IEnumerable<ResultWithError<TNewError>> BindOnError<TError, TNewError>(this IEnumerable<ResultWithError<TError>> target, Func<TError, ResultWithError<TNewError>> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onError, nameof(onError));
            foreach (ResultWithError<TError> result in target)
            {
                yield return result.BindOnError(onError);
            }
        }

        public static IEnumerable<Result> BindOnError<TError>(this IEnumerable<ResultWithError<TError>> target, Func<TError, Result> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onError, nameof(onError));
            foreach (ResultWithError<TError> result in target)
            {
                yield return result.BindOnError(onError);
            }
        }
        public static IEnumerable<ResultWithError<TError>> Either<TError>(this IEnumerable<ResultWithError<TError>> target, Action onSuccess, Action<TError> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess), onError, nameof(onError));
            foreach (ResultWithError<TError> result in target)
            {
                yield return result.Either(onSuccess, onError);
            }
        }

        public static IEnumerable<Result<TValue, TError>> Either<TValue, TError>(this IEnumerable<ResultWithError<TError>> target, Func<TValue> onSuccess, Action<TError> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess), onError, nameof(onError));
            foreach (ResultWithError<TError> result in target)
            {
                yield return result.Either(onSuccess, onError);
            }
        }
        public static IEnumerable<ResultWithError<TNewError>> Either<TError, TNewError>(this IEnumerable<ResultWithError<TError>> target, Action onSuccess, Func<TError, TNewError> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess), onError, nameof(onError));
            foreach (ResultWithError<TError> result in target)
            {
                yield return result.Either(onSuccess, onError);
            }
        }

        public static IEnumerable<Result<TValue, TNewError>> Either<TValue, TError, TNewError>(this IEnumerable<ResultWithError<TError>> target, Func<TValue> onSuccess, Func<TError, TNewError> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess), onError, nameof(onError));
            foreach (ResultWithError<TError> result in target)
            {
                yield return result.Either(onSuccess, onError);
            }
        }

        public static IEnumerable<ResultWithError<TError>> Ensure<TError>(this IEnumerable<ResultWithError<TError>> target, Func<bool> condition, Func<TError> errorFunc)
        {
            Args.ExceptionIfNull(target, nameof(target), condition, nameof(condition), errorFunc, nameof(errorFunc));
            foreach (ResultWithError<TError> result in target)
            {
                yield return result.Ensure(condition, errorFunc);
            }
        }

        public static IEnumerable<T> Match<T, TError>(this IEnumerable<ResultWithError<TError>> target, Func<T> onSuccess, Func<TError, T> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess), onError, nameof(onError));
            foreach (ResultWithError<TError> result in target)
            {
                yield return result.Match(onSuccess, onError);
            }
        }
    }
}
