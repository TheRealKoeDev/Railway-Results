using KoeLib.Patterns.Railway.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace KoeLib.Patterns.Railway.Results
{
    /// <summary>
    /// Is a Monad without Value but with Error.
    /// The DefaultValue of this struct has no Error and represents a successful Result.
    /// </summary>
    /// <typeparam name="TError">The type of the Error.</typeparam>
    /// <seealso cref="KoeLib.Patterns.Railway.Results.IResult" />
    [DebuggerStepThrough]
    public readonly struct ResultWithError<TError> : IResult
    {
        private readonly bool _isError;

        private readonly TError _error;

        private ResultWithError(TError error)
        {            
            _error = error;
            _isError = true;
        }

        public static ResultWithError<TError> Success()
            => new ResultWithError<TError>();

        public static ResultWithError<TError> Error(TError error)
            => new ResultWithError<TError>(error);

        public static implicit operator ResultWithError<TError>(TError error)
            => new ResultWithError<TError>(error);

        public static implicit operator Result(ResultWithError<TError> result)
            => !result._isError;

        public Result AsPlainResult() => this;

        public ResultWithError<TError> Bind(Func<ResultWithError<TError>> onSuccess)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess));
            return _isError ? this : onSuccess();
        }

        public Result<TValue, TError> Bind<TValue>(Func<Result<TValue, TError>> onSuccess)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess));
            return _isError ? _error : onSuccess();
        }

        public TResult Bind<TResult>(Func<TResult> onSuccess, Func<TError, TResult> onError)
            where TResult : IResult
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess), onError, nameof(onError));
            return _isError ? onError(_error) : onSuccess();
        }

        public ResultWithError<TNewError> BindOnError<TNewError>(Func<TError, ResultWithError<TNewError>> onError)
        {
            Args.ExceptionIfNull(onError, nameof(onError));
            return _isError ? onError(_error) : new ResultWithError<TNewError>();
        }

        public Result BindOnError(Func<TError, Result> onError)
        {
            Args.ExceptionIfNull(onError, nameof(onError));
            return _isError ? onError(_error) : Result.Success();
        }

        public ResultWithError<TError> OnSuccess(Action onSuccess)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess));
            if (!_isError)
            {
                onSuccess();
            }
            return this;
        }

        public Result<TValue, TError> OnSuccess<TValue>(Func<TValue> onSuccess)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess));
            return _isError ? Result<TValue, TError>.Error(_error) : onSuccess();
        }

        public ResultWithError<TError> OnError(Action<TError> onError)
        {
            Args.ExceptionIfNull(onError, nameof(onError));
            if (_isError)
            {
                onError(_error);
            }
            return this;
        }

        public ResultWithError<TNewError> OnError<TNewError>(Func<TError, TNewError> onError)
        {
            Args.ExceptionIfNull(onError, nameof(onError));
            return _isError ? onError(_error) : new ResultWithError<TNewError>();
        }

        public ResultWithError<TError> Either(Action onSuccess, Action<TError> onError)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess), onError, nameof(onError));
            if (_isError)
            {
                onError(_error);
            }
            else
            {
                onSuccess();
            }
            return this;
        }

        public Result<TValue, TError> Either<TValue>(Func<TValue> onSuccess, Action<TError> onError)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess), onError, nameof(onError));
            if (_isError)
            {
                onError(_error);
                return _error;
            }
            return onSuccess();
        }

        public ResultWithError<TNewError> Either<TNewError>(Action onSuccess, Func<TError, TNewError> onError)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess), onError, nameof(onError));
            if (_isError)
            {
                return onError(_error);
            }
            onSuccess();
            return new ResultWithError<TNewError>();
        }

        public Result<TValue, TNewError> Either<TValue, TNewError>(Func<TValue> onSuccess, Func<TError, TNewError> onError)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess), onError, nameof(onError));
            return _isError ? onError(_error) : Result<TValue, TNewError>.Success(onSuccess());
        }

        public T Match<T>(Func<T> onSuccess, Func<TError, T> onError)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess), onError, nameof(onError));
            return _isError ? onError(_error) : onSuccess();
        }

        public ResultWithError<TError> Ensure(Func<bool> condition, Func<TError> error)
        {
            Args.ExceptionIfNull(condition, nameof(condition), error, nameof(error));
            if (_isError)
            {
                return this;
            }
            else if (!condition())
            {
                return error();
            }
            return this;
        }

        public ResultWithError<TError> KeepOnSuccess<T>(Func<T> onSuccess, out T kept)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess));
            kept = _isError ? default : onSuccess();
            return this;
        }

        public ResultWithError<TError> KeepOnError<T>(Func<TError, T> onError, out T kept)
        {
            Args.ExceptionIfNull(onError, nameof(onError));
            kept = _isError ? onError(_error) : default;
            return this;
        }

        public ResultWithError<TError> KeepEither<T>(Func<T> onSuccess, Func<TError, T> onError, out T kept)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess), onError, nameof(onError));
            kept = _isError ? onError(_error) : onSuccess();
            return this;
        }

        public ResultWithError<TError> KeepEither<T1, T2>(Func<T1> onSuccess, Func<TError, T2> onError, out T1 keptOnSuccess, out T2 keptOnError)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess), onError, nameof(onError));
            keptOnSuccess = _isError ? default : onSuccess();
            keptOnError = _isError ? onError(_error) : default;
            return this;
        }  
    }
}
