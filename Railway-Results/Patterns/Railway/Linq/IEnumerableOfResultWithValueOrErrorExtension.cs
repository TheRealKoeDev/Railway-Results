﻿using KoeLib.Patterns.Railway.Results;
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
        public static IEnumerable<Result<TValue, TError>> OnSuccess<TValue, TError>(this IEnumerable<Result<TValue, TError>> target, Action<TValue> onSuccess)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess), target, nameof(target));
            foreach (Result<TValue, TError> result in target)
            {
                yield return result.OnSuccess(onSuccess);
            }
        }

        public static IEnumerable<Result<TNewValue, TError>> OnSuccess<TValue, TError, TNewValue>(this IEnumerable<Result<TValue, TError>> target, Func<TValue, TNewValue> onSuccess)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess));
            foreach (Result<TValue, TError> result in target)
            {
                yield return result.OnSuccess(onSuccess);
            }
        }

        public static IEnumerable<Result<TNewValue, TError>> OnSuccess<TValue, TError, TNewValue>(this IEnumerable<Result<TValue, TError>> target, Func<TValue, Result<TNewValue, TError>> onSuccess)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess));
            foreach (Result<TValue, TError> result in target)
            {
                yield return result.OnSuccess(onSuccess);
            }
        }

        public static IEnumerable<ResultWithError<TError>> OnSuccess<TValue, TError>(this IEnumerable<Result<TValue, TError>> target, Func<TValue, ResultWithError<TError>> onSuccess)
        {
            Args.ExceptionIfNull(target, nameof(target), onSuccess, nameof(onSuccess));
            foreach (Result<TValue, TError> result in target)
            {
                yield return result.OnSuccess(onSuccess);
            }
        }

        public static IEnumerable<Result<TValue, TError>> OnError<TValue, TError>(this IEnumerable<Result<TValue, TError>> target, Action<TError> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onError, nameof(onError));
            foreach (Result<TValue, TError> result in target)
            {
                yield return result.OnError(onError);
            }
        }
        public static IEnumerable<Result<TValue, TNewError>> OnError<TValue, TError, TNewError>(this IEnumerable<Result<TValue, TError>> target, Func<TError, TNewError> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onError, nameof(onError));
            foreach (Result<TValue, TError> result in target)
            {
                yield return result.OnError(onError);
            }
        }

        public static IEnumerable<Result<TValue>> OnError<TValue, TError>(this IEnumerable<Result<TValue, TError>> target, Func<TError, Result<TValue>> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onError, nameof(onError));
            foreach (Result<TValue, TError> result in target)
            {
                yield return result.OnError(onError);
            }
        }
        public static IEnumerable<Result<TValue, TNewError>> OnError<TValue, TError, TNewError>(this IEnumerable<Result<TValue, TError>> target, Func<TError, Result<TValue, TNewError>> onError)
        {
            Args.ExceptionIfNull(target, nameof(target), onError, nameof(onError));
            foreach (Result<TValue, TError> result in target)
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
