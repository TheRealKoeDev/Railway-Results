using KoeLib.Patterns.Railway.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace KoeLib.Patterns.Railway.Results
{
    /// <summary>
    /// For the creation of a <see cref="ResultOrError{TError}"/> with <see cref="Create{TError}(bool, Func{TError})"/> without specifying generics.
    /// </summary>
    [DebuggerStepThrough]
    public static class ResultOrError
    {
        /// <summary>
        /// Creates a instance of <see cref="ResultOrError{TError}"/>.
        /// </summary>
        /// <typeparam name="TError"></typeparam>
        /// <param name="success"></param>
        /// <param name="errorFunc"></param>
        /// <returns></returns>
        public static ResultOrError<TError> Create<TError>(bool success, Func<TError> errorFunc)
        {
            Args.ExceptionIfNull(errorFunc, nameof(errorFunc));
            return success ? ResultOrError<TError>.Success() : ResultOrError<TError>.Error(errorFunc());
        }
    }

    /// <summary>
    /// A Result/Monad with Error but without Value, that can either be a <see cref="Success"/> or a <see cref="Error(TError)"/>.
    /// <para>The default value of <see cref="ResultOrError{TError}"/> is a <see cref="Error(TError)"/> with a default value for <typeparamref name="TError"/>.</para>
    /// </summary>
    /// <typeparam name="TError">The type of the Error.</typeparam>
    /// <seealso cref="IResult" />
    [DebuggerStepThrough]
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct ResultOrError<TError> : IResult
    {
        /// <summary>
        /// Is True if this is a <see cref="Success"/> or False if this is a <see cref="Error(TError)"/>.
        /// </summary>
        private readonly bool _isSuccess;

        /// <summary>
        /// The Error.
        /// </summary>
        private readonly TError _error;

        /// <summary>
        /// Creates a <see cref="ResultOrError{TError}"/> with a default value for <typeparamref name="TError"/>.
        /// </summary>
        /// <param name="isSuccess"> Is True if this is a <see cref="Success"/> or False if this is a <see cref="Error(TError)"/>.</param>
        private ResultOrError(bool isSuccess)
        {
            _error = default;
            _isSuccess = isSuccess;
        }

        /// <summary>
        /// Creates a <see cref="Error(TError)"/> with the <paramref name="error"/> as error value.
        /// </summary>
        /// <param name="error">The Error.</param>
        private ResultOrError(TError error)
        {            
            _error = error;
            _isSuccess = false;
        }

        /// <summary>
        /// Creates a Success <see cref="ResultOrError{TError}"/>.
        /// </summary>
        /// <returns></returns>
        public static ResultOrError<TError> Success()
            => new ResultOrError<TError>(true);

        /// <summary>
        /// Creates a Error <see cref="ResultOrError{TError}"/> with the Error <paramref name="error"/>.
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        public static ResultOrError<TError> Error(TError error)
            => new ResultOrError<TError>(error);

        /// <summary>
        /// implicitly converts a <typeparamref name="TError"/> to a <see cref="Error(TError)"/>.
        /// </summary>
        /// <param name="error">The Error.</param>
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
            return _isSuccess ? ResultOrError<TNewError>.Success() : onError(_error);
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
            return _isSuccess ? ResultOrError<TNewError>.Success() : onError(_error);
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
                return ResultOrError<TNewError>.Success();
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

        public ResultOrError<TError> KeepOnError(out TError keptError)
        {
            keptError = _isSuccess ? default : _error;
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
