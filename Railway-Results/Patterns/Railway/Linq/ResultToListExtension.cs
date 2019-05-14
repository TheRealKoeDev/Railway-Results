using KoeLib.Patterns.Railway.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KoeLib.Patterns.Railway.Linq
{
    public static class ResultToListExtension
    {
        public static IEnumerable<TResult> ToEnumerable<TResult>(this TResult target, params TResult[] results)
            where TResult: IResult
        {
            yield return target;
            using (IEnumerator<TResult> enumerator = (results ?? new TResult[0]).AsEnumerable().GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current;
                }
            }
        }
    }
}
