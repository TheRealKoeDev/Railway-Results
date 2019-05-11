using System;
using System.Collections.Generic;
using System.Linq;
using KoeLib.Patterns.Railway.Linq;
using KoeLib.Patterns.Railway.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Railway.Test
{
    [TestClass]
    public class IEnumerableOfResultPerformanceTest
    {
        private const int itemCount = 100;
        private const int iterationCount = 1000000;

        [TestMethod]
        public void Test()
        {
            for (int i = 0; i < iterationCount; i++)
            {
                Successes();
            }
        }

        private void Successes()
        {
            Result[] results = new Result[itemCount];
            for (int i = 0; i < itemCount; i++)
            {
                results[i] = Result.Success();
            }

            var result1 = results.OnSuccess(() => { }).Count();
            var result2 = results.OnSuccess(() => 1).Count();
            var result3 = results.Bind(() => Result.Success()).Count();
            var result4 = results.Bind(() => Result<int>.Success(1)).Count();
            var result5 = results.Bind(() => Result<int>.Success(1), () => Result<int>.Error()).Count();

            var result6 = results.OnError(() => { }).Count();
            var result7 = results.OnError(() => 1).Count();
            var result8 = results.BindOnError(() => Result.Error()).Count();
            var result9 = results.BindOnError(() => ResultWithError<int>.Error(1)).Count();

            var result10 = results.Either(() => { }, () => { }).Count();
            var result11 = results.Either(() => 1, () => { }).Count();
            var result12 = results.Either(() => { }, () => 1).Count();
            var result13 = results.Either(() => 1, () => 1).Count();

            var result14 = results.Match(() => true, () => false).Count();
        }
    }
}
