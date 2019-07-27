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

        /// <summary>
        /// Creates a Success <see cref="Result{TValue, TError}"/> with the <typeparamref name="TValue"/>.
        /// </summary>
        /// <param name="value">The Value of the <see cref="Result{TValue}"/>.</param>
        /// <returns></returns>
        public static Result<TValue, TError> Success(TValue value)
            => new Result<TValue, TError>(value);

        /// <summary>
        /// Creates a Error <see cref="Result{TValue, TError}"/> with the <typeparamref name="TError"/>.
        /// </summary>
        /// <param name="error">The Error of the <see cref="Result{TValue, TError}"/>.</param>
        /// <returns></returns>
        public static Result<TValue, TError> Error(TError error)
            => new Result<TValue, TError>(error);

        /// <summary>
        /// Implicitly converts a <typeparamref name="TValue"/> to a <see cref="Success(TValue)"/>.
        /// </summary>
        /// <param name="value">The Value.</param>
        public static implicit operator Result<TValue, TError>(TValue value)
            => new Result<TValue, TError>(value);

        /// <summary>
        /// Implicitly converts a <typeparamref name="TError"/> to a <see cref="Error(TError)"/>.
        /// </summary>
        /// <param name="error">The Error.</param>
        public static implicit operator Result<TValue, TError>(TError error)
            => new Result<TValue, TError>(error);

        /// <summary>
        /// Implicitly converts a <see cref="Result{TValue, TError}"/> to a <see cref="Result"/>.
        /// </summary>
        /// <param name="result">The <see cref="Result{TValue, TError}"/>.</param>
        public static implicit operator Result(Result<TValue, TError> result)
            => result._isSuccess;

        /// <summary>
        /// Implicitly converts a <see cref="Result{TValue}"/> to a <see cref="Result{TValue}"/>.
        /// </summary>
        /// <param name="result">The <see cref="Result{TValue, TError}"/>.</param>
        public static implicit operator Result<TValue>(Result<TValue, TError> result)
            => result._isSuccess ? result._value : Result<TValue>.Error();

        /// <summary>
        /// Implicitly converts a <see cref="Result{TValue}"/> to a <see cref="ResultOrError{TError}"/>.
        /// </summary>
        /// <param name="result">The <see cref="Result{TValue, TError}"/></param>
        public static implicit operator ResultOrError<TError>(Result<TValue, TError> result)
            => result._isSuccess ? ResultOrError<TError>.Success() : result._error;

        /// <summary>
        /// Converts This <see cref="Result{TValue, TError}"/> to a <see cref="Result"/>.
        /// </summary>
        /// <returns></returns>
        public Result AsPlainResult() => this;

        /// <summary>
        /// Converts This <see cref="Result{TValue, TError}"/> to a <see cref="Result{TValue}"/>.
        /// </summary>
        /// <returns></returns>
        public Result<TValue> AsResultOfValue() => this;

        /// <summary>
        /// Converts This <see cref="Result{TValue, TError}"/> to a <see cref="ResultOrError{TError}"/>.
        /// </summary>
        /// <returns></returns>
        public ResultOrError<TError> AsResultOrError() => this;

        /// <summary>
        /// Success: Calls <paramref name="onSuccess"/> and returns the <see cref="ResultOrError{TError}"/> from <paramref name="onSuccess"/>.
        /// <para></para>
        /// Error: Returns a <see cref="ResultOrError{TError}.Error(TError)"/>
        /// </summary>
        /// <param name="onSuccess">Is called and the <see cref="ResultOrError{TError}"/> is returned it This is a <see cref="Success(TValue)"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public ResultOrError<TError> Bind(Func<TValue, ResultOrError<TError>> onSuccess)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess));
            return _isSuccess ? onSuccess(_value) : _error;
        }

        /// <summary>
        /// Success: Calls <paramref name="onSuccess"/> and returns the <see cref="Result{TValue, TError}"/> of <typeparamref name="TNewValue"/>/<typeparamref name="TError"/> from <paramref name="onSuccess"/>.
        /// <para></para>
        /// Error: Returns a <see cref="Result{TValue, TError}.Error"/> of <typeparamref name="TNewValue"/>/<typeparamref name="TError"/>.
        /// </summary>
        /// <typeparam name="TNewValue">The new Type of Value.</typeparam>
        /// <param name="onSuccess">Is called and the <see cref="Result{TValue}"/> of <typeparamref name="TNewValue"/> is returned if This is a <see cref="Success(TValue)"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Result<TNewValue, TError> Bind<TNewValue>(Func<TValue, Result<TNewValue, TError>> onSuccess)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess));
            return _isSuccess ? onSuccess(_value) : _error;
        }

        /// <summary>
        /// Success: Calls <paramref name="onSuccess"/> and returns the <typeparamref name="TResult"/> from <paramref name="onSuccess"/>.
        /// <para></para>
        /// Error: Calls <paramref name="onError"/> and returns the <typeparamref name="TResult"/> from <paramref name="onError"/>.
        /// </summary>
        /// <typeparam name="TResult">The type of <see cref="IResult"/> that <paramref name="onSuccess"/> and <paramref name="onError"/> are returning.</typeparam>
        /// <param name="onSuccess">Is called and the <typeparamref name="TResult"/> is returned if This is a <see cref="Success(TValue)"/>.</param>
        /// <param name="onError">Is called and the <typeparamref name="TResult"/> is returned if This is a <see cref="Error(TError)"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public TResult Bind<TResult>(Func<TValue, TResult> onSuccess, Func<TError, TResult> onError)
           where TResult : IResult
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess), onError, nameof(onError));
            return _isSuccess ? onSuccess(_value) : onError(_error);
        }

        /// <summary>
        /// Success: Returns a <see cref="Result{TValue}.Success(TValue)"/>.
        /// <para></para>
        /// Error: Calls <paramref name="onError"/> and returns the <see cref="Result{TValue}"/> from <paramref name="onError"/>.
        /// </summary>
        /// <param name="onError">Is called and the <see cref="Result{TValue}"/> is returned if This is a <see cref="Error(TError)"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Result<TValue> BindOnError(Func<TError, Result<TValue>> onError)
        {
            Args.ExceptionIfNull(onError, nameof(onError));
            return _isSuccess ? _value : onError(_error);
        }

        /// <summary>
        /// Success: Returns a <see cref="Result{TValue, TError}.Success(TValue)"/> of <typeparamref name="TValue"/>/<typeparamref name="TNewError"/>.
        /// <para></para>
        /// Error: Calls <paramref name="onError"/> and returns the <see cref="Result{TValue, TError}.Error(TError)"/> of <typeparamref name="TValue"/>/<typeparamref name="TNewError"/> from <paramref name="onError"/>.
        /// </summary>
        /// <typeparam name="TNewError">The new type of Error.</typeparam>
        /// <param name="onError">Is called and the <see cref="Result{TValue, TError}"/> of <typeparamref name="TValue"/>/<typeparamref name="TNewError"/> is returned if This is a <see cref="Error(TError)"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Result<TValue, TNewError> BindOnError<TNewError>(Func<TError, Result<TValue, TNewError>> onError)
        {
            Args.ExceptionIfNull(onError, nameof(onError));
            return _isSuccess ? _value : onError(_error);
        }

        /// <summary>
        /// Success: Calls <paramref name="condition"/> and returns a <see cref="Success(TValue)"/> if the <paramref name="condition"/> is true or a <see cref="Error(TError)"/> with the <typeparamref name="TError"/> from <paramref name="error"/> if the <paramref name="condition"/> is false.
        /// <para></para>
        /// Error: Returns a <see cref="Error(TError)"/>.
        /// </summary>
        /// <param name="condition">Is called if This is a <see cref="Success(TValue)"/> and turned into a <see cref="Result{TValue, TError}.Error(TError)"/> with the <typeparamref name="TError"/> from <paramref name="error"/> if the <paramref name="condition"/> is false.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>

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

        /// <summary>
        /// Success: Stores the <typeparamref name="TValue"/> in <paramref name="keptOnSuccess"/>.
        /// <para></para>
        /// Error: Stores the default value of <typeparamref name="TValue"/> in <paramref name="keptOnSuccess"/>.
        /// </summary>
        /// <param name="keptOnSuccess">Stores the <typeparamref name="TValue"/> of This if This is a <see cref="Success(TValue)"/>..</param>
        /// <returns></returns>
        public Result<TValue, TError> KeepOnSuccess(out TValue keptOnSuccess)
        {
            keptOnSuccess = _isSuccess ? _value : default;
            return this;
        }

        /// <summary>
        /// Success: Calls <paramref name="onSuccess"/> and stores the <typeparamref name="T"/> in <paramref name="kept"/> for later use.
        /// <para></para>
        /// Error: Stores the default value of <typeparamref name="T"/> in <paramref name="kept"/>.
        /// </summary>
        /// <typeparam name="T">The type of the kept variable.</typeparam>
        /// <param name="onSuccess">Is called and the <typeparamref name="T"/> is stored in <paramref name="kept"/> if This is a <see cref="Success(TValue)"/>.</param>
        /// <param name="kept">Stores the <typeparamref name="T"/> from <paramref name="onSuccess"/> if This is a <see cref="Success(TValue)"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Result<TValue, TError> KeepOnSuccess<T>(Func<TValue, T> onSuccess, out T kept)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess));
            kept = _isSuccess ? onSuccess(_value) : default;
            return this;
        }

        /// <summary>
        /// Success: Stores the default value of <typeparamref name="TError"/> in <paramref name="kept"/>.
        /// <para></para>
        /// Error:  Stores the value of <typeparamref name="TError"/> in <paramref name="kept"/>.
        ///</summary>
        /// <param name="kept">Stores the <typeparamref name="TError"/> if This is a <see cref="Error"/>.</param>
        /// <returns></returns>
        public Result<TValue, TError> KeepOnError(out TError kept)
        {
            kept = _isSuccess ? default : _error;
            return this;
        }

        /// <summary>
        /// Success: Stores the default value of <typeparamref name="T"/> in <paramref name="kept"/>.
        /// <para></para>
        /// Error: Calls <paramref name="onError"/> and stores the <typeparamref name="T"/> in <paramref name="kept"/> for later use.
        /// </summary>
        /// <typeparam name="T">The type of the kept variable.</typeparam>
        /// <param name="onError">Is called and the <typeparamref name="T"/> is stored in <paramref name="kept"/> if this is a <see cref="Error"/>.</param>
        /// <param name="kept">Stores the <typeparamref name="T"/> from <paramref name="onError"/> if This is a <see cref="Error"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Result<TValue, TError> KeepOnError<T>(Func<TError, T> onError, out T kept)
        {
            Args.ExceptionIfNull(onError, nameof(onError));
            kept = _isSuccess ? default : onError(_error);
            return this;
        }

        /// <summary>
        /// Success: Stores the value of <typeparamref name="TValue"/> in <paramref name="keptOnSuccess"/> and the default value of <typeparamref name="TError"/> in <paramref name="keptOnError"/>.
        /// <para></para>
        /// Error: Stores the value of <typeparamref name="TError"/> in <paramref name="keptOnError"/> and the default value of <typeparamref name="TValue"/> in <paramref name="keptOnSuccess"/>.
        ///</summary>
        /// <param name="keptOnSuccess">Stores the <typeparamref name="TValue"/> if This is a <see cref="Success(TValue)"/>.</param>
        /// <param name="keptOnError">Stores the <typeparamref name="TError"/> if This is a <see cref="Error(TError)"/>.</param>
        /// <returns></returns>
        public Result<TValue, TError> KeepEither(out TValue keptOnSuccess, out TError keptOnError)
        {
            keptOnSuccess = _isSuccess ? _value : default;
            keptOnError = _isSuccess ? default : _error;
            return this;
        }

        /// <summary>
        /// Success: Calls <paramref name="onSuccess"/> and stores the <typeparamref name="T"/> in <paramref name="kept"/> for later use.
        /// <para></para>
        /// Error: Calls <paramref name="onError"/> and stores the <typeparamref name="T"/> in <paramref name="kept"/> for later use.
        /// </summary>
        /// <typeparam name="T">The type of the kept variable.</typeparam>
        /// <param name="onSuccess">Is called and the <typeparamref name="T"/> is stored in <paramref name="kept"/> if this is a <see cref="Success(TValue)"/>.</param>
        /// <param name="onError">Is called and the <typeparamref name="T"/> is stored in <paramref name="kept"/> if this is a <see cref="Error"/>.</param>
        /// <param name="kept">Stores the <typeparamref name="T"/> from <paramref name="onError"/> or <paramref name="onSuccess"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Result<TValue, TError> KeepEither<T>(Func<TValue, T> onSuccess, Func<TError, T> onError, out T kept)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess), onError, nameof(onError));
            kept = _isSuccess ? onSuccess(_value) : onError(_error);
            return this;
        }

        /// <summary>
        /// Success: Calls <paramref name="onSuccess"/> and stores the <typeparamref name="T1"/> in <paramref name="keptOnSuccess"/> for later use. Stores the default value of <typeparamref name="T2"/> in <paramref name="keptOnError"/>. 
        /// <para></para>
        /// Error: Calls <paramref name="onError"/> and stores the <typeparamref name="T2"/> in <paramref name="keptOnError"/> for later use. Stores the default value of <typeparamref name="T1"/> in <paramref name="keptOnSuccess"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the kept variable in case of <see cref="Success"/>.</typeparam>
        /// <typeparam name="T2">The type of the kept variable in case of <see cref="Error"/>.</typeparam>
        /// <param name="onSuccess">Is called and the <typeparamref name="T1"/> is stored in <paramref name="keptOnSuccess"/> if this is a <see cref="Success(TValue)"/>.</param>
        /// <param name="onError">Is called and the <typeparamref name="T2"/> is stored in <paramref name="keptOnError"/> if this is a <see cref="Error"/>.</param>
        /// <param name="keptOnSuccess">Stores the <typeparamref name="T1"/> from <paramref name="onSuccess"/> if This is a <see cref="Success(TValue)"/>.</param>
        /// <param name="keptOnError">Stores the <typeparamref name="T2"/> from <paramref name="onError"/> if This is a <see cref="Error"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Result<TValue, TError> KeepEither<T1, T2>(Func<TValue, T1> onSuccess, Func<TError, T2> onError, out T1 keptOnSuccess, out T2 keptOnError)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess), onError, nameof(onError));
            keptOnSuccess = _isSuccess ? onSuccess(_value) : default;
            keptOnError = _isSuccess ? default : onError(_error);
            return this;
        }

        /// <summary>
        /// Success: Calls <paramref name="onSuccess"/> and returns a <see cref="Success(TValue)"/>.
        /// <para></para>
        /// Error: Returns a <see cref="Error(TError)"/>.
        /// </summary>
        /// <param name="onSuccess">Is called if This is a <see cref="Success(TValue)"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Result<TValue, TError> OnSuccess(Action<TValue> onSuccess)
        {
            Args.ExceptionIfNull( onSuccess, nameof(onSuccess));
            if (_isSuccess)
            {
                onSuccess(_value);
            }
            return this;
        }

        /// <summary>
        /// Success: Calls <paramref name="onSuccess"/> and returns a <see cref="Result{TValue, TError}.Success(TValue)"/> of <typeparamref name="TNewValue"/>/<typeparamref name="TError"/> with the <typeparamref name="TNewValue"/> from <paramref name="onSuccess"/>.
        /// <para></para>
        /// Error: Returns a <see cref="Result{TValue, TError}.Error"/> of <typeparamref name="TNewValue"/>/<typeparamref name="TError"/>.
        /// </summary>
        /// <typeparam name="TNewValue">The new type of Value.</typeparam>
        /// <param name="onSuccess">Is called and the <typeparamref name="TNewValue"/> is returned as <see cref="Result{TValue}.Success(TValue)"/> if This is a <see cref="Success(TValue)"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Result<TNewValue, TError> OnSuccess<TNewValue>(Func<TValue, TNewValue> onSuccess)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess));
            return _isSuccess ? new Result<TNewValue, TError>(onSuccess(_value)) : _error;
        }

        /// <summary>
        /// Success: Returns a <see cref="Success(TValue)"/>.
        /// <para></para>
        /// Error: Calls <paramref name="onError"/> and returns a <see cref="Success(TValue)"/>.
        /// </summary>
        /// <param name="onError">Is called if This is a <see cref="Error(TError)"/> and is supposed to fix the <see cref="Error"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Result<TValue, TError> FixOnError(Func<TError, TValue> onError)
        {
            Args.ExceptionIfNull(onError, nameof(onError));
            return _isSuccess ? this : onError(_error);
        }

        /// <summary>
        /// Success: Returns a <see cref="Success(TValue)"/>.
        /// <para></para>
        /// Error: Calls <paramref name="onError"/> and returns a <see cref="Error"/>.
        /// </summary>
        /// <param name="onError">Is called if This is a <see cref="Error"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Result<TValue, TError> OnError(Action<TError> onError)
        {
            Args.ExceptionIfNull(onError, nameof(onError));
            if (!_isSuccess)
            {
                onError(_error);
            }
            return this;
        }

        /// <summary>
        /// Success: Returns a <see cref="Result{TValue, TError}.Success(TValue)"/> of <typeparamref name="TValue"/>/<typeparamref name="TNewError"/>.
        /// <para></para>
        /// Error: Calls <paramref name="onError"/> and returns a <see cref="Result{TValue, TError}.Error(TError)"/> of <typeparamref name="TValue"/>/<typeparamref name="TNewError"/> with the <typeparamref name="TNewError"/> from <paramref name="onError"/>.
        /// </summary>
        /// <typeparam name="TNewError">The type of the Error.</typeparam>
        /// <param name="onError">Is called and returned as <see cref="Result{TValue, TError}.Error(TError)"/> if This is a <see cref="Error"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Result<TValue, TNewError> OnError<TNewError>(Func<TError, TNewError> onError)
        {
            Args.ExceptionIfNull(onError, nameof(onError));
            return _isSuccess ? new Result<TValue, TNewError>(_value) : onError(_error);
        }

        /// <summary>
        /// Success: Calls <paramref name="onSuccess"/> and returns a <see cref="Success(TValue)"/>.
        /// <para></para>
        /// Error: Calls <paramref name="onError"/> and returns a <see cref="Error(TError)"/>.
        /// </summary>
        /// <param name="onSuccess">Is called if This is a <see cref="Success(TValue)"/>.</param>
        /// <param name="onError">Is called if This is a <see cref="Success(TValue)"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
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

        /// <summary>
        /// Success: Calls <paramref name="onSuccess"/> and returns a <see cref="Success(TValue)"/> with the <typeparamref name="TNewValue"/> from <paramref name="onSuccess"/>. 
        /// <para></para>
        /// Error: Calls <paramref name="onError"/> and returns a <see cref="Error(TError)"/> of <typeparamref name="TNewValue"/>.
        /// </summary>
        /// <typeparam name="TNewValue">The new type of the Value.</typeparam>
        /// <param name="onSuccess">Is called and returned as <see cref="Error(TError)"/> if This is a <see cref="Success"/>.</param>
        /// <param name="onError">Is called if This is a <see cref="Error(TError)"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
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

        /// <summary>
        /// Success: Calls <paramref name="onSuccess"/> and returns a <see cref="Result{TValue,TError}.Success"/> of <typeparamref name="TValue"/>/<typeparamref name="TNewError"/>.
        /// <para></para>
        /// Error: Calls <paramref name="onError"/> and returns a <see cref="Result{TValue,TError}.Error(TError)"/> of <typeparamref name="TValue"/>/<typeparamref name="TNewError"/> with the <typeparamref name="TNewError"/> from <paramref name="onError"/>.
        /// </summary>
        /// <typeparam name="TNewError">The type of the Error.</typeparam>
        /// <param name="onSuccess">Is called if This is a <see cref="Success(TValue)"/>.</param>
        /// <param name="onError">Is called and returned as <see cref="Result{TValue,TError}.Error(TError)"/> of <typeparamref name="TValue"/>/<typeparamref name="TNewError"/> if This is a <see cref="Error"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
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

        /// <summary>
        /// Success: Calls <paramref name="onSuccess"/> and returns a <see cref="Result{TValue, TError}.Success(TValue)"/> of <typeparamref name="TNewValue"/>/<typeparamref name="TNewError"/> with the <typeparamref name="TNewValue"/> from <paramref name="onSuccess"/>.
        /// <para></para>
        /// Error: Calls <paramref name="onError"/> and returns a <see cref="Result{TValue, TError}.Error(TError)"/> of <typeparamref name="TNewValue"/>/<typeparamref name="TNewValue"/> with the <typeparamref name="TNewError"/> from <paramref name="onError"/>.
        /// </summary>
        /// <typeparam name="TNewValue">The new type of the Value.</typeparam>
        /// <typeparam name="TNewError">The type of the Error.</typeparam>
        /// <param name="onSuccess">Is called and returned as <see cref="Result{TValue, TError}.Success(TValue)"/> of <typeparamref name="TNewValue"/>/<typeparamref name="TNewError"/> if This is a <see cref="Success(TValue)"/>.</param>
        /// <param name="onError">Is called and returned as <see cref="Result{TValue, TError}.Error(TError)"/> of <typeparamref name="TNewValue"/>/<typeparamref name="TNewError"/> if This is a <see cref="Error(TError)"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Result<TNewValue, TNewError> Either<TNewValue, TNewError>(Func<TValue, TNewValue> onSuccess, Func<TError, TNewError> onError)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess), onError, nameof(onError));
            return _isSuccess ? new Result<TNewValue, TNewError>(onSuccess(_value)) : onError(_error);
        }

        /// <summary>
        /// Success: Calls <paramref name="onSuccess"/> and returns the <typeparamref name="T"/>.
        /// <para></para>
        /// Error: Calls <paramref name="onError"/> and returns the <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The return-type of the Method.</typeparam>
        /// <param name="onSuccess">Is called and the <typeparamref name="T"/> is returned if This is a <see cref="Success(TValue)"/>.</param>
        /// <param name="onError">Is called and the <typeparamref name="T"/> is returned if This is a <see cref="Error"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public T Match<T>(Func<TValue, T> onSuccess, Func<TError, T> onError)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess), onError, nameof(onError));
            return _isSuccess ? onSuccess(_value) : onError(_error);
        }
    }
}