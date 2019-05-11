using System;
using KoeLib.Patterns.Railway.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Railway.Test
{
    [TestClass]
    public class ResultPerformanceTest
    {
        public const int Iterations = 100000000;

        [TestMethod]
        public void Test()
        {
            for (int i = 0; i < Iterations; i++)
            {
                Success();
            }
        }

        private void Success()
        {
            Result result = Result.Success();
            var result1 = result.OnSuccess(() => { });
            var result2 = result.OnSuccess(() => 1);
            var result3 = result.Bind(() => Result.Success());
            var result4 = result.Bind(() => Result<int>.Success(1));
            var result5 = result.Bind(() => Result<int>.Success(1), () => Result<int>.Error());

            var result6 = result.OnError(() => { });
            var result7 = result.OnError(() => 1);
            var result8 = result.BindOnError(() => Result.Error());
            var result9 = result.BindOnError(() => ResultWithError<int>.Error(1));

            var result10 = result.Either(() => { }, () => { });
            var result11 = result.Either(() => 1, () => { });
            var result12 = result.Either(() => { }, () => 1);
            var result13 = result.Either(() => 1, () => 1);

            var result14 = result.Match(() => true, () => false);
        }
    }
}
