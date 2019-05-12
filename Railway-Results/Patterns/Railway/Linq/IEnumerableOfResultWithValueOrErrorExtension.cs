using KoeLib.Patterns.Railway.Results;
using KoeLib.Patterns.Railway.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace KoeLib.Patterns.Railway.Linq
{
    [DebuggerStepThrough]
    public static class IEnumerableOfResultOfValueErrorExtension
    {
        public static IEnumerable<Result<TValue, TError>> Successes<TValue, TError>(this IEnumerable<Result<TValue, TError>> target)
        {
            Args.ExceptionIfNull(target, nameof(target));
            using (IEnumerator<Result<TValue, TError>> enumerator = target.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    if (enumerator.Current.Match(value => true, error => false))
                    {
                        yield return enumerator.Current;
                    }                   
                }
            }
        }

        public static IEnumerable<Result<TValue, TError>> Errors<TValue, TError>(this IEnumerable<Result<TValue, TError>> target)
        {
            Args.ExceptionIfNull(target, nameof(target));
            using (IEnumerator<Result<TValue, TError>> enumerator = target.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    if (enumerator.Current.Match(value => false, error => true))
                    {
                        yield return enumerator.Current;
                    }
                }
            }
        }

        public static IEnumerable<Result<TValue, TError>> Filter<TValue, TError>(this IEnumerable<Result<TValue, TError>> target, Func<TValue, bool> successFilter, Func<TError, bool> errorFilter)
        {
            Args.ExceptionIfNull(target, nameof(target), successFilter, nameof(successFilter), errorFilter, nameof(errorFilter));
            using (IEnumerator<Result<TValue, TError>> enumerator = target.GetEnumerator())
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

        public static IEnumerable<Result> AsPlainResults<TValue, TError>(this IEnumerable<Result<TValue, TError>> target)
        {
            Args.ExceptionIfNull(target, nameof(target));
            using (IEnumerator<Result<TValue, TError>> enumerator = target.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current;
                }
            }
        }

        public static IEnumerable<Result<TValue>> AsResultsWithValue<TValue, TError>(this IEnumerable<Result<TValue, TError>> target)
        {
            Args.ExceptionIfNull(target, nameof(target));
            using (IEnumerator<Result<TValue, TError>> enumerator = target.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current;
                }
            }
        }

        public static IEnumerable<ResultWithError<TError>> AsResultsWithError<TValue, TError>(this IEnumerable<Result<TValue, TError>> target)
        {
            Args.ExceptionIfNull(target, nameof(target));
            using (IEnumerator<Result<TValue, TError>> enumerator = target.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current;
                }
            }
        }

        public static IEnumerable<Result<TNewValue, TError>> Bind<TValue, TError, TNewValue>(this IEnumerable<Result<TValue, TError>> target, Func<TValue, Result<TNewValue, TError>> onSuccess)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess));
            using (IEnumerator<Result<TValue, TError>> enumerator = target.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current.Bind(onSuccess);
                }
            }
        }

        public static IEnumerable<ResultWithError<TError>> Bind<TValue, TError>(this IEnumerable<Result<TValue, TError>> target, Func<TValue, ResultWithError<TError>> onSuccess)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess));
            using (IEnumerator<Result<TValue, TError>> enumerator = target.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current.Bind(onSuccess);
                }
            }
        }

        public static IEnumerable<TResult> Bind<TValue, TError, TResult>(this IEnumerable<Result<TValue, TError>> target, Func<TValue, TResult> onSuccess, Func<TError, TResult> onError)
            where TResult : IResult
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess), onError, nameof(onError));
            using (IEnumerator<Result<TValue, TError>> enumerator = target.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current.Bind(onSuccess, onError);
                }
            }
        }

        public static IEnumerable<Result<TValue>> BindOnError<TValue, TError>(this IEnumerable<Result<TValue, TError>> target, Func<TError, Result<TValue>> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onError, nameof(onError));
            using (IEnumerator<Result<TValue, TError>> enumerator = target.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current.BindOnError(onError);
                }
            }
        }

        public static IEnumerable<Result<TValue, TNewError>> BindOnError<TValue, TError, TNewError>(this IEnumerable<Result<TValue, TError>> target, Func<TError, Result<TValue, TNewError>> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onError, nameof(onError));
            using (IEnumerator<Result<TValue, TError>> enumerator = target.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current.BindOnError(onError);
                }
            }
        }
        
        public static IEnumerable<Result<TValue, TError>> OnSuccess<TValue, TError>(this IEnumerable<Result<TValue, TError>> target, Action<TValue> onSuccess)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess));
            using (IEnumerator<Result<TValue, TError>> enumerator = target.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current.OnSuccess(onSuccess);
                }
            }
        }

        public static IEnumerable<Result<TNewValue, TError>> OnSuccess<TValue, TError, TNewValue>(this IEnumerable<Result<TValue, TError>> target, Func<TValue, TNewValue> onSuccess)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess));
            using (IEnumerator<Result<TValue, TError>> enumerator = target.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current.OnSuccess(onSuccess);
                }
            }
        }

        public static IEnumerable<Result<TValue, TError>> OnError<TValue, TError>(this IEnumerable<Result<TValue, TError>> target, Action<TError> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onError, nameof(onError));
            using (IEnumerator<Result<TValue, TError>> enumerator = target.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current.OnError(onError);
                }
            }
        }

        public static IEnumerable<Result<TValue, TNewError>> OnError<TValue, TError, TNewError>(this IEnumerable<Result<TValue, TError>> target, Func<TError, TNewError> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onError, nameof(onError));
            using (IEnumerator<Result<TValue, TError>> enumerator = target.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current.OnError(onError);
                }
            }
        }        

        public static IEnumerable<Result<TValue, TError>> Either<TValue, TError>(this IEnumerable<Result<TValue, TError>> target, Action<TValue> onSuccess, Action<TError> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess), onError, nameof(onError));
            using (IEnumerator<Result<TValue, TError>> enumerator = target.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current.Either(onSuccess, onError);
                }
            }
        }

        public static IEnumerable<Result<TNewValue, TError>> Either<TValue, TError, TNewValue>(this IEnumerable<Result<TValue, TError>> target, Func<TValue, TNewValue> onSuccess, Action<TError> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess), onError, nameof(onError));
            using (IEnumerator<Result<TValue, TError>> enumerator = target.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current.Either(onSuccess, onError);
                }
            }
        }

        public static IEnumerable<Result<TValue, TNewError>> Either<TValue, TError, TNewError>(this IEnumerable<Result<TValue, TError>> target, Action<TValue> onSuccess, Func<TError, TNewError> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess), onError, nameof(onError));
            using (IEnumerator<Result<TValue, TError>> enumerator = target.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current.Either(onSuccess, onError);
                }
            }
        }

        public static IEnumerable<Result<TNewValue, TNewError>> Either<TValue, TError, TNewValue, TNewError>(this IEnumerable<Result<TValue, TError>> target, Func<TValue, TNewValue> onSuccess, Func<TError, TNewError> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess), onError, nameof(onError));
            using (IEnumerator<Result<TValue, TError>> enumerator = target.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current.Either(onSuccess, onError);
                }
            }
        }

        public static IEnumerable<Result<TValue, TError>> Ensure<TValue, TError>(this IEnumerable<Result<TValue, TError>> target, Func<TValue, bool> condition, Func<TError> errorFunc)
        {
            Args.ExceptionIfNull(target, nameof(target), condition, nameof(condition), errorFunc, nameof(errorFunc));
            using (IEnumerator<Result<TValue, TError>> enumerator = target.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current.Ensure(condition, errorFunc);
                }
            }
        }

        public static IEnumerable<T> Match<TValue, TError, T>(this IEnumerable<Result<TValue, TError>> target, Func<TValue, T> onSuccess, Func<TError, T> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess), onError, nameof(onError));
            using (IEnumerator<Result<TValue, TError>> enumerator = target.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current.Match(onSuccess, onError);
                }
            }
        }
    }
}
