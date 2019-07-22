using KoeLib.Patterns.Railway.Tools;
using System;
using System.Diagnostics;

namespace KoeLib.Patterns.Railway.Results
{
    /// <summary>
    /// Try-Catch class for railway oriented programming.
    /// </summary>
    /// <typeparam name="TResult">The Result after calling <see cref="Catch(Func{Exception, TResult})"/>.</typeparam>
    [DebuggerStepThrough]
    public readonly struct TryCatchResult<TResult>
    {
        /// <summary>
        /// The function that is called and try catched.
        /// </summary>
        private readonly Func<TResult> _resultFunc;

        /// <summary>
        /// Creates an instance of <see cref="TryCatchResult{T}"/>.
        /// </summary>
        /// <param name="resultFunc">The function that is called and try catched.</param>
        internal TryCatchResult(Func<TResult> resultFunc)
        {
            Args.ExceptionIfNull(resultFunc, nameof(resultFunc));
            _resultFunc = resultFunc;
        }

        /// <summary>
        /// Try-Catches the <see cref="Func{T}"/> and converts the <see cref="Exception"/> to a <typeparamref name="TResult"/> on occurence.
        /// </summary>
        /// <param name="onException">The function that converts the Exception to a <typeparamref name="TResult"/>.</param>
        /// <returns></returns>
        public TResult Catch(Func<Exception, TResult> onException)
        {
            Args.ExceptionIfNull(onException, nameof(onException));
            try
            {
                return _resultFunc();
            }
            catch (Exception e)
            {
                return onException(e);
            }
        }
    }
}
