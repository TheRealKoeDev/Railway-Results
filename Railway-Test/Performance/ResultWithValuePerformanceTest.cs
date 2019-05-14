using KoeLib.Patterns.Railway.Linq;
using KoeLib.Patterns.Railway.Results;
using KoeLib.Patterns.Railway.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Railway.Test.Performance
{
    [TestClass]
    [DoNotParallelize]
    [TestCategory("Performance")]
    public class ResultWithValuePerformanceTest
    {
        private readonly string LargeContent = new string('*', Settings.LargeContentSize);
        private readonly Result<int>[] Successes = new Result<int>[Settings.ItemCount];
        private readonly Result<int>[] Errors = new Result<int>[Settings.ItemCount];

        [TestInitialize]
        public void Initialize()
        {
            for (int i = 0; i < Settings.ItemCount; i++)
            {
                Successes[i] = Result<int>.Success(100);
                Errors[i] = Result<int>.Error();
            }
        }

        [TestMethod]
        public void TestSingleResult()
        {
            for (int i = 0; i < Settings.Iterations; i++)
            {
                TestSingleResult(Result<int>.Success(100));
                TestSingleResult(Result<int>.Error());
            }
        }

        private void TestSingleResult(Result<int> result)
        {
            result.Bind(value => Result.Success());
            result.Bind(value => Result<int>.Success(101));
            result.Bind(value => Result<int, int>.Success(101), () => Result<int, int>.Error(500));
            result.BindOnError(() => Result<int>.Error());
            result.BindOnError(() => Result<int, int>.Error(500));

            result.OnSuccess(value => { });
            result.OnSuccess(value => 101);
            result.OnError(() => { });
            result.OnError(() => 500);

            result.Either(value => { }, () => { });
            result.Either(value => 101, () => { });
            result.Either(value => { }, () => 500);
            result.Either(value => 101, () => 500);

            result.Ensure(value => true);
            result.Match(value => true, () => false);
        }

        [TestMethod]
        public void TestTaskOfResult()
        {
            for (int i = 0; i < Settings.Iterations; i++)
            {
                TestTaskOfResult(Result<int>.Success(100));
                TestTaskOfResult(Result<int>.Error());
            }
        }

        private void TestTaskOfResult(Result<int> result)
        {
            Task[] tasks = new Task[]
            {
                result.Async().Bind(value => Result.Success()),
                result.Async().Bind(value => Result<int>.Success(101)),
                result.Async().Bind(value => Result<int, int>.Success(101), () => Result<int, int>.Error(500)),
                result.Async().BindOnError(() => Result<int>.Error()),
                result.Async().BindOnError(() => Result<int, int>.Error(500)),

                result.Async().OnSuccess(value => { }),
                result.Async().OnSuccess(value => 101),
                result.Async().OnError(() => { }),
                result.Async().OnError(() => 500),

                result.Async().Either(value => { }, () => { }),
                result.Async().Either(value => 101, () => { }),
                result.Async().Either(value => { }, () => 500),
                result.Async().Either(value => 101, () => 500),

                result.Async().Ensure(value => true),
                result.Async().Match(value => true, () => false),
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

        private void TestIEnumerableOfResult(Result<int>[] results)
        {
            results.Bind(value => Result.Success()).Count();
            results.Bind(value => Result<int>.Success(101)).Count();
            results.Bind(value => Result<int, int>.Success(101), () => Result<int, int>.Error(500)).Count();
            results.BindOnError(() => Result<int>.Error()).Count();
            results.BindOnError(() => Result<int, int>.Error(500)).Count();

            results.OnSuccess(value => { }).Count();
            results.OnSuccess(value => 101).Count();
            results.OnError(() => { }).Count();
            results.OnError(() => 500).Count();

            results.Either(value => { }, () => { }).Count();
            results.Either(value => 101, () => { }).Count();
            results.Either(value => { }, () => 500).Count();
            results.Either(value => 101, () => 500).Count();

            results.Ensure(value => true).Count();
            results.Match(value => true, () => false).Count();
        }

        [TestMethod]
        public void TestKeepMethodsOfResult()
        {
            for (int i = 0; i < Settings.Iterations; i++)
            {
                TestKeepMethodsOfResult(Result<int>.Success(100));
                TestKeepMethodsOfResult(Result<int>.Error());
            }
        }

        private void TestKeepMethodsOfResult(Result<int> result)
        {
            result.Keep(() => 100, out int num);
            result.KeepOnSuccess(value => 100, out int sCode);
            result.KeepOnError(() => 500, out int eCode);
            result.KeepEither(value => 100, () => 500, out int statusCode);
            result.KeepEither(value => 100, () => 500, out int successCode, out int errorCode);
        }
       
        [TestMethod]
        public void TestLargeContent()
        {            
            for (int i = 0; i < Settings.Iterations; i++)
            {
                TestLargeContent(Result<string>.Error());
                TestLargeContent(Result<string>.Success(LargeContent));                
            }
        }

        private void TestLargeContent(Result<string> result)
        {
            result.Bind(value => Result.Success());
            result.Bind(value => Result<string>.Success(value));
            result.Bind(value => Result<string, string>.Success(value), () => Result<string, string>.Error(LargeContent));
            result.BindOnError(() => Result<string>.Error());
            result.BindOnError(() => Result<string, string>.Error(LargeContent));

            result.OnSuccess(value => { });
            result.OnSuccess(value => value);
            result.OnError(() => { });
            result.OnError(() => LargeContent);

            result.Either(value => { }, () => { });
            result.Either(value => value, () => { });
            result.Either(value => { }, () => LargeContent);
            result.Either(value => value, () => LargeContent);

            result.Ensure(value => true);
            result.Match(value => LargeContent, () => LargeContent);
        }
    }
}
