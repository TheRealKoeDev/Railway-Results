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
    public class ResultWithValueOrErrorPerformanceTest
    {
        private readonly string LargeContent = new string('*', Settings.LargeContentSize);
        private readonly Result<int, int>[] Successes = new Result<int, int>[Settings.ItemCount];
        private readonly Result<int, int>[] Errors = new Result<int, int>[Settings.ItemCount];

        [TestInitialize]
        public void Initialize()
        {
            for (int i = 0; i < Settings.ItemCount; i++)
            {
                Successes[i] = Result<int, int>.Success(100);
                Errors[i] = Result<int, int>.Error(500);
            }
        }

        [TestMethod]
        public void TestSingleResult()
        {
            for (int i = 0; i < Settings.Iterations; i++)
            {
                TestSingleResult(Result<int, int>.Success(100));
                TestSingleResult(Result<int, int>.Error(500));
            }
        }

        private void TestSingleResult(Result<int, int> result)
        {
            result.Bind(value => ResultWithError<int>.Success());
            result.Bind(value => Result<int, int>.Success(101));
            result.Bind(value => Result<int, int>.Success(101), error => Result<int, int>.Error(500));
            result.BindOnError(error => Result<int>.Error());
            result.BindOnError(error => Result<int, int>.Error(500));

            result.OnSuccess(value => { });
            result.OnSuccess(value => 101);
            result.OnError(error => { });
            result.OnError(error => 500);

            result.Either(value => { }, error => { });
            result.Either(value => 101, error => { });
            result.Either(value => { }, error => 500);
            result.Either(value => 101, error => 500);

            result.Ensure(value => true, () => 404);
            result.Match(value => true, error => false);
        }

        [TestMethod]
        public void TestTaskOfResult()
        {
            for (int i = 0; i < Settings.Iterations; i++)
            {
                TestTaskOfResult(Result<int, int>.Success(100));
                TestTaskOfResult(Result<int,  int>.Error(500));
            }
        }

        private void TestTaskOfResult(Result<int, int> result)
        {
            Task[] tasks = new Task[]
            {
                result.Async().Bind(value => ResultWithError<int>.Success()),
                result.Async().Bind(value => Result<int, int>.Success(101)),
                result.Async().Bind(value => Result<int, int>.Success(101), error => Result<int, int>.Error(500)),
                result.Async().BindOnError(error => Result<int>.Error()),
                result.Async().BindOnError(error => Result<int, int>.Error(500)),

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

        [TestMethod]
        public void TestIEnumerableOfResult()
        {
            for (int i = 0; i < Settings.Iterations / Settings.ItemCount; i++)
            {
                TestIEnumerableOfResult(Successes);
                TestIEnumerableOfResult(Errors);
            }
        }

        private void TestIEnumerableOfResult(Result<int, int>[] results)
        {
            results.Bind(value => ResultWithError<int>.Success());
            results.Bind(value => Result<int, int>.Success(101));
            results.Bind(value => Result<int, int>.Success(101), error => Result<int, int>.Error(500));
            results.BindOnError(error => Result<int>.Error());
            results.BindOnError(error => Result<int, int>.Error(500));

            results.OnSuccess(value => { });
            results.OnSuccess(value => 101);
            results.OnError(error => { });
            results.OnError(error => 500);

            results.Either(value => { }, error => { });
            results.Either(value => 101, error => { });
            results.Either(value => { }, error => 500);
            results.Either(value => 101, error => 500);

            results.Ensure(value => true, () => 404);
            results.Match(value => true, error => false);
        }

        [TestMethod]
        public void TestKeepMethodsOfResult()
        {
            for (int i = 0; i < Settings.Iterations; i++)
            {
                TestKeepMethodsOfResult(Result<int, int>.Success(100));
                TestKeepMethodsOfResult(Result<int, int>.Error(500));
            }
        }

        private void TestKeepMethodsOfResult(Result<int, int> result)
        {
            result.Keep(() => 100, out int num);
            result.KeepOnSuccess(value => 100, out int sCode);
            result.KeepOnError(error => 500, out int eCode);
            result.KeepEither(value => 100, error => 500, out int statusCode);
            result.KeepEither(value => 100, error => 500, out int successCode, out int errorCode);
        }

        [TestMethod]
        public void TestLargeContent()
        {
            for (int i = 0; i < Settings.Iterations; i++)
            {
                TestLargeContent(Result<string, string>.Error(LargeContent));
                TestLargeContent(Result<string, string>.Success(LargeContent));
            }
        }

        private void TestLargeContent(Result<string, string> result)
        {
            result.Bind(value => ResultWithError<string>.Success());
            result.Bind(value => Result<string, string>.Success(value));
            result.Bind(value => Result<string, string>.Success(value), error => Result<string, string>.Error(error));
            result.BindOnError(error => Result<string>.Error());
            result.BindOnError(error => Result<string, string>.Error(error));

            result.OnSuccess(value => { });
            result.OnSuccess(value => value);
            result.OnError(error => { });
            result.OnError(error => error);

            result.Either(value => { }, error => { });
            result.Either(value => value, error => { });
            result.Either(value => { }, error => error);
            result.Either(value => value, error => error);

            result.Ensure(value => true, () => LargeContent);
            result.Match(value => value, error => error);
        }
    }
}
