using KoeLib.Patterns.Railway.Linq;
using KoeLib.Patterns.Railway.Results;
using KoeLib.Patterns.Railway.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Railway.Test.Performance
{
    [TestClass]
    [DoNotParallelize]
    [TestCategory("Performance")]
    public class ResultWithErrorPerformanceTest : PerformanceTestBase<ResultWithError<int>, ResultWithError<string>>
    {
        protected override ResultWithError<int> Success => ResultWithError<int>.Success();
        protected override ResultWithError<int> Error => ResultWithError<int>.Error(500);

        protected override ResultWithError<string> LargeContentSuccess => ResultWithError<string>.Success();
        protected override ResultWithError<string> LargeContentError => ResultWithError<string>.Error(LargeContent);

        protected override void TestSingleResult(ResultWithError<int> result)
        {
            result.Bind(() => ResultWithError<int>.Success());
            result.Bind(() => Result<int, int>.Success(100));
            result.Bind(() => Result<int, int>.Success(101), error => Result<int, int>.Error(500));
            result.BindOnError(error => ResultWithError<int>.Error(500));
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

        protected override void TestTaskOfResult(ResultWithError<int> result)
        {
            Task[] tasks = new Task[]
            {
                result.Async().Bind(() => ResultWithError<int>.Success()),
                result.Async().Bind(() => Result<int, int>.Success(100)),
                result.Async().Bind(() => Result<int, int>.Success(101), error => Result<int, int>.Error(500)),
                result.Async().BindOnError(error => ResultWithError<int>.Error(500)),
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

        protected override void TestIEnumerableOfResult(ResultWithError<int>[] results)
        {
            results.Bind(() => ResultWithError<int>.Success());
            results.Bind(() => Result<int, int>.Success(100));
            results.Bind(() => Result<int, int>.Success(101), error => Result<int, int>.Error(500));
            results.BindOnError(error => ResultWithError<int>.Error(500));
            results.BindOnError(error => Result.Error());

            results.OnSuccess(() => { });
            results.OnSuccess(() => 101);
            results.OnError(error => { });
            results.OnError(error => 500);

            results.Either(() => { }, error => { });
            results.Either(() => 101, error => { });
            results.Either(() => { }, error => 500);
            results.Either(() => 101, error => 500);

            results.Ensure(() => true, () => 500);
            results.Match(() => true, error => false);
        }

        protected override void TestKeepMethodsOfResult(ResultWithError<int> result)
        {
            result.Keep(() => 100, out int num);
            result.KeepOnSuccess(() => 100, out int sCode);
            result.KeepOnError(error => 500, out int eCode);
            result.KeepEither(() => 100, error => 500, out int statusCode);
            result.KeepEither(() => 100, error => 500, out int successCode, out int errorCode);
        }

        protected override void TestLargeContent(ResultWithError<string> result)
        {
            result.Bind(() => ResultWithError<string>.Success());
            result.Bind(() => Result<string, string>.Success(LargeContent));
            result.Bind(() => Result<string, string>.Success(LargeContent), error => Result<string, string>.Error(error));
            result.BindOnError(error => ResultWithError<string>.Error(error));
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
