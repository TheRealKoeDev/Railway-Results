using System;
using System.Linq;
using System.Threading.Tasks;
using KoeLib.Patterns.Railway.Tasks;
using KoeLib.Patterns.Railway.Linq;
using KoeLib.Patterns.Railway.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Railway.Test
{
    [TestClass]
    [TestCategory("Performance")]
    public class ResultPerformanceTest
    {
        private readonly Result[] Successes = new Result[Settings.ItemCount];
        private readonly Result[] Errors = new Result[Settings.ItemCount];

        [TestInitialize]
        public void Initialize()
        {
            for (int i = 0; i < Settings.ItemCount; i++)
            {
                Successes[i] = Result.Success();
                Errors[i] = Result.Error();
            }
        }

        [TestMethod]        
        public void TestResult()
        {            
            for (int i = 0; i < Settings.Iterations; i++)
            {
                TestResult(Result.Success());
                TestResult(Result.Error());
            }
        }

        private void TestResult(Result result)
        {
            result.Bind(() => Result.Success());
            result.Bind(() => Result<int>.Success(1));
            result.Bind(() => Result<int>.Success(1), () => Result<int>.Error());
            result.BindOnError(() => Result.Error());
            result.BindOnError(() => ResultWithError<int>.Error(1));

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

        [TestMethod]
        public void TestTaskOfResult()
        {
            for (int i = 0; i < Settings.Iterations; i++)
            {
                TestTaskOfResult(Result.Success());
                TestTaskOfResult(Result.Error());
            }
        }

        private void TestTaskOfResult(Result result)
        {
            Task[] tasks = new Task[]
            {
                Task.FromResult(result).Bind(() => Result.Success()),
                Task.FromResult(result).Bind(() => Result<int>.Success(1)),
                Task.FromResult(result).Bind(() => Result<int>.Success(1), () => Result<int>.Error()),
                Task.FromResult(result).BindOnError(() => Result.Error()),
                Task.FromResult(result).BindOnError(() => ResultWithError<int>.Error(1)),

                Task.FromResult(result).OnSuccess(() => { }),
                Task.FromResult(result).OnSuccess(() => 1),
                Task.FromResult(result).OnError(() => { }),
                Task.FromResult(result).OnError(() => 1),

                Task.FromResult(result).Either(() => { }, () => { }),
                Task.FromResult(result).Either(() => 1, () => { }),
                Task.FromResult(result).Either(() => { }, () => 1),
                Task.FromResult(result).Either(() => 1, () => 1),

                Task.FromResult(result).Ensure(() => true),
                Task.FromResult(result).Match(() => true, () => false),
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

        private void TestIEnumerableOfResult(Result[] results)
        {
            results.Bind(() => Result.Success()).Count();
            results.Bind(() => Result<int>.Success(1)).Count();
            results.Bind(() => Result<int>.Success(1), () => Result<int>.Error()).Count();
            results.BindOnError(() => Result.Error()).Count();
            results.BindOnError(() => ResultWithError<int>.Error(1)).Count();

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

        [TestMethod]
        public void TestExtensionMethodsOfResult()
        {
            for (int i = 0; i < Settings.Iterations; i++)
            {
                TestExtensionMethodsOfResult(Result.Success());
                TestExtensionMethodsOfResult(Result.Error());
            }
        }

        private void TestExtensionMethodsOfResult(Result result)
        {
            result.Do(() => { });
            result.Async();
            result.Keep(() => 100, out int num);
        }

        [TestMethod]
        public void TestKeepMethodsOfResult()
        {
            for (int i = 0; i < Settings.Iterations; i++)
            {
                TestKeepMethodsOfResult(Result.Success());
                TestKeepMethodsOfResult(Result.Error());
            }
        }

        private void TestKeepMethodsOfResult(Result result)
        {
            result.Keep(() => 100, out int num);
            result.KeepOnSuccess(() => 100, out int sCode);
            result.KeepOnError(() => 500, out int eCode);
            result.KeepEither(() => 100, () => 500, out int statusCode);
            result.KeepEither(() => 100, () => 500, out int successCode, out int errorCode);
        }
    }
}
