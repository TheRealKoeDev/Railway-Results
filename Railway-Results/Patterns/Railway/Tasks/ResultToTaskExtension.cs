using KoeLib.Patterns.Railway.Results;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KoeLib.Patterns.Railway.Tasks
{
    public static class ResultToTaskExtension
    {
        public static Task<TResult> Async<TResult>(this TResult result)
           where TResult : IResult
           => Task.FromResult(result);
    }
}
