using KoeLib.Patterns.Railway.Results;
using KoeLib.Patterns.Railway.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace KoeLib.Patterns.Railway.Linq
{
    [DebuggerStepThrough]
    public static class IEnumerableOfResultOfValueExtension
    {
        public static IEnumerable<Result<TValue>> Successes<TValue>(this IEnumerable<Result<TValue>> target)
        {
            Args.ExceptionIfNull(target, nameof(target));
            foreach (Result<TValue> result in target)
            {
                if (result.Match(value => true, () => false))
                {
                    yield return result;
                }
            }
        }

        public static IEnumerable<Result<TValue>> Errors<TValue>(this IEnumerable<Result<TValue>> target)
        {
            Args.ExceptionIfNull(target, nameof(target));
            foreach (Result<TValue> result in target)
            {
                if (result.Match(value => false, () => true))
                {
                    yield return result;
                }
            }
        }

        public static IEnumerable<Result<TValue>> Filter<TValue>(this IEnumerable<Result<TValue>> target, Func<TValue, bool> successFilter, Func<bool> errorFilter)
        {
            Args.ExceptionIfNull(target, nameof(target), successFilter, nameof(successFilter), errorFilter, nameof(errorFilter));
            foreach (Result<TValue> result in target)
            {
                if (result.Match(successFilter, errorFilter))
                {
                    yield return result;
                }
            }
        }

        public static IEnumerable<Result> AsPlainResults<TValue>(this IEnumerable<Result<TValue>> target)
        {
            Args.ExceptionIfNull(target, nameof(target));            
            foreach (Result<TValue> result in target)
            {
                yield return result;
            }
        }
        
        public static IEnumerable<Result> Bind<TValue>(this IEnumerable<Result<TValue>> target, Func<TValue, Result> onSuccess)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess));
            foreach (Result<TValue> result in target)
            {
                yield return result.Bind(onSuccess);
            }
        }

        public static IEnumerable<Result<TNewValue>> Bind<TValue, TNewValue>(this IEnumerable<Result<TValue>> target, Func<TValue, Result<TNewValue>> onSuccess)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess));
            foreach (Result<TValue> result in target)
            {
                yield return result.Bind(onSuccess);
            }
        }

        public static IEnumerable<TResult> Bind<TValue, TResult>(this IEnumerable<Result<TValue>> target, Func<TValue, TResult> onSuccess, Func<TResult> onError)
            where TResult : IResult
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess), onError, nameof(onError));
            foreach (Result<TValue> result in target)
            {
                yield return result.Bind(onSuccess, onError);
            }
        }

        public static IEnumerable<Result<TValue>> BindOnError<TValue>(this IEnumerable<Result<TValue>> target, Func<Result<TValue>> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onError, nameof(onError));
            foreach (Result<TValue> result in target)
            {
                yield return result.BindOnError(onError);
            }
        }

        public static IEnumerable<Result<TValue, TError>> BindOnError<TValue, TError>(this IEnumerable<Result<TValue>> target, Func<Result<TValue, TError>> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onError, nameof(onError));
            foreach (Result<TValue> result in target)
            {
                yield return result.BindOnError(onError);
            }
        }

        public static IEnumerable<Result<TValue>> OnSuccess<TValue>(this IEnumerable<Result<TValue>> target, Action<TValue> onSuccess)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess));
            foreach (Result<TValue> result in target)
            {
                yield return result.OnSuccess(onSuccess);
            }
        }

        public static IEnumerable<Result<TNewValue>> OnSuccess<TValue, TNewValue>(this IEnumerable<Result<TValue>> target, Func<TValue, TNewValue> onSuccess)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess));
            foreach (Result<TValue> result in target)
            {
                yield return result.OnSuccess(onSuccess);
            }
        }

        public static IEnumerable<Result<TValue>> OnError<TValue>(this IEnumerable<Result<TValue>> target, Action onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onError, nameof(onError));
            foreach (Result<TValue> result in target)
            {
                yield return result.OnError(onError);
            }
        }

        public static IEnumerable<Result<TValue, TError>> OnError<TValue, TError>(this IEnumerable<Result<TValue>> target, Func<TError> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onError, nameof(onError));
            foreach (Result<TValue> result in target)
            {
                yield return result.OnError(onError);
            }
        }
        
        public static IEnumerable<Result<TValue>> Either<TValue>(this IEnumerable<Result<TValue>> target, Action<TValue> onSuccess, Action onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess), onError, nameof(onError));
            foreach (Result<TValue> result in target)
            {
                yield return result.OnError(onError);
            }
        }

        public static IEnumerable<Result<TNewValue>> Either<TValue, TNewValue>(this IEnumerable<Result<TValue>> target, Func<TValue, TNewValue> onSuccess, Action onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess), onError, nameof(onError));
            foreach (Result<TValue> result in target)
            {
                yield return result.Either(onSuccess, onError);
            }
        }

        public static IEnumerable<Result<TValue, TNewError>> Either<TValue, TNewError>(this IEnumerable<Result<TValue>> target, Action<TValue> onSuccess, Func<TNewError> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess), onError, nameof(onError));
            foreach (Result<TValue> result in target)
            {
                yield return result.Either(onSuccess, onError);
            }
        }

        public static IEnumerable<Result<TNewValue, TNewError>> Either<TValue, TNewValue, TNewError>(this IEnumerable<Result<TValue>> target, Func<TValue, TNewValue> onSuccess, Func<TNewError> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess), onError, nameof(onError));
            foreach (Result<TValue> result in target)
            {
                yield return result.Either(onSuccess, onError);
            }
        }

        public static IEnumerable<Result<TValue>> Ensure<TValue>(this IEnumerable<Result<TValue>> target, Func<TValue, bool> condition)
        {
            Args.ExceptionIfNull(target, nameof(target), condition, nameof(condition));
            foreach (Result<TValue> result in target)
            {
                yield return result.Ensure(condition);
            }
        }

        public static IEnumerable<T> Match<T, TValue>(this IEnumerable<Result<TValue>> target, Func<TValue, T> onSuccess, Func<T> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess), onError, nameof(onError));
            foreach (Result<TValue> result in target)
            {
                yield return result.Match(onSuccess, onError);
            }
        }
    }
}
