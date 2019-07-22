using KoeLib.Patterns.Railway.Tools;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace KoeLib.Patterns.Railway.Results
{
    /// <summary>
    /// A Result/Monad with Value, that can either be a <see cref="Success(TValue)"/> or a <see cref="Error"/>.
    /// <para>The default value of <see cref="Result{TValue}"/> is a <see cref="Error"/>.</para>
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <seealso cref="IResult" />
    [DebuggerStepThrough]
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct Result<TValue> : IResult
    {
        /// <summary>
        /// Is True this is a <see cref="Success"/> or False if this is a <see cref="Error"/>.
        /// </summary>
        private readonly bool _isSuccess;

        /// <summary>
        /// The Value.
        /// </summary>
        private readonly TValue _value;

        /// <summary>
        /// Creates a Success <see cref="Result{TValue}"/>.
        /// </summary>
        /// <param name="value">The Value of the <see cref="Result{TValue}"/>.</param>
        private Result(TValue value)
        {
            _isSuccess = true;
            _value = value;
        }

        /// <summary>
        /// Creates a Success <see cref="Result{TValue}"/> with the <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The Value of the <see cref="Result{TValue}"/>.</param>
        /// <returns></returns>
        public static Result<TValue> Success(TValue value)
            => new Result<TValue>(value);

        /// <summary>
        /// Creates a Error <see cref="Result{TValue}"/>.
        /// </summary>
        /// <returns></returns>
        public static Result<TValue> Error()
            => new Result<TValue>();

        /// <summary>
        /// Implicitly converts a <typeparamref name="TValue"/> to a <see cref="Success(TValue)"/>.
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator Result<TValue>(TValue value)
            => new Result<TValue>(value);

        /// <summary>
        /// Implicitly converts a <see cref="Result{TValue}"/> to a <see cref="Result"/>.
        /// </summary>
        /// <param name="result">The <see cref="Result{TValue}"/></param>
        public static implicit operator Result(Result<TValue> result)
            => result._isSuccess;

        /// <summary>
        /// Converts a <see cref="Result{TValue}"/> to a <see cref="Result"/>.
        /// </summary>
        /// <returns></returns>
        public Result AsPlainResult() => this;

        /// <summary>
        /// Case <see cref="Success(TValue)"/>: Calls <paramref name="onSuccess"/> and returns the <see cref="Result"/> from <paramref name="onSuccess"/>.
        /// <para></para>
        /// Case <see cref="Error"/>: Returns a <see cref="Result.Error"/>
        /// </summary>
        /// <param name="onSuccess">Is called and the <see cref="Result"/> is returned it This is a <see cref="Success(TValue)"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Result Bind(Func<TValue, Result> onSuccess)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess));
            return _isSuccess ? onSuccess(_value) : Result.Error();
        }

        /// <summary>
        /// Case <see cref="Success(TValue)"/>: Calls <paramref name="onSuccess"/> and returns the <see cref="Result{TValue}"/> of <typeparamref name="TNewValue"/> from <paramref name="onSuccess"/>.
        /// <para></para>
        /// Case <see cref="Error"/>: Returns a <see cref="Result{TValue}.Error"/> of <typeparamref name="TNewValue"/>.
        /// </summary>
        /// <typeparam name="TNewValue">The new Type of Value.</typeparam>
        /// <param name="onSuccess">Is called and the <see cref="Result{TValue}"/> of <typeparamref name="TNewValue"/> is returned if This is a <see cref="Success(TValue)"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Result<TNewValue> Bind<TNewValue>(Func<TValue, Result<TNewValue>> onSuccess)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess));
            return _isSuccess ? onSuccess(_value) : new Result<TNewValue>();
        }

        /// <summary>
        /// Case <see cref="Success(TValue)"/>: Calls <paramref name="onSuccess"/> and returns the <typeparamref name="TResult"/> from <paramref name="onSuccess"/>.
        /// <para></para>
        /// Case <see cref="Error"/>: Calls <paramref name="onError"/> and returns the <typeparamref name="TResult"/> from <paramref name="onError"/>.
        /// </summary>
        /// <typeparam name="TResult">The type of <see cref="IResult"/> that <paramref name="onSuccess"/> and <paramref name="onError"/> are returning.</typeparam>
        /// <param name="onSuccess">Is called and the <typeparamref name="TResult"/> is returned if This is a <see cref="Success(TValue)"/>.</param>
        /// <param name="onError">Is called and the <typeparamref name="TResult"/> is returned if This is a <see cref="Error"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public TResult Bind<TResult>(Func<TValue, TResult> onSuccess, Func<TResult> onError)
            where TResult : IResult
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess), onError, nameof(onError));
            return _isSuccess ? onSuccess(_value) : onError();
        }

        /// <summary>
        /// Case <see cref="Success(TValue)"/>: Returns a <see cref="Success(TValue)"/>.
        /// <para></para>
        /// Case <see cref="Error"/>: Calls <paramref name="onError"/> and returns the <see cref="Result{TValue}"/> from <paramref name="onError"/>.
        /// </summary>
        /// <param name="onError">Is called and the <see cref="Result{TValue}"/> is returned if This is a <see cref="Error"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Result<TValue> BindOnError(Func<Result<TValue>> onError)
        {
            Args.ExceptionIfNull(onError, nameof(onError));
            return _isSuccess ? this : onError();
        }

        /// <summary>
        /// Case <see cref="Success(TValue)"/>: Returns a <see cref="Result{TValue, TError}.Success(TValue)"/>.
        /// <para></para>
        /// Case <see cref="Error"/>: Calls <paramref name="onError"/> and returns the <see cref="Result{TValue, TError}.Error(TError)"/> from <paramref name="onError"/>.
        /// </summary>
        /// <typeparam name="TError">The type of Error.</typeparam>
        /// <param name="onError">Is called and the <see cref="Result{TValue, TError}"/> is returned if This is a <see cref="Error"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Result<TValue, TError> BindOnError<TError>(Func<Result<TValue, TError>> onError)
        {
            Args.ExceptionIfNull(onError, nameof(onError));
            return _isSuccess ? Result<TValue, TError>.Success(_value) : onError();
        }

        /// <summary>
        /// Case <see cref="Success(TValue)"/>: Calls <paramref name="condition"/> and returns a <see cref="Success(TValue)"/> if the <paramref name="condition"/> is true or a <see cref="Error"/> if the <paramref name="condition"/> is false.
        /// <para></para>
        /// Case <see cref="Error"/>: Returns a <see cref="Error"/>.
        /// </summary>
        /// <param name="condition">Is called if This is a <see cref="Success(TValue)"/> and turned into a <see cref="Result{TValue}.Error"/> if the <paramref name="condition"/> is false.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Result<TValue> Ensure(Func<TValue, bool> condition)
        {
            Args.ExceptionIfNull(condition, nameof(condition));
            return _isSuccess && condition(_value) ? this : new Result<TValue>();
        }

        /// <summary>
        /// Case <see cref="Success(TValue)"/>: Stores the <typeparamref name="TValue"/> in <paramref name="keptOnSuccess"/>.
        /// <para></para>
        /// Case <see cref="Error"/>: Stores the default value of <typeparamref name="TValue"/> in <paramref name="keptOnSuccess"/>.
        /// </summary>
        /// <param name="keptOnSuccess">Stores the <typeparamref name="TValue"/> of This if This is a <see cref="Success(TValue)"/>..</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Result<TValue> KeepOnSuccess(out TValue keptOnSuccess)
        {
            keptOnSuccess = _isSuccess ? _value : default;
            return this;
        }

        /// <summary>
        /// Case <see cref="Success(TValue)"/>: Calls <paramref name="onSuccess"/> and stores the <typeparamref name="T"/> in <paramref name="kept"/> for later use.
        /// <para></para>
        /// Case <see cref="Error"/>: Stores the default value of <typeparamref name="T"/> in <paramref name="kept"/>.
        /// </summary>
        /// <typeparam name="T">The type of the kept variable.</typeparam>
        /// <param name="onSuccess">Is called and the <typeparamref name="T"/> is stored in <paramref name="kept"/> if This is a <see cref="Success(TValue)"/>.</param>
        /// <param name="kept">Stores the <typeparamref name="T"/> from <paramref name="onSuccess"/> if This is a <see cref="Success(TValue)"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Result<TValue> KeepOnSuccess<T>(Func<TValue, T> onSuccess, out T kept)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess));
            kept = _isSuccess ? onSuccess(_value) : default;
            return this;
        }

        /// <summary>
        /// Case <see cref="Success(TValue)"/>: Stores the default value of <typeparamref name="T"/> in <paramref name="kept"/>.
        /// <para></para>
        /// Case <see cref="Error"/>: Calls <paramref name="onError"/> and stores the <typeparamref name="T"/> in <paramref name="kept"/> for later use.
        /// </summary>
        /// <typeparam name="T">The type of the kept variable.</typeparam>
        /// <param name="onError">Is called and the <typeparamref name="T"/> is stored in <paramref name="kept"/> if this is a <see cref="Error"/>.</param>
        /// <param name="kept">Stores the <typeparamref name="T"/> from <paramref name="onError"/> if This is a <see cref="Error"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Result<TValue> KeepOnError<T>(Func<T> onError, out T kept)
        {
            Args.ExceptionIfNull(onError, nameof(onError));
            kept = _isSuccess ? default : onError();
            return this;
        }

        /// <summary>
        /// Case <see cref="Success(TValue)"/>: Calls <paramref name="onSuccess"/> and stores the <typeparamref name="T"/> in <paramref name="kept"/> for later use.
        /// <para></para>
        /// Case <see cref="Error"/>: Calls <paramref name="onError"/> and stores the <typeparamref name="T"/> in <paramref name="kept"/> for later use.
        /// </summary>
        /// <typeparam name="T">The type of the kept variable.</typeparam>
        /// <param name="onSuccess">Is called and the <typeparamref name="T"/> is stored in <paramref name="kept"/> if this is a <see cref="Success(TValue)"/>.</param>
        /// <param name="onError">Is called and the <typeparamref name="T"/> is stored in <paramref name="kept"/> if this is a <see cref="Error"/>.</param>
        /// <param name="kept">Stores the <typeparamref name="T"/> from <paramref name="onError"/> or <paramref name="onSuccess"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Result<TValue> KeepEither<T>(Func<TValue, T> onSuccess, Func<T> onError, out T kept)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess), onError, nameof(onError));
            kept = _isSuccess ? onSuccess(_value) : onError();
            return this;
        }

        /// <summary>
        /// Case <see cref="Success(TValue)"/>: Calls <paramref name="onSuccess"/> and stores the <typeparamref name="T1"/> in <paramref name="keptOnSuccess"/> for later use. Stores the default value of <typeparamref name="T2"/> in <paramref name="keptOnError"/>. 
        /// <para></para>
        /// Case <see cref="Error"/>: Calls <paramref name="onError"/> and stores the <typeparamref name="T2"/> in <paramref name="keptOnError"/> for later use. Stores the default value of <typeparamref name="T1"/> in <paramref name="keptOnSuccess"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the kept variable in case of <see cref="Success"/>.</typeparam>
        /// <typeparam name="T2">The type of the kept variable in case of <see cref="Error"/>.</typeparam>
        /// <param name="onSuccess">Is called and the <typeparamref name="T1"/> is stored in <paramref name="keptOnSuccess"/> if this is a <see cref="Success(TValue)"/>.</param>
        /// <param name="onError">Is called and the <typeparamref name="T2"/> is stored in <paramref name="keptOnError"/> if this is a <see cref="Error"/>.</param>
        /// <param name="keptOnSuccess">Stores the <typeparamref name="T1"/> from <paramref name="onSuccess"/> if This is a <see cref="Success(TValue)"/>.</param>
        /// <param name="keptOnError">Stores the <typeparamref name="T2"/> from <paramref name="onError"/> if This is a <see cref="Error"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Result<TValue> KeepEither<T1, T2>(Func<TValue, T1> onSuccess, Func<T2> onError, out T1 keptOnSuccess, out T2 keptOnError)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess), onError, nameof(onError));
            keptOnSuccess = _isSuccess ? onSuccess(_value) : default;
            keptOnError = _isSuccess ? default : onError();
            return this;
        }

        /// <summary>
        /// Case <see cref="Success(TValue)"/>: Calls <paramref name="onSuccess"/> and returns a <see cref="Success(TValue)"/>.
        /// <para></para>
        /// Case <see cref="Error"/>: Returns a <see cref="Error"/>.
        /// </summary>
        /// <param name="onSuccess">Is called if This is a <see cref="Success(TValue)"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Result<TValue> OnSuccess(Action<TValue> onSuccess)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess));
            if (_isSuccess)
            {
                onSuccess(_value);
            }
            return this;
        }

        /// <summary>
        /// Case <see cref="Success(TValue)"/>: Calls <paramref name="onSuccess"/> and returns a <see cref="Result{TValue}.Success(TValue)"/> with the <typeparamref name="TNewValue"/> from <paramref name="onSuccess"/>.
        /// <para></para>
        /// Case <see cref="Error"/>: Returns a <see cref="Result{TValue}.Error"/> of <typeparamref name="TNewValue"/>.
        /// </summary>
        /// <typeparam name="TNewValue">The new type of Value.</typeparam>
        /// <param name="onSuccess">Is called and the <typeparamref name="TNewValue"/> is returned as <see cref="Result{TValue}.Success(TValue)"/> if This is a <see cref="Success(TValue)"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Result<TNewValue> OnSuccess<TNewValue>(Func<TValue, TNewValue> onSuccess)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess));
            return _isSuccess ? onSuccess(_value) : new Result<TNewValue>();
        }

        /// <summary>
        /// Case <see cref="Success(TValue)"/>: Returns a <see cref="Success(TValue)"/>.
        /// <para></para>
        /// Case <see cref="Error"/>: Calls <paramref name="onError"/> and returns a <see cref="Success(TValue)"/>.
        /// </summary>
        /// <param name="onError">Is called if This is a <see cref="Error"/> and is supposed to fix the <see cref="Error"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Result<TValue> FixOnError(Func<TValue> onError)
        {
            Args.ExceptionIfNull(onError, nameof(onError));
            return _isSuccess ? this : onError();
        }

        /// <summary>
        /// Case <see cref="Success(TValue)"/>: Returns a <see cref="Success(TValue)"/>.
        /// <para></para>
        /// Case <see cref="Error"/>: Calls <paramref name="onError"/> and returns a <see cref="Error"/>.
        /// </summary>
        /// <param name="onError">Is called if This is a <see cref="Error"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Result<TValue> OnError(Action onError)
        {
            Args.ExceptionIfNull(onError, nameof(onError));
            if (_isSuccess)
            {
                onError();
            }
            return this;
        }

        /// <summary>
        /// Case <see cref="Success(TValue)"/>: Returns a <see cref="Result{TValue, TError}.Success(TValue)"/>.
        /// <para></para>
        /// Case <see cref="Error"/>: Calls <paramref name="onError"/> and returns a <see cref="Result{TValue, TError}.Error(TError)"/> with the <typeparamref name="TError"/> from <paramref name="onError"/>.
        /// </summary>
        /// <typeparam name="TError">The type of the Error.</typeparam>
        /// <param name="onError">Is called and returned as <see cref="Result{TValue, TError}.Error(TError)"/> if This is a <see cref="Error"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Result<TValue, TError> OnError<TError>(Func<TError> onError)
        {
            Args.ExceptionIfNull(onError, nameof(onError));
            return _isSuccess ? Result<TValue, TError>.Success(_value) : onError();
        }

        /// <summary>
        /// Case <see cref="Success(TValue)"/>: Calls <paramref name="onSuccess"/> and returns a <see cref="Success(TValue)"/>.
        /// <para></para>
        /// Case <see cref="Error"/>: Calls <paramref name="onError"/> and returns a <see cref="Error"/>.
        /// </summary>
        /// <param name="onSuccess">Is called if This is a <see cref="Success(TValue)"/>.</param>
        /// <param name="onError">Is called if This is a <see cref="Success(TValue)"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
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

        /// <summary>
        /// Case <see cref="Success(TValue)"/>: Calls <paramref name="onSuccess"/> and returns a <see cref="Result{TValue}.Success(TValue)"/> with the <typeparamref name="TNewValue"/> from <paramref name="onSuccess"/>. 
        /// <para></para>
        /// Case <see cref="Error"/>: Calls <paramref name="onError"/> and returns a <see cref="Result{TValue}.Error"/> of <typeparamref name="TNewValue"/>.
        /// </summary>
        /// <typeparam name="TNewValue">The new type of the Value.</typeparam>
        /// <param name="onSuccess">Is called and returned as <see cref="Result{TValue}.Success(TValue)"/> if This is a <see cref="Success"/>.</param>
        /// <param name="onError">Is called if This is a <see cref="Error"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
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

        /// <summary>
        /// Case <see cref="Success(TValue)"/>: Calls <paramref name="onSuccess"/> and returns a <see cref="Result{TValue,TError}.Success"/> of <typeparamref name="TValue"/>.
        /// <para></para>
        /// Case <see cref="Error"/>: Calls <paramref name="onError"/> and returns a <see cref="Result{TValue,TError}.Error(TError)"/> with the <typeparamref name="TError"/> from <paramref name="onError"/>.
        /// </summary>
        /// <typeparam name="TError">The type of the Error.</typeparam>
        /// <param name="onSuccess">Is called if This is a <see cref="Success(TValue)"/>.</param>
        /// <param name="onError">Is called and returned as <see cref="Result{TValue,TError}.Error(TError)"/> if This is a <see cref="Error"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>

        public Result<TValue, TError> Either<TError>(Action<TValue> onSuccess, Func<TError> onError)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess), onError, nameof(onError));
            if (_isSuccess)
            {
                onSuccess(_value);
                return Result<TValue, TError>.Success(_value);
            }
            return onError();
        }

        /// <summary>
        /// Case <see cref="Success(TValue)"/>: Calls <paramref name="onSuccess"/> and returns a <see cref="Result{TValue, TError}.Success(TValue)"/> of <typeparamref name="TNewValue"/> and <typeparamref name="TError"/> with the <typeparamref name="TNewValue"/> from <paramref name="onSuccess"/>.
        /// <para></para>
        /// Case <see cref="Error"/>: Calls <paramref name="onError"/> and returns a <see cref="Result{TValue, TError}.Error(TError)"/> of <typeparamref name="TValue"/> and <typeparamref name="TNewValue"/> with the <typeparamref name="TError"/> from <paramref name="onError"/>.
        /// </summary>
        /// <typeparam name="TNewValue">The new type of the Value.</typeparam>
        /// <typeparam name="TError">The type of the Error.</typeparam>
        /// <param name="onSuccess">Is called and returned as <see cref="Result{TValue, TError}.Success(TValue)"/> of <typeparamref name="TNewValue"/> and <typeparamref name="TError"/> if This is a <see cref="Success"/>.</param>
        /// <param name="onError">Is called and returned as <see cref="Result{TValue, TError}.Error(TError)"/> of <typeparamref name="TNewValue"/> and <typeparamref name="TError"/> if This is a <see cref="Error"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Result<TNewValue, TError> Either<TNewValue, TError>(Func<TValue, TNewValue> onSuccess, Func<TError> onError)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess), onError, nameof(onError));
            return _isSuccess ? Result<TNewValue, TError>.Success(onSuccess(_value)) : onError();
        }

        /// <summary>
        /// Case <see cref="Success(TValue)"/>: Calls <paramref name="onSuccess"/> and returns the <typeparamref name="T"/>.
        /// <para></para>
        /// Case <see cref="Error"/>: Calls <paramref name="onError"/> and returns the <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The return-type of the Method.</typeparam>
        /// <param name="onSuccess">Is called and the <typeparamref name="T"/> is returned if This is a <see cref="Success(TValue)"/>.</param>
        /// <param name="onError">Is called and the <typeparamref name="T"/> is returned if This is a <see cref="Error"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public T Match<T>(Func<TValue, T> onSuccess, Func<T> onError)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess), onError, nameof(onError));
            return _isSuccess ? onSuccess(_value) : onError();
        }
    }
}