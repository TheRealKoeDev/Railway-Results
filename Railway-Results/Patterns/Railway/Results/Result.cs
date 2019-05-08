using KoeLib.Patterns.Railway.Tools;
using System;
using System.Diagnostics;

namespace KoeLib.Patterns.Railway.Results
{
    /// <summary>
    /// This is a simple Result without Value or Error
    /// </summary>
    /// <seealso cref="KoeLib.Patterns.Railway.Results.IResult" />
    [DebuggerStepThrough]
    public readonly struct Result : IResult
    {
        private readonly bool _isSuccess;

        private Result(bool success)
        {
            _isSuccess = success;            
        }

        public static Result Success()
            => new Result(true);

        public static Result Error()
            => new Result(false);

        public static Result Combine(params Result[] results)
        {
            foreach (Result result in results ?? new Result[0])
            {
                if (!result._isSuccess)
                {
                    return new Result(false);
                }
            }
            return new Result(true);
        }

        public static implicit operator Result(bool success)
            => new Result(success);

        public Result Bind(Func<Result> onSuccess)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess));
            return _isSuccess ? onSuccess() : this;
        }

        public Result<TValue> Bind<TValue>(Func<Result<TValue>> onSuccess)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess));
            return _isSuccess ? onSuccess() : Result<TValue>.Error();
        }

        public TResult Bind<TResult>(Func<TResult> onSuccess, Func<TResult> onError)
           where TResult : IResult
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess), onError, nameof(onError));
            return _isSuccess ? onSuccess() : onError();
        }

        public Result BindOnError(Func<Result> onError)
        {
            Args.ExceptionIfNull(onError, nameof(onError));
            return _isSuccess ? this : onError();
        }

        public ResultWithError<TError> BindOnError<TError>(Func<ResultWithError<TError>> onError)
        {
            Args.ExceptionIfNull(onError, nameof(onError));
            return _isSuccess ? ResultWithError<TError>.Success() : onError();
        }

        public Result OnSuccess(Action onSuccess)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess));
            if (_isSuccess)
            {
                onSuccess();
            }
            return this;
        }

        public Result<TValue> OnSuccess<TValue>(Func<TValue> onSuccess)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess));
            return _isSuccess ? onSuccess() : Result<TValue>.Error();
        }

        public Result OnError(Action onError)
        {
            Args.ExceptionIfNull(onError, nameof(onError));
            if (!_isSuccess)
            {
                onError();
            }
            return this;
        }

        public ResultWithError<TError> OnError<TError>(Func<TError> onError)
        {
            Args.ExceptionIfNull(onError, nameof(onError));
            return _isSuccess ? ResultWithError<TError>.Success() : onError();
        }

        public Result Either(Action onSuccess, Action onError)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess), onError, nameof(onError));
            if (_isSuccess)
            {
                onSuccess();
            }
            else
            {
                onError();
            }
            return this;
        }

        public Result<TValue> Either<TValue>(Func<TValue> onSuccess, Action onError)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess), onError, nameof(onError));
            if (_isSuccess)
            {
                return onSuccess();
            }
            onError();
            return Result<TValue>.Error();
        }

        public ResultWithError<TError> Either<TError>(Action onSuccess, Func<TError> onError)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess), onError, nameof(onError));
            if (_isSuccess)
            {
                onSuccess();
                return ResultWithError<TError>.Success();
            }
            return onError();
        }

        public Result<TValue, TError> Either<TValue, TError>(Func<TValue> onSuccess, Func<TError> onError)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess), onError, nameof(onError));
            return _isSuccess ? Result<TValue, TError>.Success(onSuccess()) : Result<TValue, TError>.Error(onError());
        }

        public T Match<T>(Func<T> onSuccess, Func<T> onError)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess), onError, nameof(onError));
            return _isSuccess ? onSuccess() : onError();
        }

        public Result Ensure(Func<bool> condition)
        {
            Args.ExceptionIfNull(condition, nameof(condition));
            return _isSuccess && condition();
        }

        public Result KeepOnSuccess<T>(Func<T> onSuccess, out T kept)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess));
            kept = _isSuccess ? onSuccess() : default;
            return this;
        }

        public Result KeepOnError<T>(Func<T> onError, out T kept)
        {
            Args.ExceptionIfNull(onError, nameof(onError));
            kept = _isSuccess ? default : onError();
            return this;
        }

        public Result KeepEither<T>(Func<T> onSuccess, Func<T> onError, out T keptValue)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess), onError, nameof(onError));
            keptValue = _isSuccess ? onSuccess() : onError();
            return this;
        }

        public Result KeepEither<T1, T2>(Func<T1> onSuccess, Func<T2> onError, out T1 keptOnSuccess, out T2 keptOnError)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess), onError, nameof(onError));
            keptOnSuccess = _isSuccess ? onSuccess() : default;
            keptOnError = _isSuccess ? default : onError();
            return this;
        }
    }
}