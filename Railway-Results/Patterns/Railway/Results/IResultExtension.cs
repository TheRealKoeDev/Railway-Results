using KoeLib.Patterns.Railway.Tools;
using System;
using System.Diagnostics;

namespace KoeLib.Patterns.Railway.Results
{
    [DebuggerStepThrough]
    public static class IResultExtension
    {
        /// <summary>
        /// Allways calls the <paramref name="action"/>.
        /// </summary>
        /// <typeparam name="TResult">The type of This Result.</typeparam>
        /// <param name="target">This Result.</param>
        /// <param name="action">The Action.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static TResult Then<TResult>(this TResult target, Action action)
           where TResult : IResult
        {
            Args.ExceptionIfNull(action, nameof(action));
            action();
            return target;
        }

        /// <summary>
        /// Allways calls and returns the <typeparamref name="TNewResult"/> of <paramref name="nextResult"/>.
        /// </summary>
        /// <typeparam name="TResult">The type of This Result.</typeparam>
        /// <typeparam name="TNewResult">The type of the new Result thats returned.</typeparam>
        /// <param name="_">This Result.</param>
        /// <param name="nextResult">The function that returns the <typeparamref name="TNewResult"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static TNewResult Then<TResult, TNewResult>(this TResult _, Func<TNewResult> nextResult)
           where TResult : IResult
           where TNewResult : IResult
        {
            Args.ExceptionIfNull(nextResult, nameof(nextResult));
            return nextResult();
        }

        /// <summary>
        /// Allways calls and returns the <typeparamref name="TNewResult"/> of <paramref name="newResultFunc"/>.
        /// </summary>
        /// <typeparam name="TResult">The type of This Result.</typeparam>
        /// <typeparam name="TNewResult">The type of the new Result thats returned.</typeparam>
        /// <param name="target">This Result.</param>
        /// <param name="newResultFunc">The function that returns the <typeparamref name="TNewResult"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static TNewResult Then<TResult, TNewResult>(this TResult target, Func<TResult, TNewResult> newResultFunc)
            where TResult : IResult
            where TNewResult : IResult
        {
            Args.ExceptionIfNull(newResultFunc, nameof(newResultFunc));
            return newResultFunc(target);
        }

        /// <summary>
        /// Calls <paramref name="keepFunc"/> and stores the <typeparamref name="T"/> in <paramref name="kept"/>.
        /// </summary>
        /// <typeparam name="T">The type of the kept value.</typeparam>
        /// <typeparam name="TResult">This Result.</typeparam>
        /// <param name="target">This Result.</param>
        /// <param name="keepFunc">The funtion that provides the <typeparamref name="T"/> to keep in <paramref name="kept"/>.</param>
        /// <param name="kept">The kept value.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static TResult Keep<T, TResult>(this TResult target, Func<T> keepFunc, out T kept)
            where TResult: IResult
        {
            Args.ExceptionIfNull(keepFunc, nameof(keepFunc));
            kept = keepFunc();
            return target;
        }

        /// <summary>
        /// Returns a <see cref="TryCatchResult{T}"/> that tries to call <paramref name="resultFunc"/> in a <see cref="TryCatchResult{T}.Catch(Func{Exception, T})"/>.
        /// </summary>
        /// <typeparam name="TResult">This result.</typeparam>
        /// <typeparam name="T">The return value of the <see cref="TryCatchResult{T}.Catch(Func{Exception, T})"/>.</typeparam>
        /// <param name="result">This Result.</param>
        /// <param name="resultFunc">The function thats being called in a <see cref="TryCatchResult{T}.Catch(Func{Exception, T})"/></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static TryCatchResult<T> Try<TResult, T>(this TResult result, Func<TResult, T> resultFunc)
           where TResult : IResult
        {
            Args.ExceptionIfNull(resultFunc, nameof(resultFunc));
            return new TryCatchResult<T>(() => resultFunc(result));
        }
    }
}
