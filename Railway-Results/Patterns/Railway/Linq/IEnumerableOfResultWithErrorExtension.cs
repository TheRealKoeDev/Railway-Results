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
            using (IEnumerator<ResultWithError<TError>> enumerator = target.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    if (enumerator.Current.Match(() => true, error => false))
                    {
                        yield return enumerator.Current;
                    }
                }
            }
        }

        public static IEnumerable<ResultWithError<TError>> Errors<TError>(this IEnumerable<ResultWithError<TError>> target)
        {
            Args.ExceptionIfNull(target, nameof(target));
            using (IEnumerator<ResultWithError<TError>> enumerator = target.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    if (enumerator.Current.Match(() => false, error => true))
                    {
                        yield return enumerator.Current;
                    }
                }
            }
        }

        public static IEnumerable<ResultWithError<TError>> Filter<TError>(this IEnumerable<ResultWithError<TError>> target, Func<bool> successFilter, Func<TError, bool> errorFilter)
        {
            Args.ExceptionIfNull(target, nameof(target), successFilter, nameof(successFilter), errorFilter, nameof(errorFilter));
            using (IEnumerator<ResultWithError<TError>> enumerator = target.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    if (enumerator.Current.Match(successFilter, errorFilter))
                    {
                        yield return enumerator.Current;
                    }
                }
            }
        }

        public static IEnumerable<Result> AsPlainResults<TError>(this IEnumerable<ResultWithError<TError>> target)
        {
            Args.ExceptionIfNull(target, nameof(target));
            using (IEnumerator<ResultWithError<TError>> enumerator = target.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current;
                }
            }
        }        

        public static IEnumerable<ResultWithError<TError>> Bind<TError>(this IEnumerable<ResultWithError<TError>> target, Func<ResultWithError<TError>> onSuccess)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess));
            using (IEnumerator<ResultWithError<TError>> enumerator = target.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current.Bind(onSuccess);
                }
            }
        }

        public static IEnumerable<Result<TValue, TError>> Bind<TValue, TError>(this IEnumerable<ResultWithError<TError>> target, Func<Result<TValue, TError>> onSuccess)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess));
            using (IEnumerator<ResultWithError<TError>> enumerator = target.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current.Bind(onSuccess);
                }
            }
        }

        public static IEnumerable<TResult> Bind<TError, TResult>(this IEnumerable<ResultWithError<TError>> target, Func<TResult> onSuccess, Func<TError, TResult> onError)
            where TResult : IResult
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess), onError, nameof(onError));
            using (IEnumerator<ResultWithError<TError>> enumerator = target.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current.Bind(onSuccess, onError);
                }
            }
        }

        public static IEnumerable<ResultWithError<TNewError>> BindOnError<TError, TNewError>(this IEnumerable<ResultWithError<TError>> target, Func<TError, ResultWithError<TNewError>> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onError, nameof(onError));
            using (IEnumerator<ResultWithError<TError>> enumerator = target.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current.BindOnError(onError);
                }
            }
        }

        public static IEnumerable<Result> BindOnError<TError>(this IEnumerable<ResultWithError<TError>> target, Func<TError, Result> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onError, nameof(onError));
            using (IEnumerator<ResultWithError<TError>> enumerator = target.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current.BindOnError(onError);
                }
            }
        }

        public static IEnumerable<ResultWithError<TError>> OnSuccess<TError>(this IEnumerable<ResultWithError<TError>> target, Action onSuccess)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess));
            using (IEnumerator<ResultWithError<TError>> enumerator = target.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current.OnSuccess(onSuccess);
                }
            }
        }

        public static IEnumerable<Result<TValue, TError>> OnSuccess<TValue, TError>(this IEnumerable<ResultWithError<TError>> target, Func<TValue> onSuccess)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess));
            using (IEnumerator<ResultWithError<TError>> enumerator = target.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current.OnSuccess(onSuccess);
                }
            }
        }

        public static IEnumerable<ResultWithError<TError>> OnError<TError>(this IEnumerable<ResultWithError<TError>> target, Action<TError> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onError, nameof(onError));
            using (IEnumerator<ResultWithError<TError>> enumerator = target.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current.OnError(onError);
                }
            }
        }

        public static IEnumerable<ResultWithError<TNewError>> OnError<TError, TNewError>(this IEnumerable<ResultWithError<TError>> target, Func<TError, TNewError> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onError, nameof(onError));
            using (IEnumerator<ResultWithError<TError>> enumerator = target.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current.OnError(onError);
                }
            }
        }
        
        public static IEnumerable<ResultWithError<TError>> Either<TError>(this IEnumerable<ResultWithError<TError>> target, Action onSuccess, Action<TError> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess), onError, nameof(onError));
            using (IEnumerator<ResultWithError<TError>> enumerator = target.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current.Either(onSuccess, onError);
                }
            }
        }

        public static IEnumerable<Result<TValue, TError>> Either<TValue, TError>(this IEnumerable<ResultWithError<TError>> target, Func<TValue> onSuccess, Action<TError> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess), onError, nameof(onError));
            using (IEnumerator<ResultWithError<TError>> enumerator = target.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current.Either(onSuccess, onError);
                }
            }
        }

        public static IEnumerable<ResultWithError<TNewError>> Either<TError, TNewError>(this IEnumerable<ResultWithError<TError>> target, Action onSuccess, Func<TError, TNewError> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess), onError, nameof(onError));
            using (IEnumerator<ResultWithError<TError>> enumerator = target.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current.Either(onSuccess, onError);
                }
            }
        }

        public static IEnumerable<Result<TValue, TNewError>> Either<TValue, TError, TNewError>(this IEnumerable<ResultWithError<TError>> target, Func<TValue> onSuccess, Func<TError, TNewError> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess), onError, nameof(onError));
            using (IEnumerator<ResultWithError<TError>> enumerator = target.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current.Either(onSuccess, onError);
                }
            }
        }

        public static IEnumerable<ResultWithError<TError>> Ensure<TError>(this IEnumerable<ResultWithError<TError>> target, Func<bool> condition, Func<TError> errorFunc)
        {
            Args.ExceptionIfNull(target, nameof(target), condition, nameof(condition), errorFunc, nameof(errorFunc));
            using (IEnumerator<ResultWithError<TError>> enumerator = target.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current.Ensure(condition, errorFunc);
                }
            }
        }

        public static IEnumerable<T> Match<T, TError>(this IEnumerable<ResultWithError<TError>> target, Func<T> onSuccess, Func<TError, T> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess), onError, nameof(onError));
            using (IEnumerator<ResultWithError<TError>> enumerator = target.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current.Match(onSuccess, onError);
                }
            }
        }
    }
}
