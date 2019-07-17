using KoeLib.Patterns.Railway.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace KoeLib.Patterns.Railway.Results
{
    [DebuggerStepThrough]
    public static class ResultOrError
    {
        public static ResultOrError<TError> Create<TError>(bool success, Func<TError> errorFunc)
        {
            Args.ExceptionIfNull(errorFunc, nameof(errorFunc));
            return success ? ResultOrError<TError>.Success() : ResultOrError<TError>.Error(errorFunc());
        }
    }

    /// <summary>
    /// Is a Monad without Value but with Error.
    /// The DefaultValue of this struct is a <see cref="Error(TError)"/> with the default value of <typeparamref name="TError"/>.
    /// </summary>
    /// <typeparam name="TError">The type of the Error.</typeparam>
    /// <seealso cref="KoeLib.Patterns.Railway.Results.IResult" />
    [DebuggerStepThrough]
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct ResultOrError<TError> : IResult
    {
        private readonly bool _isSuccess;

        private readonly TError _error;

        private ResultOrError(bool isSuccess)
        {
            _error = default;
            _isSuccess = isSuccess;
        }

        private ResultOrError(TError error)
        {            
            _error = error;
            _isSuccess = false;
        }

        public static ResultOrError<TError> Success()
            => new ResultOrError<TError>(true);

        public static ResultOrError<TError> Error(TError error)
            => new ResultOrError<TError>(error);

        public static implicit operator ResultOrError<TError>(TError error)
            => new ResultOrError<TError>(error);

        public static implicit operator Result(ResultOrError<TError> result)
            => result._isSuccess;

        public Result AsPlainResult() => this;

        public ResultOrError<TError> Bind(Func<ResultOrError<TError>> onSuccess)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess));
            return _isSuccess ? onSuccess() : this;
        }

        public Result<TValue, TError> Bind<TValue>(Func<Result<TValue, TError>> onSuccess)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess));
            return _isSuccess ? onSuccess() : _error;
        }

        public TResult Bind<TResult>(Func<TResult> onSuccess, Func<TError, TResult> onError)
            where TResult : IResult
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess), onError, nameof(onError));
            return _isSuccess ? onSuccess() : onError(_error);
        }

        public ResultOrError<TNewError> BindOnError<TNewError>(Func<TError, ResultOrError<TNewError>> onError)
        {
            Args.ExceptionIfNull(onError, nameof(onError));
            return _isSuccess ? new ResultOrError<TNewError>(true) : onError(_error);
        }

        public Result BindOnError(Func<TError, Result> onError)
        {
            Args.ExceptionIfNull(onError, nameof(onError));
            return _isSuccess ? Result.Success() : onError(_error);
        }

        public ResultOrError<TError> OnSuccess(Action onSuccess)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess));
            if (_isSuccess)
            {
                onSuccess();
            }
            return this;
        }

        public Result<TValue, TError> OnSuccess<TValue>(Func<TValue> onSuccess)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess));
            return _isSuccess ? onSuccess() : Result<TValue, TError>.Error(_error);
        }

        public ResultOrError<TError> FixOnError(Action<TError> onError)
        {
            Args.ExceptionIfNull(onError, nameof(onError));
            if (!_isSuccess)
            {
                onError(_error);
            }
            return Success();
        }

        public ResultOrError<TError> OnError(Action<TError> onError)
        {
            Args.ExceptionIfNull(onError, nameof(onError));
            if (!_isSuccess)
            {
                onError(_error);
            }
            return this;
        }

        public ResultOrError<TNewError> OnError<TNewError>(Func<TError, TNewError> onError)
        {
            Args.ExceptionIfNull(onError, nameof(onError));
            return _isSuccess ? new ResultOrError<TNewError>(true) : onError(_error);
        }

        public ResultOrError<TError> Either(Action onSuccess, Action<TError> onError)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess), onError, nameof(onError));
            if (_isSuccess)
            {
                onSuccess();
            }
            else
            {
                onError(_error);
            }
            return this;
        }

        public Result<TValue, TError> Either<TValue>(Func<TValue> onSuccess, Action<TError> onError)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess), onError, nameof(onError));
            if (_isSuccess)
            {
                return onSuccess();
            }
            onError(_error);
            return _error;
        }

        public ResultOrError<TNewError> Either<TNewError>(Action onSuccess, Func<TError, TNewError> onError)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess), onError, nameof(onError));
            if (_isSuccess)
            {
                onSuccess();
                return new ResultOrError<TNewError>(true);
            }
            return onError(_error);
        }

        public Result<TValue, TNewError> Either<TValue, TNewError>(Func<TValue> onSuccess, Func<TError, TNewError> onError)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess), onError, nameof(onError));
            return _isSuccess ? Result<TValue, TNewError>.Success(onSuccess()) : onError(_error);
        }

        public T Match<T>(Func<T> onSuccess, Func<TError, T> onError)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess), onError, nameof(onError));
            return _isSuccess ? onSuccess() : onError(_error);
        }

        public ResultOrError<TError> Ensure(Func<bool> condition, Func<TError> error)
        {
            Args.ExceptionIfNull(condition, nameof(condition), error, nameof(error));
            if (!_isSuccess)
            {
                return this;
            }
            else if (!condition())
            {
                return error();
            }
            return this;
        }

        public ResultOrError<TError> KeepOnSuccess<T>(Func<T> onSuccess, out T kept)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess));
            kept = _isSuccess ? onSuccess() : default;
            return this;
        }

        public ResultOrError<TError> KeepOnError<T>(Func<TError, T> onError, out T kept)
        {
            Args.ExceptionIfNull(onError, nameof(onError));
            kept = _isSuccess ?  default : onError(_error);
            return this;
        }

        public ResultOrError<TError> KeepEither<T>(Func<T> onSuccess, Func<TError, T> onError, out T kept)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess), onError, nameof(onError));
            kept = _isSuccess ? onSuccess() : onError(_error);
            return this;
        }

        public ResultOrError<TError> KeepEither<T1, T2>(Func<T1> onSuccess, Func<TError, T2> onError, out T1 keptOnSuccess, out T2 keptOnError)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess), onError, nameof(onError));
            keptOnSuccess = _isSuccess ? onSuccess() : default;
            keptOnError = _isSuccess ? default : onError(_error);
            return this;
        }  
    }
}
