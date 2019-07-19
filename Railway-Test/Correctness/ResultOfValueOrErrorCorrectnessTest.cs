using System;
using KoeLib.Patterns.Railway.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Railway.Test.Correctness
{
    public class Test
    {

    }

    public class Test2 : Test
    {
    }

    [TestClass]
    public class ResultOfValueOrErrorCorrectnessTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var ok = Result<Test, string>.Error("10").CastContent<string, int>();
            int resultSize = System.Runtime.InteropServices.Marshal.SizeOf(default(Result));
            int resultOfValueSize = System.Runtime.InteropServices.Marshal.SizeOf(default(Result<double>));
            int resultOfErrorSize = System.Runtime.InteropServices.Marshal.SizeOf(default(ResultOrError<double>));
            int resultOfValueOrErrorSize = System.Runtime.InteropServices.Marshal.SizeOf(default(Result<double, double>));
            // Result<int, string> test = "test";
        }
    }
}
