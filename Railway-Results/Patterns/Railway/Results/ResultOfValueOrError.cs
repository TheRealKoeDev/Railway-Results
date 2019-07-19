using KoeLib.Patterns.Railway.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace KoeLib.Patterns.Railway.Results
{
    /// <summary>
    /// 
    /// </summary>
    [DebuggerStepThrough]
    public static class ResultOfValueOrErrorExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TValueError"></typeparam>
        /// <typeparam name="TNewValueError"></typeparam>
        /// <param name="target"></param>
        /// <param name="func"></param>
        /// <returns></returns>       
        public static Result<TNewValueError, TNewValueError> Allways<TValueError, TNewValueError>(this Result<TValueError, TValueError> target, Func<TValueError, TNewValueError> func)
        {
            Args.ExceptionIfNull(func, nameof(func));
            return target.Either(func, func);
        }
    }

    /// <summary>
    /// A Result/Monad with Value and Error, that can either be a <see cref="Success(TValue)"/> or a <see cref="Error(TError)"/>.
    /// <para>The default value of <see cref="Result{TValue, TError}"/> is a <see cref="Error(TError)"/> with the default value for <typeparamref name="TError"/>.</para>
    /// </summary>
    /// <typeparam name="TValue">The type of the Value.</typeparam>
    /// <typeparam name="TError">The type of the Error.</typeparam>
    /// <seealso cref="IResult" />   
    [DebuggerStepThrough]
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct Result<TValue, TError> : IResult
    {
        private readonly bool _isSuccess;

        private readonly TValue _value;

        private readonly TError _error;

        private Result(TValue value)
        {
            _isSuccess = true;
            _error = default;
            _value = value;
        }

        private Result(TError error)
        {
            _isSuccess = false;
            _value = default;
            _error = error;
        }

        public static Result<TValue, TError> Success(TValue value)
            => new Result<TValue, TError>(value);

        public static Result<TValue, TError> Error(TError error)
            => new Result<TValue, TError>(error);

        public static implicit operator Result<TValue, TError>(TValue value)
            => new Result<TValue, TError>(value);

        public static implicit operator Result<TValue, TError>(TError error)
            => new Result<TValue, TError>(error);

        public static implicit operator Result(Result<TValue, TError> result)
            => result._isSuccess;

        public static implicit operator Result<TValue>(Result<TValue, TError> result)
            => result._isSuccess ? result._value : Result<TValue>.Error();

        public static implicit operator ResultOrError<TError>(Result<TValue, TError> result)
            => result._isSuccess ? ResultOrError<TError>.Success() : result._error;       

        public Result AsPlainResult() => this;
        public Result<TValue> AsResultOfValue() => this;
        public ResultOrError<TError> AsResultOrError() => this;

        public ResultOrError<TError> Bind(Func<TValue, ResultOrError<TError>> onSuccess)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess));
            return _isSuccess ? onSuccess(_value) : _error;
        }

        public Result<TNewValue, TError> Bind<TNewValue>(Func<TValue, Result<TNewValue, TError>> onSuccess)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess));
            return _isSuccess ? onSuccess(_value) : _error;
        }

        public TResult Bind<TResult>(Func<TValue, TResult> onSuccess, Func<TError, TResult> onError)
           where TResult : IResult
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess), onError, nameof(onError));
            return _isSuccess ? onSuccess(_value) : onError(_error);
        }

        public Result<TValue> BindOnError(Func<TError, Result<TValue>> onError)
        {
            Args.ExceptionIfNull(onError, nameof(onError));
            return _isSuccess ? _value : onError(_error);
        }

        public Result<TValue, TNewError> BindOnError<TNewError>(Func<TError, Result<TValue, TNewError>> onError)
        {
            Args.ExceptionIfNull(onError, nameof(onError));
            return _isSuccess ? _value : onError(_error);
        }        

        public Result<TValue, TError> Ensure(Func<TValue, bool> condition, Func<TError> error)
        {
            Args.ExceptionIfNull(condition, nameof(condition), error, nameof(error));
            if (!_isSuccess)
            {
                return this;
            }
            else if (!condition(_value))
            {
                return error();
            }
            return this;
        }

        public Result<TValue, TError> KeepOnSuccess(out TValue keptOnSuccess)
        {
            keptOnSuccess = _isSuccess ? _value : default;
            return this;
        }

        public Result<TValue, TError> KeepOnSuccess<T>(Func<TValue, T> onSuccess, out T kept)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess));
            kept = _isSuccess ? onSuccess(_value) : default;
            return this;
        }

        public Result<TValue, TError> KeepOnError(out TError kept)
        {
            kept = _isSuccess ? default : _error;
            return this;
        }

        public Result<TValue, TError> KeepOnError<T>(Func<TError, T> onError, out T kept)
        {
            Args.ExceptionIfNull(onError, nameof(onError));
            kept = _isSuccess ? default : onError(_error);
            return this;
        }

        public Result<TValue, TError> KeepEither(out TValue keptOnSuccess, out TError keptOnError)
        {
            keptOnSuccess = _isSuccess ? _value : default;
            keptOnError = _isSuccess ? default : _error;
            return this;
        }

        public Result<TValue, TError> KeepEither<T>(Func<TValue, T> onSuccess, Func<TError, T> onError, out T kept)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess), onError, nameof(onError));
            kept = _isSuccess ? onSuccess(_value) : onError(_error);
            return this;
        }

        public Result<TValue, TError> KeepEither<T1, T2>(Func<TValue, T1> onSuccess, Func<TError, T2> onError, out T1 keptOnSuccess, out T2 keptOnError)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess), onError, nameof(onError));
            keptOnSuccess = _isSuccess ? onSuccess(_value) : default;
            keptOnError = _isSuccess ? default : onError(_error);
            return this;
        }

        public Result<TValue, TError> OnSuccess(Action<TValue> onSuccess)
        {
            Args.ExceptionIfNull( onSuccess, nameof(onSuccess));
            if (_isSuccess)
            {
                onSuccess(_value);
            }
            return this;
        }          

        public Result<TNewValue, TError> OnSuccess<TNewValue>(Func<TValue, TNewValue> onSuccess)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess));
            return _isSuccess ? new Result<TNewValue, TError>(onSuccess(_value)) : _error;
        }

        public Result<TValue, TError> FixOnError(Func<TError, TValue> onError)
        {
            Args.ExceptionIfNull(onError, nameof(onError));
            return _isSuccess ? this : onError(_error);
        }

        public Result<TValue, TError> OnError(Action<TError> onError)
        {
            Args.ExceptionIfNull(onError, nameof(onError));
            if (!_isSuccess)
            {
                onError(_error);
            }
            return this;
        }

        public Result<TValue, TNewError> OnError<TNewError>(Func<TError, TNewError> onError)
        {
            Args.ExceptionIfNull(onError, nameof(onError));
            return _isSuccess ? new Result<TValue, TNewError>(_value) : onError(_error);
        }       

        public Result<TValue, TError> Either(Action<TValue> onSuccess, Action<TError> onError)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess), onError, nameof(onError));
            if (_isSuccess)
            {
                onSuccess(_value);
            }
            else
            {
                onError(_error);
            }
            return this;
        }

        public Result<TNewValue, TError> Either<TNewValue>(Func<TValue, TNewValue> onSuccess, Action<TError> onError)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess), onError, nameof(onError));
            if (_isSuccess)
            {
                return onSuccess(_value);
            }
            onError(_error);
            return _error;
        }

        public Result<TValue, TNewError> Either<TNewError>(Action<TValue> onSuccess, Func<TError, TNewError> onError)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess), onError, nameof(onError));
            if (_isSuccess)
            {
                onSuccess(_value);
                return _value;
            }
            return onError(_error);
        }

        public Result<TNewValue, TNewError> Either<TNewValue, TNewError>(Func<TValue, TNewValue> onSuccess, Func<TError, TNewError> onError)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess), onError, nameof(onError));
            return _isSuccess ? new Result<TNewValue, TNewError>(onSuccess(_value)) : onError(_error);
        }

        public T Match<T>(Func<TValue, T> onSuccess, Func<TError, T> onError)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess), onError, nameof(onError));
            return _isSuccess ? onSuccess(_value) : onError(_error);
        }
    }
}