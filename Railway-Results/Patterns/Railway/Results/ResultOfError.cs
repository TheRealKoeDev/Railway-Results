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
    /// A Result/Monad with Error, that can either be a <see cref="Success"/> or a <see cref="Error(TError)"/>.
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

        /// <summary>
        /// Implicitly converts a <see cref="ResultOrError{TError}"/> to a <see cref="Result"/>.
        /// </summary>
        /// <param name="result"></param>
        public static implicit operator Result(ResultOrError<TError> result)
            => result._isSuccess;

        /// <summary>
        /// Converts this <see cref="ResultOrError{TError}"/> to a <see cref="Result"/>.
        /// </summary>
        /// <returns></returns>
        public Result AsPlainResult() 
            => this;

        /// <summary>
        /// Case <see cref="Success"/>: Calls <paramref name="onSuccess"/> and returns the <see cref="ResultOrError{TError}"/> from <paramref name="onSuccess"/>.
        /// <para></para>
        /// Case <see cref="Error(TError)"/>: Returns a <see cref="Error(TError)"/>.
        /// </summary>
        /// <param name="onSuccess">Is called and the <see cref="ResultOrError{TError}"/> is returned if This is a <see cref="Success"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public ResultOrError<TError> Bind(Func<ResultOrError<TError>> onSuccess)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess));
            return _isSuccess ? onSuccess() : this;
        }

        /// <summary>
        /// Case <see cref="Success"/>: Calls <paramref name="onSuccess"/> and returns the <see cref="Result{TValue, TError}"/> from <paramref name="onSuccess"/>.
        /// <para></para>
        /// Case <see cref="Error(TError)"/>: Returns a <see cref="Result{TValue, TError}.Error(TError)"/>
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="onSuccess">Is called and the <see cref="Result{TValue, TError}"/> is returned it This is a <see cref="Success"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Result<TValue, TError> Bind<TValue>(Func<Result<TValue, TError>> onSuccess)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess));
            return _isSuccess ? onSuccess() : _error;
        }

        /// <summary>
        /// Case <see cref="Success"/>: Calls <paramref name="onSuccess"/> and returns the <typeparamref name="TResult"/> from <paramref name="onSuccess"/>.
        /// <para></para>
        /// Case <see cref="Error(TError)"/>: Calls <paramref name="onError"/> and returns the <typeparamref name="TResult"/> from <paramref name="onError"/>.
        /// </summary>
        /// <typeparam name="TResult">The type of <see cref="IResult"/> that <paramref name="onSuccess"/> and <paramref name="onError"/> are returning.</typeparam>
        /// <param name="onSuccess">Is called and the <typeparamref name="TResult"/> is returned if This is a <see cref="Success"/>.</param>
        /// <param name="onError">Is called and the <typeparamref name="TResult"/> is returned if This is a <see cref="Error(TError)"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public TResult Bind<TResult>(Func<TResult> onSuccess, Func<TError, TResult> onError)
            where TResult : IResult
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess), onError, nameof(onError));
            return _isSuccess ? onSuccess() : onError(_error);
        }

        /// <summary>
        /// Case <see cref="Success"/>: Returns a <see cref="ResultOrError{TNewError}.Success"/>.
        /// <para></para>
        /// Case <see cref="Error"/>: Calls <paramref name="onError"/> and returns the <see cref="ResultOrError{TNewError}"/> of <typeparamref name="TNewError"/> from <paramref name="onError"/>.
        /// </summary>
        /// <param name="onError">Is called and a <see cref="ResultOrError{TError}"/> of <typeparamref name="TNewError"/> is returned if This is a <see cref="Error(TError)"/>.</param>
        /// <typeparam name="TNewError">The new type of the Error.</typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public ResultOrError<TNewError> BindOnError<TNewError>(Func<TError, ResultOrError<TNewError>> onError)
        {
            Args.ExceptionIfNull(onError, nameof(onError));
            return _isSuccess ? ResultOrError<TNewError>.Success() : onError(_error);
        }

        /// <summary>
        /// Case <see cref="Success"/>: Returns a <see cref="Result.Success"/>.
        /// <para></para>
        /// Case <see cref="Error"/>: Calls <paramref name="onError"/> and returns the <see cref="Result"/> from <paramref name="onError"/>.
        /// </summary>
        /// <param name="onError">Is called and the <see cref="Result"/> is returned if This is a <see cref="Error(TError)"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Result BindOnError(Func<TError, Result> onError)
        {
            Args.ExceptionIfNull(onError, nameof(onError));
            return _isSuccess ? Result.Success() : onError(_error);
        }

        /// <summary>
        /// Case <see cref="Success"/>: Calls <paramref name="onSuccess"/> and returns a <see cref="Success"/>.
        /// <para></para>
        /// Case <see cref="Error(TError)"/>: Returns a <see cref="Error(TError)"/>.
        /// </summary>
        /// <param name="onSuccess">Is called if This is a <see cref="Success"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public ResultOrError<TError> OnSuccess(Action onSuccess)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess));
            if (_isSuccess)
            {
                onSuccess();
            }
            return this;
        }

        /// <summary>
        /// Case <see cref="Success"/>: Calls <paramref name="onSuccess"/> and returns a <see cref="Result{TValue, TError}.Success(TValue)"/> with the <typeparamref name="TValue"/> from <paramref name="onSuccess"/>.
        /// <para></para>
        /// Case <see cref="Error(TError)"/>: Returns a <see cref="Result{TValue, TError}.Error(TError)"/>.
        /// </summary>
        /// <typeparam name="TValue">The type of Value.</typeparam>
        /// <param name="onSuccess">Is called and the <typeparamref name="TValue"/> is returned as <see cref="Result{TValue, TError}.Success(TValue)"/> if This is a <see cref="Success"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Result<TValue, TError> OnSuccess<TValue>(Func<TValue> onSuccess)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess));
            return _isSuccess ? onSuccess() : Result<TValue, TError>.Error(_error);
        }

        /// <summary>
        /// Case <see cref="Success"/>: Returns a <see cref="Success"/>.
        /// <para></para>
        /// Case <see cref="Error(TError)"/>: Calls <paramref name="onError"/> and returns a <see cref="Success"/>.
        /// </summary>
        /// <param name="onError">Is called if This is a <see cref="Error"/> and is supposed to fix the <see cref="Error(TError)"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public ResultOrError<TError> FixOnError(Action<TError> onError)
        {
            Args.ExceptionIfNull(onError, nameof(onError));
            if (!_isSuccess)
            {
                onError(_error);
            }
            return Success();
        }

        /// <summary>
        /// Case <see cref="Success"/>: Returns a <see cref="Success"/>.
        /// <para></para>
        /// Case <see cref="Error(TError)"/>: Calls <paramref name="onError"/> and returns a <see cref="Error(TError)"/>.
        /// </summary>
        /// <param name="onError">Is called if This is a <see cref="Error(TError)"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public ResultOrError<TError> OnError(Action<TError> onError)
        {
            Args.ExceptionIfNull(onError, nameof(onError));
            if (!_isSuccess)
            {
                onError(_error);
            }
            return this;
        }

        /// <summary>
        /// Case <see cref="Success"/>: Returns a <see cref="ResultOrError{TError}"/> of <typeparamref name="TNewError"/>.
        /// <para></para>
        /// Case <see cref="Error(TError)"/>: Calls <paramref name="onError"/> and returns a <see cref="ResultOrError{TNewError}.Error(TNewError)"/> with <typeparamref name="TNewError"/> from <paramref name="onError"/>.
        /// </summary>
        /// <typeparam name="TNewError">The new type of the <typeparamref name="TError"/>.</typeparam>
        /// <param name="onError">Is called and returned as <see cref="ResultOrError{TError}.Error(TError)"/> of <typeparamref name="TNewError"/> if This is a <see cref="Error(TError)"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public ResultOrError<TNewError> OnError<TNewError>(Func<TError, TNewError> onError)
        {
            Args.ExceptionIfNull(onError, nameof(onError));
            return _isSuccess ? ResultOrError<TNewError>.Success() : onError(_error);
        }

        /// <summary>
        /// Case <see cref="Success"/>: Calls <paramref name="onSuccess"/> and returns a <see cref="Success"/>.
        /// <para></para>
        /// Case <see cref="Error(TError)"/>: Calls <paramref name="onError"/> and returns a <see cref="Error(TError)"/>.
        /// </summary>
        /// <param name="onSuccess">Is called if This is a <see cref="Success"/>.</param>
        /// <param name="onError">Is called if This is a <see cref="Error(TError)"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
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

        /// <summary>
        /// Case <see cref="Success"/>: Calls <paramref name="onSuccess"/> and returns a <see cref="Result{TValue, TError}.Success(TValue)"/> with the <typeparamref name="TValue"/> from <paramref name="onSuccess"/>. 
        /// <para></para>
        /// Case <see cref="Error(TError)"/>: Calls <paramref name="onError"/> and returns a <see cref="Result{TValue, TError}.Error(TError)"/>.
        /// </summary>
        /// <typeparam name="TValue">The type of the Value.</typeparam>
        /// <param name="onSuccess">Is called and returned as <see cref="Result{TValue, TError}.Success(TValue)"/> if This is a <see cref="Success"/>.</param>
        /// <param name="onError">Is called if This is a <see cref="Error"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
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

        /// <summary>
        /// Case <see cref="Success"/>: Calls <paramref name="onSuccess"/> and returns a <see cref="ResultOrError{TError}.Success"/> of <typeparamref name="TNewError"/>.
        /// <para></para>
        /// Case <see cref="Error(TError)"/>: Calls <paramref name="onError"/> and returns a <see cref="ResultOrError{TError}.Error(TError)"/> with the <typeparamref name="TNewError"/> from <paramref name="onError"/>.
        /// </summary>
        /// <typeparam name="TNewError">The new type of the Error.</typeparam>
        /// <param name="onSuccess">Is called if This is a <see cref="Success"/>.</param>
        /// <param name="onError">Is called and returned as <see cref="ResultOrError{TError}.Error(TError)"/> of <typeparamref name="TNewError"/> if This is a <see cref="Error(TError)"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
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

        /// <summary>
        /// Case <see cref="Success"/>: Calls <paramref name="onSuccess"/> and returns a <see cref="Result{TValue, TError}.Success(TValue)"/> with the <typeparamref name="TValue"/> from <paramref name="onSuccess"/>.
        /// <para></para>
        /// Case <see cref="Error(TError)"/>: Calls <paramref name="onError"/> and returns a <see cref="Result{TValue, TError}.Error(TError)"/> with the <typeparamref name="TNewError"/> from <paramref name="onError"/>.
        /// </summary>
        /// <typeparam name="TValue">The type of the Value.</typeparam>
        /// <typeparam name="TNewError">The new type of the Error.</typeparam>
        /// <param name="onSuccess">Is called and returned as <see cref="Result{TValue, TError}.Success(TValue)"/> if This is a <see cref="Success"/>.</param>
        /// <param name="onError">Is called and returned as <see cref="Result{TValue, TError}.Error(TError)"/> of <typeparamref name="TNewError"/> if This is a <see cref="Error"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Result<TValue, TNewError> Either<TValue, TNewError>(Func<TValue> onSuccess, Func<TError, TNewError> onError)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess), onError, nameof(onError));
            return _isSuccess ? Result<TValue, TNewError>.Success(onSuccess()) : onError(_error);
        }

        /// <summary>
        /// Case <see cref="Success"/>: Calls <paramref name="onSuccess"/> and returns the <typeparamref name="T"/>.
        /// <para></para>
        /// Case <see cref="Error(TError)"/>: Calls <paramref name="onError"/> and returns the <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The return-type of the Method.</typeparam>
        /// <param name="onSuccess">Is called and the <typeparamref name="T"/> is returned if This is a <see cref="Success"/>.</param>
        /// <param name="onError">Is called and the <typeparamref name="T"/> is returned if This is a <see cref="Error(TError)"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public T Match<T>(Func<T> onSuccess, Func<TError, T> onError)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess), onError, nameof(onError));
            return _isSuccess ? onSuccess() : onError(_error);
        }

        /// <summary>
        /// Case <see cref="Success"/>: Calls <paramref name="condition"/> and returns a <see cref="Success"/> if the <paramref name="condition"/> is true or a <see cref="Error(TError)"/> with the <typeparamref name="TError"/> from <paramref name="onError"/> if the <paramref name="condition"/> is false.
        /// <para></para>
        /// Case <see cref="Error(TError)"/>: Returns a <see cref="Error(TError)"/>.
        /// </summary>
        /// <param name="condition">Is called if This is a <see cref="Success"/> and turned into a <see cref="ResultOrError{TError}"/> with <paramref name="onError"/> if this is a <see cref="Error(TError)"/>.</param>
        /// <param name="onError">The function that provides the <typeparamref name="TError"/> is the <paramref name="condition"/> if false.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public ResultOrError<TError> Ensure(Func<bool> condition, Func<TError> onError)
        {
            Args.ExceptionIfNull(condition, nameof(condition), onError, nameof(onError));
            if (!_isSuccess)
            {
                return this;
            }
            else if (!condition())
            {
                return onError();
            }
            return this;
        }

        /// <summary>
        /// Case <see cref="Success"/>: Calls <paramref name="onSuccess"/> and stores the <typeparamref name="T"/> in <paramref name="kept"/> for later use.
        /// <para></para>
        /// Case <see cref="Error(TError)"/>: Stores the default value of <typeparamref name="T"/> in <paramref name="kept"/>.
        /// </summary>
        /// <typeparam name="T">The type of the kept variable.</typeparam>
        /// <param name="onSuccess">Is called and the <typeparamref name="T"/> is stored in <paramref name="kept"/> if This is a <see cref="Success"/>.</param>
        /// <param name="kept">Stores the <typeparamref name="T"/> from <paramref name="onSuccess"/> if This is a <see cref="Success"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public ResultOrError<TError> KeepOnSuccess<T>(Func<T> onSuccess, out T kept)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess));
            kept = _isSuccess ? onSuccess() : default;
            return this;
        }

        /// <summary>
        /// Case <see cref="Success"/>: Stores the default value of <typeparamref name="TError"/> in <paramref name="keptError"/>. 
        /// <para></para>
        /// Case <see cref="Error(TError)"/>:  Stores the <typeparamref name="TError"/> in <paramref name="keptError"/>.
        /// </summary>
        /// <param name="keptError">Stores the <typeparamref name="TError"/> of This or the default value of <typeparamref name="TError"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public ResultOrError<TError> KeepOnError(out TError keptError)
        {
            keptError = _isSuccess ? default : _error;
            return this;
        }

        /// <summary>
        /// Case <see cref="Success"/>: Stores the default value of <typeparamref name="T"/> in <paramref name="kept"/>.
        /// <para></para>
        /// Case <see cref="Error(TError)"/>: Calls <paramref name="onError"/> and stores the <typeparamref name="T"/> in <paramref name="kept"/> for later use.
        /// </summary>
        /// <typeparam name="T">The type of the kept variable.</typeparam>
        /// <param name="onError">Is called and the <typeparamref name="T"/> is stored in <paramref name="kept"/> if this is a <see cref="Error(TError)"/>.</param>
        /// <param name="kept">Stores the <typeparamref name="T"/> from <paramref name="onError"/> if This is a <see cref="Error(TError)"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public ResultOrError<TError> KeepOnError<T>(Func<TError, T> onError, out T kept)
        {
            Args.ExceptionIfNull(onError, nameof(onError));
            kept = _isSuccess ?  default : onError(_error);
            return this;
        }

        /// <summary>
        /// Case <see cref="Success"/>: Calls <paramref name="onSuccess"/> and stores the <typeparamref name="T"/> in <paramref name="kept"/> for later use.
        /// <para></para>
        /// Case <see cref="Error(TError)"/>: Calls <paramref name="onError"/> and stores the <typeparamref name="T"/> in <paramref name="kept"/> for later use.
        /// </summary>
        /// <typeparam name="T">The type of the kept variable.</typeparam>
        /// <param name="onSuccess">Is called and the <typeparamref name="T"/> is stored in <paramref name="kept"/> if this is a <see cref="Success"/>.</param>
        /// <param name="onError">Is called and the <typeparamref name="T"/> is stored in <paramref name="kept"/> if this is a <see cref="Error(TError)"/>.</param>
        /// <param name="kept">Stores the <typeparamref name="T"/> from <paramref name="onError"/> or <paramref name="onSuccess"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public ResultOrError<TError> KeepEither<T>(Func<T> onSuccess, Func<TError, T> onError, out T kept)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess), onError, nameof(onError));
            kept = _isSuccess ? onSuccess() : onError(_error);
            return this;
        }

        /// <summary>
        /// Case <see cref="Success"/>: Calls <paramref name="onSuccess"/> and stores the <typeparamref name="T1"/> in <paramref name="keptOnSuccess"/> for later use. Stores the default value of <typeparamref name="T2"/> in <paramref name="keptOnError"/>. 
        /// <para></para>
        /// Case <see cref="Error(TError)"/>: Calls <paramref name="onError"/> and stores the <typeparamref name="T2"/> in <paramref name="keptOnError"/> for later use. Stores the default value of <typeparamref name="T1"/> in <paramref name="keptOnSuccess"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the kept variable in case of <see cref="Success"/>.</typeparam>
        /// <typeparam name="T2">The type of the kept variable in case of <see cref="Error(TError)"/>.</typeparam>
        /// <param name="onSuccess">Is called and the <typeparamref name="T1"/> is stored in <paramref name="keptOnSuccess"/> if this is a <see cref="Success"/>.</param>
        /// <param name="onError">Is called and the <typeparamref name="T2"/> is stored in <paramref name="keptOnError"/> if this is a <see cref="Error(TError)"/>.</param>
        /// <param name="keptOnSuccess">Stores the <typeparamref name="T1"/> from <paramref name="onSuccess"/> if This is a <see cref="Success"/>.</param>
        /// <param name="keptOnError">Stores the <typeparamref name="T2"/> from <paramref name="onError"/> if This is a <see cref="Error(TError)"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public ResultOrError<TError> KeepEither<T1, T2>(Func<T1> onSuccess, Func<TError, T2> onError, out T1 keptOnSuccess, out T2 keptOnError)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess), onError, nameof(onError));
            keptOnSuccess = _isSuccess ? onSuccess() : default;
            keptOnError = _isSuccess ? default : onError(_error);
            return this;
        }  
    }
}
