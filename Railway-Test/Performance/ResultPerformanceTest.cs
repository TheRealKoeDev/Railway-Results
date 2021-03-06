﻿using System;
using System.Linq;
using System.Threading.Tasks;
using KoeLib.Patterns.Railway.Tasks;
using KoeLib.Patterns.Railway.Linq;
using KoeLib.Patterns.Railway.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace Railway.Test.Performance
{
    [TestClass]
    [DoNotParallelize]
    [TestCategory("Performance")]
    public class ResultPerformanceTest : PerformanceTestBase<Result, Result>
    {
        protected override Result Success => Result.Success();
        protected override Result Error => Result.Error();

        protected override Result LargeContentSuccess => Result.Success();
        protected override Result LargeContentError => Result.Error();

        protected override void TestSingleResult(Result result)
        {
            result.Bind(() => Result.Success());
            result.Bind(() => Result<int>.Success(1));
            result.Bind(() => Result<int, int>.Success(101), () => Result<int, int>.Error(500));
            result.BindOnError(() => Result.Error());
            result.BindOnError(() => ResultOrError<int>.Error(1));

            result.OnSuccess(() => { });
            result.OnSuccess(() => 1);
            result.OnError(() => { });
            result.OnError(() => 1);

            result.Either(() => { }, () => { });
            result.Either(() => 1, () => { });
            result.Either(() => { }, () => 1);
            result.Either(() => 1, () => 1);

            result.Ensure(() => true);
            result.Match(() => true, () => false);
        }        

        protected override void TestTaskOfResult(Result result)
        {
            Task[] tasks = new Task[]
            {
                result.Async().Bind(() => Result.Success()),
                result.Async().Bind(() => Result<int>.Success(1)),
                result.Async().Bind(() => Result<int, int>.Success(101), () => Result<int, int>.Error(500)),
                result.Async().BindOnError(() => Result.Error()),
                result.Async().BindOnError(() => ResultOrError<int>.Error(1)),

                result.Async().OnSuccess(() => { }),
                result.Async().OnSuccess(() => 1),
                result.Async().OnError(() => { }),
                result.Async().OnError(() => 1),

                result.Async().Either(() => { }, () => { }),
                result.Async().Either(() => 1, () => { }),
                result.Async().Either(() => { }, () => 1),
                result.Async().Either(() => 1, () => 1),

                result.Async().Ensure(() => true),
                result.Async().Match(() => true, () => false),
            };

            Task.WaitAll(tasks);
        }
        

        protected override void TestIEnumerableOfResult(Result[] results)
        {
            results.Bind(() => Result.Success()).Count();
            results.Bind(() => Result<int>.Success(1)).Count();
            results.Bind(() => Result<int, int>.Success(101), () => Result<int, int>.Error(500)).Count();
            results.BindOnError(() => Result.Error()).Count();
            results.BindOnError(() => ResultOrError<int>.Error(1)).Count();

            results.OnSuccess(() => { }).Count();
            results.OnSuccess(() => 1).Count();
            results.OnError(() => { }).Count();
            results.OnError(() => 1).Count();

            results.Either(() => { }, () => { }).Count();
            results.Either(() => 1, () => { }).Count();
            results.Either(() => { }, () => 1).Count();
            results.Either(() => 1, () => 1).Count();

            results.Ensure(() => true).Count();
            results.Match(() => true, () => false).Count();
        }

        protected override void TestKeepMethodsOfResult(Result result)
        {
            result.Keep(() => 100, out int num);
            result.KeepOnSuccess(() => 100, out int sCode);
            result.KeepOnError(() => 500, out int eCode);
            result.KeepEither(() => 100, () => 500, out int statusCode);
            result.KeepEither(() => 100, () => 500, out int successCode, out int errorCode);
        }

        protected override void TestLargeContent(Result result)
        {
            result.Bind(() => Result.Success());
            result.Bind(() => Result<string>.Success(LargeContent));
            result.Bind(() => Result<string, string>.Success(LargeContent), () => Result<string, string>.Error(LargeContent));
            result.BindOnError(() => ResultOrError<string>.Error(LargeContent));
            result.BindOnError(() => Result.Error());

            result.OnSuccess(() => { });
            result.OnSuccess(() => LargeContent);
            result.OnError(() => { });
            result.OnError(() => LargeContent);

            result.Either(() => { }, () => { });
            result.Either(() => LargeContent, () => { });
            result.Either(() => { }, () => LargeContent);
            result.Either(() => LargeContent, () => LargeContent);

            result.Ensure(() => true);
            result.Match(() => LargeContent, () => LargeContent);
        }
    }
}
