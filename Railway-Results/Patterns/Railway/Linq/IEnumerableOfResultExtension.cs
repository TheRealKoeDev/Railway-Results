using KoeLib.Patterns.Railway.Results;
using KoeLib.Patterns.Railway.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace KoeLib.Patterns.Railway.Linq
{
    [DebuggerStepThrough]
    public static class IEnumerableOfResultExtension
    {
        public static IEnumerable<Result> Successes(this IEnumerable<Result> target)
        {            
            Args.ExceptionIfNull(target, nameof(target));
            foreach (Result result in target)
            {
                if (result.Match(() => true, () => false))
                {
                    yield return result;
                }
            }
        }

        public static IEnumerable<Result> Errors(this IEnumerable<Result> target)
        {
            Args.ExceptionIfNull(target, nameof(target));
            foreach (Result result in target)
            {
                if (result.Match(() => false, () => true))
                {
                    yield return result;
                }
            }
        }

        public static IEnumerable<Result> Bind(this IEnumerable<Result> target, Func<Result> onSuccess)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess));
            foreach (Result result in target)
            {
                yield return result.Bind(onSuccess);
            }
        }

        public static IEnumerable<Result<TValue>> Bind<TValue>(this IEnumerable<Result> target, Func<Result<TValue>> onSuccess)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess));
            foreach (Result result in target)
            {
                yield return result.Bind(onSuccess);
            }
        }

        public static IEnumerable<Result> OnSuccess(this IEnumerable<Result> target, Action onSuccess)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess));
            foreach (Result result in target)
            {
                yield return result.OnSuccess(onSuccess);
            }
        }

        public static IEnumerable<Result<TValue>> OnSuccess<TValue>(this IEnumerable<Result> target, Func<TValue> onSuccess)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess));
            foreach (Result result in target)
            {
                yield return result.OnSuccess(onSuccess);
            }
        }

        public static IEnumerable<Result> BindOnError(this IEnumerable<Result> target, Func<Result> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onError, nameof(onError));
            foreach (Result result in target)
            {
                yield return result.BindOnError(onError);
            }
        }

        public static IEnumerable<ResultWithError<TError>> BindOnError<TError>(this IEnumerable<Result> target, Func<ResultWithError<TError>> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onError, nameof(onError));
            foreach (Result result in target)
            {
                yield return result.BindOnError(onError);
            }
        }

        public static IEnumerable<Result> OnError(this IEnumerable<Result> target, Action onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onError, nameof(onError));
            foreach (Result result in target)
            {
                yield return result.OnError(onError);
            }
        }

        public static IEnumerable<ResultWithError<TError>> OnError<TError>(this IEnumerable<Result> target, Func<TError> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onError, nameof(onError));
            foreach (Result result in target)
            {
                yield return result.OnError(onError);
            }
        }

        public static IEnumerable<Result> Either(this IEnumerable<Result> target, Action onSuccess, Action onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess), onError, nameof(onError));
            foreach (Result result in target)
            {
                yield return result.Either(onSuccess, onError);
            }
        }

        public static IEnumerable<Result<TValue>> Either<TValue>(this IEnumerable<Result> target, Func<TValue> onSuccess, Action onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess), onError, nameof(onError));
            foreach (Result result in target)
            {
                yield return result.Either(onSuccess, onError);
            }
        }

        public static IEnumerable<ResultWithError<TError>> Either<TError>(this IEnumerable<Result> target, Action onSuccess, Func<TError> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess), onError, nameof(onError));
            foreach (Result result in target)
            {
                yield return result.Either(onSuccess, onError);
            }
        }

        public static IEnumerable<Result<TValue, TError>> Either<TValue, TError>(this IEnumerable<Result> target, Func<TValue> onSuccess, Func<TError> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess), onError, nameof(onError));
            foreach (Result result in target)
            {
                yield return result.Either(onSuccess, onError);
            }
        }

        public static IEnumerable<Result> Ensure(this IEnumerable<Result> target, Func<bool> condition)
        {
            Args.ExceptionIfNull(target, nameof(target), condition, nameof(condition));
            foreach (Result result in target)
            {
                yield return result.Ensure(condition);
            }
        }

        public static IEnumerable<T> Match<T>(this IEnumerable<Result> target, Func<T> onSuccess, Func<T> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess), onError, nameof(onError));
            foreach (Result result in target)
            {
                yield return result.Match(onSuccess, onError);
            }
        }
    }
}
