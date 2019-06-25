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
        public static IEnumerable<ResultOrError<TError>> Successes<TError>(this IEnumerable<ResultOrError<TError>> target)
        {
            Args.ExceptionIfNull(target, nameof(target));
            using (IEnumerator<ResultOrError<TError>> enumerator = target.GetEnumerator())
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

        public static IEnumerable<ResultOrError<TError>> Errors<TError>(this IEnumerable<ResultOrError<TError>> target)
        {
            Args.ExceptionIfNull(target, nameof(target));
            using (IEnumerator<ResultOrError<TError>> enumerator = target.GetEnumerator())
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

        public static Result Combine<TError>(this IEnumerable<ResultOrError<TError>> target, bool successIfEmpty = true)
        {
            Args.ExceptionIfNull(target, nameof(target));
            using (IEnumerator<ResultOrError<TError>> enumerator = target.GetEnumerator())
            {
                if (!enumerator.MoveNext())
                {
                    return successIfEmpty;
                }

                enumerator.Reset();

                while (enumerator.MoveNext())
                {
                    if (!enumerator.Current.Match(() => true, _ => false))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static IEnumerable<ResultOrError<TError>> Filter<TError>(this IEnumerable<ResultOrError<TError>> target, Func<bool> successFilter, Func<TError, bool> errorFilter)
        {
            Args.ExceptionIfNull(target, nameof(target), successFilter, nameof(successFilter), errorFilter, nameof(errorFilter));
            using (IEnumerator<ResultOrError<TError>> enumerator = target.GetEnumerator())
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

        public static IEnumerable<Result> AsPlainResults<TError>(this IEnumerable<ResultOrError<TError>> target)
        {
            Args.ExceptionIfNull(target, nameof(target));
            using (IEnumerator<ResultOrError<TError>> enumerator = target.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current;
                }
            }
        }        

        public static IEnumerable<ResultOrError<TError>> Bind<TError>(this IEnumerable<ResultOrError<TError>> target, Func<ResultOrError<TError>> onSuccess)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess));
            using (IEnumerator<ResultOrError<TError>> enumerator = target.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current.Bind(onSuccess);
                }
            }
        }

        public static IEnumerable<Result<TValue, TError>> Bind<TValue, TError>(this IEnumerable<ResultOrError<TError>> target, Func<Result<TValue, TError>> onSuccess)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess));
            using (IEnumerator<ResultOrError<TError>> enumerator = target.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current.Bind(onSuccess);
                }
            }
        }

        public static IEnumerable<TResult> Bind<TError, TResult>(this IEnumerable<ResultOrError<TError>> target, Func<TResult> onSuccess, Func<TError, TResult> onError)
            where TResult : IResult
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess), onError, nameof(onError));
            using (IEnumerator<ResultOrError<TError>> enumerator = target.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current.Bind(onSuccess, onError);
                }
            }
        }

        public static IEnumerable<ResultOrError<TNewError>> BindOnError<TError, TNewError>(this IEnumerable<ResultOrError<TError>> target, Func<TError, ResultOrError<TNewError>> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onError, nameof(onError));
            using (IEnumerator<ResultOrError<TError>> enumerator = target.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current.BindOnError(onError);
                }
            }
        }

        public static IEnumerable<Result> BindOnError<TError>(this IEnumerable<ResultOrError<TError>> target, Func<TError, Result> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onError, nameof(onError));
            using (IEnumerator<ResultOrError<TError>> enumerator = target.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current.BindOnError(onError);
                }
            }
        }

        public static IEnumerable<ResultOrError<TError>> OnSuccess<TError>(this IEnumerable<ResultOrError<TError>> target, Action onSuccess)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess));
            using (IEnumerator<ResultOrError<TError>> enumerator = target.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current.OnSuccess(onSuccess);
                }
            }
        }

        public static IEnumerable<Result<TValue, TError>> OnSuccess<TValue, TError>(this IEnumerable<ResultOrError<TError>> target, Func<TValue> onSuccess)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess));
            using (IEnumerator<ResultOrError<TError>> enumerator = target.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current.OnSuccess(onSuccess);
                }
            }
        }

        public static IEnumerable<ResultOrError<TError>> OnError<TError>(this IEnumerable<ResultOrError<TError>> target, Action<TError> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onError, nameof(onError));
            using (IEnumerator<ResultOrError<TError>> enumerator = target.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current.OnError(onError);
                }
            }
        }

        public static IEnumerable<ResultOrError<TNewError>> OnError<TError, TNewError>(this IEnumerable<ResultOrError<TError>> target, Func<TError, TNewError> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onError, nameof(onError));
            using (IEnumerator<ResultOrError<TError>> enumerator = target.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current.OnError(onError);
                }
            }
        }
        
        public static IEnumerable<ResultOrError<TError>> Either<TError>(this IEnumerable<ResultOrError<TError>> target, Action onSuccess, Action<TError> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess), onError, nameof(onError));
            using (IEnumerator<ResultOrError<TError>> enumerator = target.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current.Either(onSuccess, onError);
                }
            }
        }

        public static IEnumerable<Result<TValue, TError>> Either<TValue, TError>(this IEnumerable<ResultOrError<TError>> target, Func<TValue> onSuccess, Action<TError> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess), onError, nameof(onError));
            using (IEnumerator<ResultOrError<TError>> enumerator = target.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current.Either(onSuccess, onError);
                }
            }
        }

        public static IEnumerable<ResultOrError<TNewError>> Either<TError, TNewError>(this IEnumerable<ResultOrError<TError>> target, Action onSuccess, Func<TError, TNewError> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess), onError, nameof(onError));
            using (IEnumerator<ResultOrError<TError>> enumerator = target.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current.Either(onSuccess, onError);
                }
            }
        }

        public static IEnumerable<Result<TValue, TNewError>> Either<TValue, TError, TNewError>(this IEnumerable<ResultOrError<TError>> target, Func<TValue> onSuccess, Func<TError, TNewError> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess), onError, nameof(onError));
            using (IEnumerator<ResultOrError<TError>> enumerator = target.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current.Either(onSuccess, onError);
                }
            }
        }

        public static IEnumerable<ResultOrError<TError>> Ensure<TError>(this IEnumerable<ResultOrError<TError>> target, Func<bool> condition, Func<TError> errorFunc)
        {
            Args.ExceptionIfNull(target, nameof(target), condition, nameof(condition), errorFunc, nameof(errorFunc));
            using (IEnumerator<ResultOrError<TError>> enumerator = target.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current.Ensure(condition, errorFunc);
                }
            }
        }

        public static IEnumerable<T> Match<T, TError>(this IEnumerable<ResultOrError<TError>> target, Func<T> onSuccess, Func<TError, T> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess), onError, nameof(onError));
            using (IEnumerator<ResultOrError<TError>> enumerator = target.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current.Match(onSuccess, onError);
                }
            }
        }
    }
}
