using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Railway.Test
{
    static class Settings
    {
        /// <summary>
        /// How often each Test should be called.
        /// </summary>
        public const int Iterations = 1000000;

        /// <summary>
        /// The number of Items for Tests of IEnumerables.
        /// </summary>
        public const int ItemCount = 1000;

        /// <summary>
        /// The number of instances for parallelization.
        /// </summary>
        public const int WorkerCount = 4;

        /// <summary>
        /// How the Tests should be parallelized.
        /// </summary>
        public const ExecutionScope Scope = ExecutionScope.MethodLevel;
    }
}
