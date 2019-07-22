using System;
using KoeLib.Patterns.Railway.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Railway.Test.Correctness
{
    public class Test
    {
        public void Te(Func<Test, int> lol)
        {
            lol(this);
        }

        public void Te(Func<Test2, int> lol2)
        {
            lol2(new Test2());
        }
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
            Test t = new Test();
            t.Te(lol2: _ => 0);
            int resultSize = System.Runtime.InteropServices.Marshal.SizeOf(default(Result));
            int resultOfValueSize = System.Runtime.InteropServices.Marshal.SizeOf(default(Result<double>));
            int resultOfErrorSize = System.Runtime.InteropServices.Marshal.SizeOf(default(ResultOrError<double>));
            int resultOfValueOrErrorSize = System.Runtime.InteropServices.Marshal.SizeOf(default(Result<double, double>));
            // Result<int, string> test = "test";
        }
    }
}
