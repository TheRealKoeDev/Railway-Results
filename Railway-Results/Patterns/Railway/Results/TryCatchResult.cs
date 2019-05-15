﻿using KoeLib.Patterns.Railway.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace KoeLib.Patterns.Railway.Results
{
    public sealed class TryCatchResult<T>
    {
        private readonly Func<T> _resultFunc;

        internal TryCatchResult(Func<T> resultFunc)
        {
            Args.ExceptionIfNull(resultFunc, nameof(resultFunc));
            _resultFunc = resultFunc;
        }

        public T Catch(Func<Exception, T> onException)
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