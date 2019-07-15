using KoeLib.Patterns.Railway.Tools;
using System;
using System.Diagnostics;

namespace KoeLib.Patterns.Railway.Results
{
    /// <summary>
    /// Is a Monad with Value.
    /// The DefaultValue of this struct has no Value and represents a failed Result.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <seealso cref="KoeLib.Patterns.Railway.Results.IResult" />
    [DebuggerStepThrough]
    public readonly struct Result<TValue> : IResult
    {
        private readonly bool _isSuccess;

        private readonly TValue _value;

        private Result(TValue value)
        {
            _isSuccess = true;
            _value = value;
        }        

        public static Result<TValue> Success(TValue value)
            => new Result<TValue>(value);

        public static Result<TValue> Error()
            => new Result<TValue>();

        public static implicit operator Result<TValue>(TValue value)
            => new Result<TValue>(value);

        public static implicit operator Result(Result<TValue> result)
            => result._isSuccess;
        public Result AsPlainResult() => this;

        public Result Bind(Func<TValue, Result> onSuccess)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess));
            return _isSuccess ? onSuccess(_value) : Result.Error();
        }

        public Result<TNewValue> Bind<TNewValue>(Func<TValue, Result<TNewValue>> onSuccess)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess));
            return _isSuccess ? onSuccess(_value) : new Result<TNewValue>();
        }

        public TResult Bind<TResult>(Func<TValue, TResult> onSuccess, Func<TResult> onError)
            where TResult : IResult
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess), onError, nameof(onError));
            return _isSuccess ? onSuccess(_value) : onError();
        }

        public Result<TValue> BindOnError(Func<Result<TValue>> onError)
        {
            Args.ExceptionIfNull(onError, nameof(onError));
            return _isSuccess ? this : onError();
        }

        public Result<TValue, TError> BindOnError<TError>(Func<Result<TValue, TError>> onError)
        {
            Args.ExceptionIfNull(onError, nameof(onError));
            return _isSuccess ? Result<TValue, TError>.Success(_value) : onError();
        }

        public Result<TValue> Ensure(Func<TValue, bool> condition)
        {
            Args.ExceptionIfNull(condition, nameof(condition));
            return _isSuccess && condition(_value) ? this : new Result<TValue>();
        }

        public Result<TValue> KeepOnSuccess<T>(Func<TValue, T> onSuccess, out T kept)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess));
            kept = _isSuccess ? onSuccess(_value) : default;
            return this;
        }

        public Result<TValue> KeepOnError<T>(Func<T> onError, out T kept)
        {
            Args.ExceptionIfNull(onError, nameof(onError));
            kept = _isSuccess ? default : onError();
            return this;
        }

        public Result<TValue> KeepEither<T>(Func<TValue, T> onSuccess, Func<T> onError, out T kept)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess), onError, nameof(onError));
            kept = _isSuccess ? onSuccess(_value) : onError();
            return this;
        }

        public Result<TValue> KeepEither<T1, T2>(Func<TValue, T1> onSuccess, Func<T2> onError, out T1 keptOnSuccess, out T2 keptOnError)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess), onError, nameof(onError));
            keptOnSuccess = _isSuccess ? onSuccess(_value) : default;
            keptOnError = _isSuccess ? default : onError();
            return this;
        }

        public Result<TValue> OnSuccess(Action<TValue> onSuccess)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess));
            if (_isSuccess)
            {
                onSuccess(_value);
            }
            return this;
        }

        public Result<TNewValue> OnSuccess<TNewValue>(Func<TValue, TNewValue> onSuccess)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess));
            return _isSuccess ? onSuccess(_value) : new Result<TNewValue>();
        }

        public Result<TValue> FixOnError(Func<TValue> onError)
        {
            Args.ExceptionIfNull(onError, nameof(onError));
            return _isSuccess ? this : onError();
        }

        public Result<TValue> OnError(Action onError)
        {
            Args.ExceptionIfNull(onError, nameof(onError));
            if (_isSuccess)
            {
                onError();
            }
            return this;
        }

        public Result<TValue, TError> OnError<TError>(Func<TError> onError)
        {
            Args.ExceptionIfNull(onError, nameof(onError));
            return _isSuccess ? Result<TValue, TError>.Success(_value) : onError();
        }

        public Result<TValue> Either(Action<TValue> onSuccess, Action onError)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess), onError, nameof(onError));
            if (_isSuccess)
            {
                onSuccess(_value);
            }
            else
            {
                onError();
            }
            return this;
        }

        public Result<TNewValue> Either<TNewValue>(Func<TValue, TNewValue> onSuccess, Action onError)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess), onError, nameof(onError));
            if (_isSuccess)
            {
                return onSuccess(_value);
            }
            onError();
            return new Result<TNewValue>();
        }

        public Result<TValue, TNewError> Either<TNewError>(Action<TValue> onSuccess, Func<TNewError> onError)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess), onError, nameof(onError));
            if (_isSuccess)
            {
                onSuccess(_value);
                return Result<TValue, TNewError>.Success(_value);
            }
            return onError();
        }

        public Result<TNewValue, TNewError> Either<TNewValue, TNewError>(Func<TValue, TNewValue> onSuccess, Func<TNewError> onError)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess), onError, nameof(onError));
            return _isSuccess ? Result<TNewValue, TNewError>.Success(onSuccess(_value)) : onError();
        }        

        public T Match<T>(Func<TValue, T> onSuccess, Func<T> onError)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess), onError, nameof(onError));
            return _isSuccess ? onSuccess(_value) : onError();
        }
    }
}