using KoeLib.Patterns.Railway.Tools;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace KoeLib.Patterns.Railway.Results
{
    /// <summary>
    /// A plain Result/Monad without Value or Error, that can either be a <see cref="Success"/> or a <see cref="Error"/>.
    /// </summary>
    /// <seealso cref="IResult" />
    [DebuggerStepThrough]
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct Result : IResult
    {
        /// <summary>
        /// Is True this is a <see cref="Success"/> or False if this is a <see cref="Error"/>.
        /// </summary>
        private readonly bool _isSuccess;

        /// <summary>
        /// Creates a instance of <see cref="Result"/>.
        /// </summary>
        /// <param name="success">True for a <see cref="Success"/> or False for a <see cref="Error"/>.</param>
        private Result(bool success)
        {            
            _isSuccess = success;            
        }

        /// <summary>
        /// Creates a instance of <see cref="Result"/>.
        /// </summary>
        /// <param name="success">True for a <see cref="Success"/> or False for a <see cref="Error"/>.</param>
        public static Result Create(bool success) => new Result(success);

        /// <summary>
        /// Creates a instance of <see cref="Result{TValue}"/>.
        /// </summary>
        /// <typeparam name="TValue">The type of the Value.</typeparam>
        /// <param name="success">True for a <see cref="Result{TValue}.Success(TValue)"/> or False for a <see cref="Result{TValue}.Error"/>.</param>
        /// <param name="valueFunc">Provides the <typeparamref name="TValue"/> in case of <see cref="Result{TValue}.Success(TValue)"/>.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static Result<TValue> Create<TValue>(bool success, Func<TValue> valueFunc)
        {
            Args.ExceptionIfNull(valueFunc, nameof(valueFunc));
            return success ? Result<TValue>.Success(valueFunc()) : Result<TValue>.Error();
        }

        /// <summary>
        /// Creates a instance of <see cref="Result{TValue,TError}"/>.
        /// </summary>
        /// <typeparam name="TValue">The type of the Value.</typeparam>
        /// <typeparam name="TError">The type of the Error.</typeparam>
        /// <param name="success">True for a <see cref="Result{TValue,TError}.Success(TValue)"/> or False for a <see cref="Result{TValue,TError}.Error(TError)"/>.</param>
        /// <param name="valueFunc">Provides the <typeparamref name="TValue"/> in case of <see cref="Result{TValue,TError}.Success(TValue)"/>.</param>
        /// <param name="errorFunc">Provides the <typeparamref name="TError"/> in case of <see cref="Result{TValue,TError}.Error(TError)"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static Result<TValue, TError> Create<TValue, TError>(bool success, Func<TValue> valueFunc, Func<TError> errorFunc)
        {
            Args.ExceptionIfNull(valueFunc, nameof(valueFunc), errorFunc, nameof(errorFunc));
            return success ? Result<TValue, TError>.Success(valueFunc()) : Result<TValue, TError>.Error(errorFunc());
        }

        /// <summary>
        /// Creates a Success <see cref="Result"/>.
        /// </summary>
        /// <returns></returns>
        public static Result Success()
            => new Result(true);

        /// <summary>
        /// Creates a Error <see cref="Result"/>.
        /// </summary>
        /// <returns></returns>
        public static Result Error()
            => new Result(false);

        /// <summary>
        /// Returns a <see cref="TryCatchResult{T}"/> that executes the <paramref name="resultFunc"/> in a try-catch statement.
        /// </summary>
        /// <typeparam name="T">The return-value of <paramref name="resultFunc"/>.</typeparam>
        /// <param name="resultFunc">The function that is executed in <see cref="TryCatchResult{T}"/>.</param>
        /// <returns></returns>
        public static TryCatchResult<T> Try<T>(Func<T> resultFunc) => new TryCatchResult<T>(resultFunc);

        /// <summary>
        /// Returns a <see cref="Success"/> if <paramref name="results"/> is null or empty or only contains <see cref="Success"/>,
        /// otherwise it returns a <see cref="Error"/>.
        /// </summary>
        /// <param name="results">The Array of <see cref="Result"/> that is being combined.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Implicitly converts a <see cref="bool"/> to a <see cref="Result"/> by calling <see cref="Result(bool)"/>.
        /// </summary>
        /// <param name="success"></param>
        public static implicit operator Result(bool success)
            => new Result(success);

        /// <summary>
        /// Case <see cref="Success"/>: Calls <paramref name="onSuccess"/> and returns the <see cref="Result"/> from <paramref name="onSuccess"/>.
        /// <para></para>
        /// Case <see cref="Error"/>: Returns a <see cref="Error"/>.
        /// </summary>
        /// <param name="onSuccess">Is called and the <see cref="Result"/> is returned if This is a <see cref="Success"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Result Bind(Func<Result> onSuccess)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess));
            return _isSuccess ? onSuccess() : Error();
        }

        /// <summary>
        /// Case <see cref="Success"/>: Calls <paramref name="onSuccess"/> and returns the <see cref="Result{TValue}"/> from <paramref name="onSuccess"/>.
        /// <para></para>
        /// Case <see cref="Error"/>: Returns a <see cref="Result{TValue}.Error"/>
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="onSuccess">Is called and the <see cref="Result{TValue}"/> is returned it This is a <see cref="Success"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Result<TValue> Bind<TValue>(Func<Result<TValue>> onSuccess)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess));
            return _isSuccess ? onSuccess() : Result<TValue>.Error();
        }

        /// <summary>
        /// Case <see cref="Success"/>: Calls <paramref name="onSuccess"/> and returns the <typeparamref name="TResult"/> from <paramref name="onSuccess"/>.
        /// <para></para>
        /// Case <see cref="Error"/>: Calls <paramref name="onError"/> and returns the <typeparamref name="TResult"/> from <paramref name="onError"/>.
        /// </summary>
        /// <typeparam name="TResult">The type of <see cref="IResult"/> that <paramref name="onSuccess"/> and <paramref name="onError"/> are returning.</typeparam>
        /// <param name="onSuccess">Is called and the <typeparamref name="TResult"/> is returned if This is a <see cref="Success"/>.</param>
        /// <param name="onError">Is called and the <typeparamref name="TResult"/> is returned if This is a <see cref="Error"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public TResult Bind<TResult>(Func<TResult> onSuccess, Func<TResult> onError)
           where TResult : IResult
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess), onError, nameof(onError));
            return _isSuccess ? onSuccess() : onError();
        }

        /// <summary>
        /// Case <see cref="Success"/>: Returns a <see cref="Success"/>.
        /// <para></para>
        /// Case <see cref="Error"/>: Calls <paramref name="onError"/> and returns the <see cref="Result"/> from <paramref name="onError"/>.
        /// </summary>
        /// <param name="onError">Is called and the <see cref="Result"/> is returned if This is a <see cref="Error"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Result BindOnError(Func<Result> onError)
        {
            Args.ExceptionIfNull(onError, nameof(onError));
            return _isSuccess ? Success() : onError();
        }

        /// <summary>
        /// Case <see cref="Success"/>: Returns a <see cref="ResultOrError{TError}.Success"/>.
        /// <para></para>
        /// Case <see cref="Error"/>: Calls <paramref name="onError"/> and returns the <see cref="ResultOrError{TError}"/> from <paramref name="onError"/>.
        /// </summary>
        /// <typeparam name="TError">The type of Error.</typeparam>
        /// <param name="onError">Is called and the <see cref="ResultOrError{TError}"/> is returned if This is a <see cref="Error"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public ResultOrError<TError> BindOnError<TError>(Func<ResultOrError<TError>> onError)
        {
            Args.ExceptionIfNull(onError, nameof(onError));
            return _isSuccess ? ResultOrError<TError>.Success() : onError();
        }

        /// <summary>
        /// Case <see cref="Success"/>: Calls <paramref name="onSuccess"/> and returns a <see cref="Success"/>.
        /// <para></para>
        /// Case <see cref="Error"/>: Returns a <see cref="Error"/>.
        /// </summary>
        /// <param name="onSuccess">Is called if This is a <see cref="Success"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Result OnSuccess(Action onSuccess)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess));
            if (_isSuccess)
            {
                onSuccess();
            }
            return this;
        }

        /// <summary>
        /// Case <see cref="Success"/>: Calls <paramref name="onSuccess"/> and returns a <see cref="Result{TValue}.Success(TValue)"/> with the <typeparamref name="TValue"/> from <paramref name="onSuccess"/>.
        /// <para></para>
        /// Case <see cref="Error"/>: Returns a <see cref="Result{TValue}.Error"/>.
        /// </summary>
        /// <typeparam name="TValue">The type of Value.</typeparam>
        /// <param name="onSuccess">Is called and the <typeparamref name="TValue"/> is returned as <see cref="Result{TValue}.Success(TValue)"/> if This is a <see cref="Success"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Result<TValue> OnSuccess<TValue>(Func<TValue> onSuccess)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess));
            return _isSuccess ? onSuccess() : Result<TValue>.Error();
        }        

        /// <summary>
        /// Case <see cref="Success"/>: Returns a <see cref="Success"/>.
        /// <para></para>
        /// Case <see cref="Error"/>: Calls <paramref name="onError"/> and returns a <see cref="Error"/>.
        /// </summary>
        /// <param name="onError">Is called if This is a <see cref="Error"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Result OnError(Action onError)
        {
            Args.ExceptionIfNull(onError, nameof(onError));
            if (!_isSuccess)
            {
                onError();
            }
            return this;
        }

        /// <summary>
        /// Case <see cref="Success"/>: Returns a <see cref="ResultOrError{TError}.Success"/>.
        /// <para></para>
        /// Case <see cref="Error"/>: Calls <paramref name="onError"/> and returns a <see cref="ResultOrError{TError}.Error(TError)"/> with the <typeparamref name="TError"/> from <paramref name="onError"/>.
        /// </summary>
        /// <typeparam name="TError">The type of the Error.</typeparam>
        /// <param name="onError">Is called and returned as <see cref="ResultOrError{TError}.Error(TError)"/> if This is a <see cref="Error"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public ResultOrError<TError> OnError<TError>(Func<TError> onError)
        {
            Args.ExceptionIfNull(onError, nameof(onError));
            return _isSuccess ? ResultOrError<TError>.Success() : onError();
        }

        /// <summary>
        /// Case <see cref="Success"/>: Returns a <see cref="Success"/>.
        /// <para></para>
        /// Case <see cref="Error"/>: Calls <paramref name="onError"/> and returns a <see cref="Success"/>.
        /// </summary>
        /// <param name="onError">Is called if This is a <see cref="Error"/> and is supposed to fix the <see cref="Error"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Result FixOnError(Action onError)
        {
            Args.ExceptionIfNull(onError, nameof(onError));
            if (!_isSuccess)
            {
                onError();
            }
            return Success();
        }

        /// <summary>
        /// Case <see cref="Success"/>: Calls <paramref name="onSuccess"/> and returns a <see cref="Success"/>.
        /// <para></para>
        /// Case <see cref="Error"/>: Calls <paramref name="onError"/> and returns a <see cref="Error"/>.
        /// </summary>
        /// <param name="onSuccess">Is called if This is a <see cref="Success"/>.</param>
        /// <param name="onError">Is called if This is a <see cref="Error"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
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

        /// <summary>
        /// Case <see cref="Success"/>: Calls <paramref name="onSuccess"/> and returns a <see cref="Result{TValue}.Success(TValue)"/> with the <typeparamref name="TValue"/> from <paramref name="onSuccess"/>. 
        /// <para></para>
        /// Case <see cref="Error"/>: Calls <paramref name="onError"/> and returns a <see cref="Result{TValue}.Error"/>.
        /// </summary>
        /// <typeparam name="TValue">The type of the Value.</typeparam>
        /// <param name="onSuccess">Is called and returned as <see cref="Result{TValue}.Success(TValue)"/> if This is a <see cref="Success"/>.</param>
        /// <param name="onError">Is called if This is a <see cref="Error"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
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

        /// <summary>
        /// Case <see cref="Success"/>: Calls <paramref name="onSuccess"/> and returns a <see cref="ResultOrError{TError}.Success"/>.
        /// <para></para>
        /// Case <see cref="Error"/>: Calls <paramref name="onError"/> and returns a <see cref="ResultOrError{TError}.Error(TError)"/> with the <typeparamref name="TError"/> from <paramref name="onError"/>.
        /// </summary>
        /// <typeparam name="TError">The type of the Error.</typeparam>
        /// <param name="onSuccess">Is called if This is a <see cref="Success"/>.</param>
        /// <param name="onError">Is called and returned as <see cref="ResultOrError{TError}.Error(TError)"/> if This is a <see cref="Error"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public ResultOrError<TError> Either<TError>(Action onSuccess, Func<TError> onError)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess), onError, nameof(onError));
            if (_isSuccess)
            {
                onSuccess();
                return ResultOrError<TError>.Success();
            }
            return onError();
        }

        /// <summary>
        /// Case <see cref="Success"/>: Calls <paramref name="onSuccess"/> and returns a <see cref="Result{TValue, TError}.Success(TValue)"/> with the <typeparamref name="TValue"/> from <paramref name="onSuccess"/>.
        /// <para></para>
        /// Case <see cref="Error"/>: Calls <paramref name="onError"/> and returns a <see cref="Result{TValue, TError}.Error(TError)"/> with the <typeparamref name="TError"/> from <paramref name="onError"/>.
        /// </summary>
        /// <typeparam name="TValue">The type of the Value.</typeparam>
        /// <typeparam name="TError">The type of the Error.</typeparam>
        /// <param name="onSuccess">Is called and returned as <see cref="Result{TValue, TError}.Success(TValue)"/> if This is a <see cref="Success"/>.</param>
        /// <param name="onError">Is called and returned as <see cref="Result{TValue, TError}.Error(TError)"/> if This is a <see cref="Error"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Result<TValue, TError> Either<TValue, TError>(Func<TValue> onSuccess, Func<TError> onError)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess), onError, nameof(onError));
            return _isSuccess ? Result<TValue, TError>.Success(onSuccess()) : Result<TValue, TError>.Error(onError());
        }

        /// <summary>
        /// Case <see cref="Success"/>: Calls <paramref name="onSuccess"/> and returns the <typeparamref name="T"/>.
        /// <para></para>
        /// Case <see cref="Error"/>: Calls <paramref name="onError"/> and returns the <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The return-type of the Method.</typeparam>
        /// <param name="onSuccess">Is called and the <typeparamref name="T"/> is returned if This is a <see cref="Success"/>.</param>
        /// <param name="onError">Is called and the <typeparamref name="T"/> is returned if This is a <see cref="Error"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public T Match<T>(Func<T> onSuccess, Func<T> onError)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess), onError, nameof(onError));
            return _isSuccess ? onSuccess() : onError();
        }

        /// <summary>
        /// Case <see cref="Success"/>: Calls <paramref name="condition"/> and returns a <see cref="Success"/> if the <paramref name="condition"/> is true or a <see cref="Error"/> if the <paramref name="condition"/> is false.
        /// <para></para>
        /// Case <see cref="Error"/>: Returns a <see cref="Error"/>.
        /// </summary>
        /// <param name="condition">Is called if This is a <see cref="Success"/> and the <see cref="bool"/> is turned into a <see cref="Result(bool)"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Result Ensure(Func<bool> condition)
        {
            Args.ExceptionIfNull(condition, nameof(condition));
            return _isSuccess && condition();
        }

        /// <summary>
        /// Case <see cref="Success"/>: Calls <paramref name="onSuccess"/> and stores the <typeparamref name="T"/> in <paramref name="kept"/> for later use.
        /// <para></para>
        /// Case <see cref="Error"/>: Stores the default-value of <typeparamref name="T"/> in <paramref name="kept"/>.
        /// </summary>
        /// <typeparam name="T">The type of the kept variable.</typeparam>
        /// <param name="onSuccess">Is called and the <typeparamref name="T"/> is stored in <paramref name="kept"/> if This is a <see cref="Success"/>.</param>
        /// <param name="kept">Stores the <typeparamref name="T"/> from <paramref name="onSuccess"/> if This is a <see cref="Success"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Result KeepOnSuccess<T>(Func<T> onSuccess, out T kept)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess));
            kept = _isSuccess ? onSuccess() : default;
            return this;
        }

        /// <summary>
        /// Case <see cref="Success"/>: Stores the default-value of <typeparamref name="T"/> in <paramref name="kept"/>.
        /// <para></para>
        /// Case <see cref="Error"/>: Calls <paramref name="onError"/> and stores the <typeparamref name="T"/> in <paramref name="kept"/> for later use.
        /// </summary>
        /// <typeparam name="T">The type of the kept variable.</typeparam>
        /// <param name="onError">Is called and the <typeparamref name="T"/> is stored in <paramref name="kept"/> if this is a <see cref="Error"/>.</param>
        /// <param name="kept">Stores the <typeparamref name="T"/> from <paramref name="onError"/> if This is a <see cref="Error"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Result KeepOnError<T>(Func<T> onError, out T kept)
        {
            Args.ExceptionIfNull(onError, nameof(onError));
            kept = _isSuccess ? default : onError();
            return this;
        }

        /// <summary>
        /// Case <see cref="Success"/>: Calls <paramref name="onSuccess"/> and stores the <typeparamref name="T"/> in <paramref name="kept"/> for later use.
        /// <para></para>
        /// Case <see cref="Error"/>: Calls <paramref name="onError"/> and stores the <typeparamref name="T"/> in <paramref name="kept"/> for later use.
        /// </summary>
        /// <typeparam name="T">The type of the kept variable.</typeparam>
        /// <param name="onSuccess">Is called and the <typeparamref name="T"/> is stored in <paramref name="kept"/> if this is a <see cref="Success"/>.</param>
        /// <param name="onError">Is called and the <typeparamref name="T"/> is stored in <paramref name="kept"/> if this is a <see cref="Error"/>.</param>
        /// <param name="kept">Stores the <typeparamref name="T"/> from <paramref name="onError"/> or <paramref name="onSuccess"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Result KeepEither<T>(Func<T> onSuccess, Func<T> onError, out T kept)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess), onError, nameof(onError));
            kept = _isSuccess ? onSuccess() : onError();
            return this;
        }

        /// <summary>
        /// Case <see cref="Success"/>: Calls <paramref name="onSuccess"/> and stores the <typeparamref name="T1"/> in <paramref name="keptOnSuccess"/> for later use. Stores the default-value of <typeparamref name="T2"/> in <paramref name="keptOnError"/>. 
        /// <para></para>
        /// Case <see cref="Error"/>: Calls <paramref name="onError"/> and stores the <typeparamref name="T2"/> in <paramref name="keptOnError"/> for later use. Stores the default-value of <typeparamref name="T1"/> in <paramref name="keptOnSuccess"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the kept variable in case of <see cref="Success"/>.</typeparam>
        /// <typeparam name="T2">The type of the kept variable in case of <see cref="Error"/>.</typeparam>
        /// <param name="onSuccess">Is called and the <typeparamref name="T1"/> is stored in <paramref name="keptOnSuccess"/> if this is a <see cref="Success"/>.</param>
        /// <param name="onError">Is called and the <typeparamref name="T2"/> is stored in <paramref name="keptOnError"/> if this is a <see cref="Error"/>.</param>
        /// <param name="keptOnSuccess">Stores the <typeparamref name="T1"/> from <paramref name="onSuccess"/> if This is a <see cref="Success"/>.</param>
        /// <param name="keptOnError">Stores the <typeparamref name="T2"/> from <paramref name="onError"/> if This is a <see cref="Error"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Result KeepEither<T1, T2>(Func<T1> onSuccess, Func<T2> onError, out T1 keptOnSuccess, out T2 keptOnError)
        {
            Args.ExceptionIfNull(onSuccess, nameof(onSuccess), onError, nameof(onError));
            keptOnSuccess = _isSuccess ? onSuccess() : default;
            keptOnError = _isSuccess ? default : onError();
            return this;
        }
    }
}