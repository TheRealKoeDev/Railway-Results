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
    public class ResultWithValueOrErrorPerformanceTest : PerformanceTestBase<Either<int, int>, Either<string, string>>
    {
        protected override Either<int, int> Success => Either<int, int>.Success(100);
        protected override Either<int, int> Error => Either<int, int>.Error(500);

        protected override Either<string, string> LargeContentSuccess => Either<string, string>.Success(LargeContent);
        protected override Either<string, string> LargeContentError => Either<string, string>.Error(LargeContent);

        protected override void TestSingleResult(Either<int, int> result)
        {
            result.Bind(value => Else<int>.Success());
            result.Bind(value => Either<int, int>.Success(101));
            result.Bind(value => Either<int, int>.Success(101), error => Either<int, int>.Error(500));
            result.BindOnError(error => If<int>.Error());
            result.BindOnError(error => Either<int, int>.Error(500));

            result.OnSuccess(value => { });
            result.OnSuccess(value => 101);
            result.OnError(error => { });
            result.OnError(error => 500);

            result.OnEither(value => { }, error => { });
            result.OnEither(value => 101, error => { });
            result.OnEither(value => { }, error => 500);
            result.OnEither(value => 101, error => 500);

            result.Ensure(value => true, () => 404);
            result.Match(value => true, error => false);
        }

        protected override void TestTaskOfResult(Either<int, int> result)
        {
            Task[] tasks = new Task[]
            {
                result.Async().Bind(value => Else<int>.Success()),
                result.Async().Bind(value => Either<int, int>.Success(101)),
                result.Async().Bind(value => Either<int, int>.Success(101), error => Either<int, int>.Error(500)),
                result.Async().BindOnError(error => If<int>.Error()),
                result.Async().BindOnError(error => Either<int, int>.Error(500)),

                result.Async().OnSuccess(value => { }),
                result.Async().OnSuccess(value => 101),
                result.Async().OnError(error => { }),
                result.Async().OnError(error => 500),

                result.Async().Either(value => { }, error => { }),
                result.Async().Either(value => 101, error => { }),
                result.Async().Either(value => { }, error => 500),
                result.Async().Either(value => 101, error => 500),

                result.Async().Ensure(value => true, () => 404),
                result.Async().Match(value => true, error => false)
            };

            Task.WaitAll(tasks);
        }

        protected override void TestIEnumerableOfResult(Either<int, int>[] results)
        {
            results.Bind(value => Else<int>.Success()).Count();
            results.Bind(value => Either<int, int>.Success(101)).Count();
            results.Bind(value => Either<int, int>.Success(101), error => Either<int, int>.Error(500)).Count();
            results.BindOnError(error => If<int>.Error()).Count();
            results.BindOnError(error => Either<int, int>.Error(500)).Count();

            results.OnSuccess(value => { }).Count();
            results.OnSuccess(value => 101).Count();
            results.OnError(error => { }).Count();
            results.OnError(error => 500).Count();

            results.Either(value => { }, error => { }).Count();
            results.Either(value => 101, error => { }).Count();
            results.Either(value => { }, error => 500).Count();
            results.Either(value => 101, error => 500).Count();

            results.Ensure(value => true, () => 404).Count();
            results.Match(value => true, error => false).Count();
        }

        protected override void TestKeepMethodsOfResult(Either<int, int> result)
        {
            result.Keep(() => 100, out int num);
            result.KeepOnSuccess(value => 100, out int sCode);
            result.KeepOnError(error => 500, out int eCode);
            result.KeepEither(value => 100, error => 500, out int statusCode);
            result.KeepEither(value => 100, error => 500, out int successCode, out int errorCode);
        }

        protected override void TestLargeContent(Either<string, string> result)
        {
            result.Bind(value => Else<string>.Success());
            result.Bind(value => Either<string, string>.Success(value));
            result.Bind(value => Either<string, string>.Success(value), error => Either<string, string>.Error(error));
            result.BindOnError(error => If<string>.Error());
            result.BindOnError(error => Either<string, string>.Error(error));

            result.OnSuccess(value => { });
            result.OnSuccess(value => value);
            result.OnError(error => { });
            result.OnError(error => error);

            result.OnEither(value => { }, error => { });
            result.OnEither(value => value, error => { });
            result.OnEither(value => { }, error => error);
            result.OnEither(value => value, error => error);

            result.Ensure(value => true, () => LargeContent);
            result.Match(value => value, error => error);
        }
    }
}
