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
    public class ResultOfErrorPerformanceTest : PerformanceTestBase<ResultOrError<int>, ResultOrError<string>>
    {
        protected override ResultOrError<int> Success => ResultOrError<int>.Success();
        protected override ResultOrError<int> Error => ResultOrError<int>.Error(500);

        protected override ResultOrError<string> LargeContentSuccess => ResultOrError<string>.Success();
        protected override ResultOrError<string> LargeContentError => ResultOrError<string>.Error(LargeContent);

        protected override void TestSingleResult(ResultOrError<int> result)
        {
            result.Bind(() => ResultOrError<int>.Success());
            result.Bind(() => Result<int, int>.Success(100));
            result.Bind(() => Result<int, int>.Success(101), error => Result<int, int>.Error(500));
            result.BindOnError(error => ResultOrError<int>.Error(500));
            result.BindOnError(error => Result.Error());

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

        protected override void TestTaskOfResult(ResultOrError<int> result)
        {
            Task[] tasks = new Task[]
            {
                result.Async().Bind(() => ResultOrError<int>.Success()),
                result.Async().Bind(() => Result<int, int>.Success(100)),
                result.Async().Bind(() => Result<int, int>.Success(101), error => Result<int, int>.Error(500)),
                result.Async().BindOnError(error => ResultOrError<int>.Error(500)),
                result.Async().BindOnError(error => Result.Error()),

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

        protected override void TestIEnumerableOfResult(ResultOrError<int>[] results)
        {
            results.Bind(() => ResultOrError<int>.Success()).Count();
            results.Bind(() => Result<int, int>.Success(100)).Count();
            results.Bind(() => Result<int, int>.Success(101), error => Result<int, int>.Error(500)).Count();
            results.BindOnError(error => ResultOrError<int>.Error(500)).Count();
            results.BindOnError(error => Result.Error()).Count();

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

        protected override void TestKeepMethodsOfResult(ResultOrError<int> result)
        {
            result.Keep(() => 100, out int num);
            result.KeepOnSuccess(() => 100, out int sCode);
            result.KeepOnError(error => 500, out int eCode);
            result.KeepEither(() => 100, error => 500, out int statusCode);
            result.KeepEither(() => 100, error => 500, out int successCode, out int errorCode);
        }

        protected override void TestLargeContent(ResultOrError<string> result)
        {
            result.Bind(() => ResultOrError<string>.Success());
            result.Bind(() => Result<string, string>.Success(LargeContent));
            result.Bind(() => Result<string, string>.Success(LargeContent), error => Result<string, string>.Error(error));
            result.BindOnError(error => ResultOrError<string>.Error(error));
            result.BindOnError(error => Result<string, string>.Error(error));

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
