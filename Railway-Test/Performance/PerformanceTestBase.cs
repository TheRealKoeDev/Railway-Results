using KoeLib.Patterns.Railway.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Railway.Test.Performance
{
    public abstract class PerformanceTestBase<TResult, TLargeContentResult>
        where TResult: IResult
        where TLargeContentResult: IResult
    {
        protected readonly string LargeContent = new string('*', Settings.LargeContentSize);
        protected readonly TResult[] Successes = new TResult[Settings.ItemCount];
        protected readonly TResult[] Errors = new TResult[Settings.ItemCount];

        protected abstract TResult Success { get; }
        protected abstract TResult Error { get; }

        protected abstract TLargeContentResult LargeContentSuccess { get; }
        protected abstract TLargeContentResult LargeContentError { get; }


        protected abstract void TestSingleResult(TResult result);
        protected abstract void TestTaskOfResult(TResult result);
        protected abstract void TestIEnumerableOfResult(TResult[] results);
        protected abstract void TestKeepMethodsOfResult(TResult result);
        protected abstract void TestLargeContent(TLargeContentResult result);


        [TestInitialize]
        public void Initialize()
        {
            for (int i = 0; i < Settings.ItemCount; i++)
            {
                Successes[i] = Success;
                Errors[i] = Error;
            }
        }

        [TestMethod]
        public void TestSingleResult()
        {
            for (int i = 0; i < Settings.Iterations; i++)
            {
                TestSingleResult(Success);
                TestSingleResult(Error);
            }
        }

        [TestMethod]
        public void TestTaskOfResult()
        {
            for (int i = 0; i < Settings.Iterations; i++)
            {
                TestTaskOfResult(Success);
                TestTaskOfResult(Error);
            }
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

        [TestMethod]
        public void TestKeepMethodsOfResult()
        {
            for (int i = 0; i < Settings.Iterations; i++)
            {
                TestKeepMethodsOfResult(Success);
                TestKeepMethodsOfResult(Error);
            }
        }

        [TestMethod]
        public void TestLargeContent()
        {
            for (int i = 0; i < Settings.Iterations; i++)
            {
                TestLargeContent(LargeContentSuccess);
                TestLargeContent(LargeContentError);
            }
        }
    }
}
