using KoeLib.Patterns.Railway.Linq;
using KoeLib.Patterns.Railway.Results;
using KoeLib.Patterns.Railway.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;

namespace Railway.Test.Performance
{
    [TestClass]
    [DoNotParallelize]
    [TestCategory("Performance")]
    public class ResultWithErrorPerformanceTest : PerformanceTestBase<Else<int>, Else<string>>
    {
        protected override Else<int> Success => Else<int>.Success();
        protected override Else<int> Error => Else<int>.Error(500);

        protected override Else<string> LargeContentSuccess => Else<string>.Success();
        protected override Else<string> LargeContentError => Else<string>.Error(LargeContent);

        protected override void TestSingleResult(Else<int> result)
        {
            result.Bind(() => Else<int>.Success());
            result.Bind(() => Either<int, int>.Success(100));
            result.Bind(() => Either<int, int>.Success(101), error => Either<int, int>.Error(500));
            result.BindOnError(error => Else<int>.Error(500));
            result.BindOnError(error => Switch.Error());

            result.OnSuccess(() => { });
            result.OnSuccess(() => 101);
            result.OnError(error => { });
            result.OnError(error => 500);

            result.Either(() => { }, error => { });
            result.Either(() => 101, error => { });
            result.Either(() => { }, error => 500);
            result.Either(() => 101, error => 500);

            result.Ensure(() => true, () => 500);
            result.Match(() => true, error => false);
        }

        protected override void TestTaskOfResult(Else<int> result)
        {
            Task[] tasks = new Task[]
            {
                result.Async().Bind(() => Else<int>.Success()),
                result.Async().Bind(() => Either<int, int>.Success(100)),
                result.Async().Bind(() => Either<int, int>.Success(101), error => Either<int, int>.Error(500)),
                result.Async().BindOnError(error => Else<int>.Error(500)),
                result.Async().BindOnError(error => Switch.Error()),

                result.Async().OnSuccess(() => { }),
                result.Async().OnSuccess(() => 101),
                result.Async().OnError(error => { }),
                result.Async().OnError(error => 500),

                result.Async().Either(() => { }, error => { }),
                result.Async().Either(() => 101, error => { }),
                result.Async().Either(() => { }, error => 500),
                result.Async().Either(() => 101, error => 500),

                result.Async().Ensure(() => true, () => 500),
                result.Async().Match(() => true, error => false),
            };

            Task.WaitAll(tasks);
        }

        protected override void TestIEnumerableOfResult(Else<int>[] results)
        {
            results.Bind(() => Else<int>.Success()).Count();
            results.Bind(() => Either<int, int>.Success(100)).Count();
            results.Bind(() => Either<int, int>.Success(101), error => Either<int, int>.Error(500)).Count();
            results.BindOnError(error => Else<int>.Error(500)).Count();
            results.BindOnError(error => Switch.Error()).Count();

            results.OnSuccess(() => { }).Count();
            results.OnSuccess(() => 101).Count();
            results.OnError(error => { }).Count();
            results.OnError(error => 500).Count();

            results.Either(() => { }, error => { }).Count();
            results.Either(() => 101, error => { }).Count();
            results.Either(() => { }, error => 500).Count();
            results.Either(() => 101, error => 500).Count();

            results.Ensure(() => true, () => 500).Count();
            results.Match(() => true, error => false).Count();
        }

        protected override void TestKeepMethodsOfResult(Else<int> result)
        {
            result.Keep(() => 100, out int num);
            result.KeepOnSuccess(() => 100, out int sCode);
            result.KeepOnError(error => 500, out int eCode);
            result.KeepEither(() => 100, error => 500, out int statusCode);
            result.KeepEither(() => 100, error => 500, out int successCode, out int errorCode);
        }

        protected override void TestLargeContent(Else<string> result)
        {
            result.Bind(() => Else<string>.Success());
            result.Bind(() => Either<string, string>.Success(LargeContent));
            result.Bind(() => Either<string, string>.Success(LargeContent), error => Either<string, string>.Error(error));
            result.BindOnError(error => Else<string>.Error(error));
            result.BindOnError(error => Either<string, string>.Error(error));

            result.OnSuccess(() => { });
            result.OnSuccess(() => LargeContent);
            result.OnError(error => { });
            result.OnError(error => error);

            result.Either(() => { }, error => { });
            result.Either(() => LargeContent, error => { });
            result.Either(() => { }, error => error);
            result.Either(() => LargeContent, error => error);

            result.Ensure(() => true, () => LargeContent);
            result.Match(() => LargeContent, error => error);
        }
    }
}
