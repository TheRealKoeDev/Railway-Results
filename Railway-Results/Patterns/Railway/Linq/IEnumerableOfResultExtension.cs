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
            using (IEnumerator<Result> enumerator = target.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    if (enumerator.Current.Match(() => true, () => false))
                    {
                        yield return enumerator.Current;
                    }
                }
            }
        }

        public static IEnumerable<Result> Errors(this IEnumerable<Result> target)
        {
            Args.ExceptionIfNull(target, nameof(target));
            using (IEnumerator<Result> enumerator = target.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    if (enumerator.Current.Match(() => false, () => true))
                    {
                        yield return enumerator.Current;
                    }
                }
            }
        }

        public static IEnumerable<Result> Bind(this IEnumerable<Result> target, Func<Result> onSuccess)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess));
            using (IEnumerator<Result> enumerator = target.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current.Bind(onSuccess);
                }
            }
        }

        public static IEnumerable<TResult> Bind<TResult>(this IEnumerable<Result> target, Func<TResult> onSuccess, Func<TResult> onError)
            where TResult: IResult
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess), onError, nameof(onError));
            using (IEnumerator<Result> enumerator = target.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current.Bind(onSuccess, onError);
                }
            }
        }

        public static IEnumerable<Result> BindOnError(this IEnumerable<Result> target, Func<Result> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onError, nameof(onError));
            using (IEnumerator<Result> enumerator = target.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current.BindOnError(onError);
                }
            }
        }

        public static IEnumerable<ResultWithError<TError>> BindOnError<TError>(this IEnumerable<Result> target, Func<ResultWithError<TError>> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onError, nameof(onError));
            using (IEnumerator<Result> enumerator = target.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current.BindOnError(onError);
                }
            }
        }

        public static IEnumerable<Result> OnSuccess(this IEnumerable<Result> target, Action onSuccess)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess));
            using (IEnumerator<Result> enumerator = target.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current.OnSuccess(onSuccess);
                }
            }
        }

        public static IEnumerable<Result<TValue>> OnSuccess<TValue>(this IEnumerable<Result> target, Func<TValue> onSuccess)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess));
            using (IEnumerator<Result> enumerator = target.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current.OnSuccess(onSuccess);
                }
            }
        }

        public static IEnumerable<Result> OnError(this IEnumerable<Result> target, Action onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onError, nameof(onError));
            using (IEnumerator<Result> enumerator = target.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current.OnError(onError);
                }
            }
        }

        public static IEnumerable<ResultWithError<TError>> OnError<TError>(this IEnumerable<Result> target, Func<TError> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onError, nameof(onError));
            using (IEnumerator<Result> enumerator = target.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current.OnError(onError);
                }
            }
        }

        public static IEnumerable<Result> Either(this IEnumerable<Result> target, Action onSuccess, Action onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess), onError, nameof(onError));
            using (IEnumerator<Result> enumerator = target.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current.Either(onSuccess, onError);
                }
            }
        }

        public static IEnumerable<Result<TValue>> Either<TValue>(this IEnumerable<Result> target, Func<TValue> onSuccess, Action onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess), onError, nameof(onError));
            using (IEnumerator<Result> enumerator = target.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current.Either(onSuccess, onError);
                }
            }
        }

        public static IEnumerable<ResultWithError<TError>> Either<TError>(this IEnumerable<Result> target, Action onSuccess, Func<TError> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess), onError, nameof(onError));
            using (IEnumerator<Result> enumerator = target.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current.Either(onSuccess, onError);
                }
            }
        }

        public static IEnumerable<Result<TValue, TError>> Either<TValue, TError>(this IEnumerable<Result> target, Func<TValue> onSuccess, Func<TError> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess), onError, nameof(onError));
            using (IEnumerator<Result> enumerator = target.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current.Either(onSuccess, onError);
                }
            }
        }

        public static IEnumerable<Result> Ensure(this IEnumerable<Result> target, Func<bool> condition)
        {
            Args.ExceptionIfNull(target, nameof(target), condition, nameof(condition));
            using (IEnumerator<Result> enumerator = target.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current.Ensure(condition);
                }
            }
        }

        public static IEnumerable<T> Match<T>(this IEnumerable<Result> target, Func<T> onSuccess, Func<T> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess), onError, nameof(onError));
            using (IEnumerator<Result> enumerator = target.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current.Match(onSuccess, onError);
                }
            }
        }        
    }
}
