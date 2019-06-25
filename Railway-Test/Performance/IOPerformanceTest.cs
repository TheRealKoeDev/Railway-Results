using System;
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
    public class ResultPerformanceTest : PerformanceTestBase<KoeLib.Patterns.Railway.Results.Switch, KoeLib.Patterns.Railway.Results.Switch>
    {
        protected override System.Diagnostics.Switch Success => KoeLib.Patterns.Railway.Results.Switch.Success();
        protected override Switch Error => KoeLib.Patterns.Railway.Results.Switch.Error();

        protected override Switch LargeContentSuccess => KoeLib.Patterns.Railway.Results.Switch.Success();
        protected override Switch LargeContentError => KoeLib.Patterns.Railway.Results.Switch.Error();

        protected override void TestSingleResult(KoeLib.Patterns.Railway.Results.Switch result)
        {
            result.Bind(() => KoeLib.Patterns.Railway.Results.Switch.Success());
            result.Bind(() => If<int>.Success(1));
            result.Bind(() => Either<int, int>.Success(101), () => Either<int, int>.Error(500));
            result.BindOnError(() => KoeLib.Patterns.Railway.Results.Switch.Error());
            result.BindOnError(() => Else<int>.Error(1));

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

        protected override void TestTaskOfResult(KoeLib.Patterns.Railway.Results.Switch result)
        {
            Task[] tasks = new Task[]
            {
                result.Async().Bind(() => KoeLib.Patterns.Railway.Results.Switch.Success()),
                result.Async().Bind(() => If<int>.Success(1)),
                result.Async().Bind(() => Either<int, int>.Success(101), () => Either<int, int>.Error(500)),
                result.Async().BindOnError(() => KoeLib.Patterns.Railway.Results.Switch.Error()),
                result.Async().BindOnError(() => Else<int>.Error(1)),

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
        

        protected override void TestIEnumerableOfResult(KoeLib.Patterns.Railway.Results.Switch[] results)
        {
            results.Bind(() => Switch.Success()).Count();
            results.Bind(() => If<int>.Success(1)).Count();
            results.Bind(() => Either<int, int>.Success(101), () => Either<int, int>.Error(500)).Count();
            results.BindOnError(() => Switch.Error()).Count();
            results.BindOnError(() => Else<int>.Error(1)).Count();

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

        protected override void TestKeepMethodsOfResult(KoeLib.Patterns.Railway.Results.Switch result)
        {
            result.Keep(() => 100, out int num);
            result.KeepOnSuccess(() => 100, out int sCode);
            result.KeepOnError(() => 500, out int eCode);
            result.KeepEither(() => 100, () => 500, out int statusCode);
            result.KeepEither(() => 100, () => 500, out int successCode, out int errorCode);
        }

        protected override void TestLargeContent(KoeLib.Patterns.Railway.Results.Switch result)
        {
            result.Bind(() => KoeLib.Patterns.Railway.Results.Switch.Success());
            result.Bind(() => If<string>.Success(LargeContent));
            result.Bind(() => Either<string, string>.Success(LargeContent), () => Either<string, string>.Error(LargeContent));
            result.BindOnError(() => Else<string>.Error(LargeContent));
            result.BindOnError(() => KoeLib.Patterns.Railway.Results.Switch.Error());

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
