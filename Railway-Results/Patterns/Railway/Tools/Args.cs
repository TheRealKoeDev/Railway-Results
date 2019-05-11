using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace KoeLib.Patterns.Railway.Tools
{
    [DebuggerStepThrough]
    internal static class Args
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ExceptionIfNull(object argument, string name)
        {
            _ = argument ?? throw new ArgumentNullException(name);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ExceptionIfNull(object argument1, string name1, object argument2, string name2)
        {
            _ = argument1 ?? throw new ArgumentNullException(name1);
            _ = argument2 ?? throw new ArgumentNullException(name2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ExceptionIfNull(object argument1, string name1, object argument2, string name2, object argument3, string name3)
        {
            _ = argument1 ?? throw new ArgumentNullException(name1);
            _ = argument2 ?? throw new ArgumentNullException(name2);
            _ = argument3 ?? throw new ArgumentNullException(name3);
        }
    }
}