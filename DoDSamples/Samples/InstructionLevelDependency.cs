using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace DoDSamples
{
    #region desc
    //
    // Instruction Level Dependency:
    // Good compilers will do it's best to break dependency chains
    // but sometimes it's just impossible and it's up to the user to break non trival chains.
    //
    #endregion

    public class InstructionLevelDependency
    {
        public static int steps = 128 * 1024 * 1024;
        public static readonly int i32Bits = sizeof(int) * 8 - 1;

        public static int TestDependency()
        {
            int a = 0;

            for (int i = 0; i < steps; i++)
            {
                a++;
                a++;
                a++;
                a++;
            }

            return a;
        }

        public static int TestIndependency()
        {
            int a = 0, b = 0, c = 0, d = 0;

            for (int i = 0; i < steps; i++)
            {
                a++;
                b++;
                c++;
                d++;
            }

            return a + b + c + d;
        }

        public static void TestSum()
        {
            double[] x = new double[steps];
            double sum = 0;

            for (int i = 0; i < steps; i++)
            {
                sum += x[i];
            }
        }

        public static void TestDependencyUnrolledSum()
        {
            double[] x = new double[steps];
            double sum = 0;

            int i = 0;
            for (; i < steps; i += 4)
            {
                sum += x[i];
                sum += x[i + 1];
                sum += x[i + 2];
                sum += x[i + 3];
            }
            // Sum residuals
            for (; i < steps; i++)
            {
                sum += x[i];
            }
        }

        public static void TestIndependencyUnrolledSum()
        {
            int steps = 64 * 1024 * 1024;

            double[] x = new double[steps];
            double sum = 0;
            double sa = 0, sb = 0, sc = 0, sd = 0;

            int i = 0;
            for (; i < steps; i += 4)
            {
                sa += x[i];
                sb += x[i + 1];
                sc += x[i + 2];
                sd += x[i + 3];
            }

            // Sum residuals
            for (; i < steps; i++)
            {
                sum += x[i];
            }

            sum = sa + sb + sc + sd;
        }

        public static void TestIndependencyUnrolledSum2()
        {
            int steps = 64 * 1024 * 1024;

            double[] x = new double[steps];
            double sum = 0;
            double sa = 0, sb = 0, sc = 0, sd = 0;

            int i = 0;
            for (; i < steps; i += 4)
            {
                sa += x[i];
                sb += x[i + 1];
                sc += x[i + 2];
                sd += x[i + 3];
            }

            // Sum residuals
            for (; i < steps; i++)
            {
                sum += x[i];
            }

            sum = (sa + sb) + (sc + sd);
        }

        public static void TestTrueDependence(int[] x)
        {
            var min = 0;

            for (int i = 0; i < x.Length; i++)
            {
                min = Math.Min(min, x[i]);
                //min = MinBranch(min, x[i]);
            }
        }

        public static void TestTrueIndependence(int[] x)
        {
            var min = 0;
            var m1 = 0;
            var m2 = 0;
            var m3 = 0;
            var m4 = 0;

            for (int i = 0; i < x.Length; i += 4)
            {
                m1 = MinBranchFree(m1, x[i]);
                m2 = MinBranchFree(m2, x[i + 1]);
                m3 = MinBranchFree(m3, x[i + 2]);
                m4 = MinBranchFree(m4, x[i + 3]);
            }

            min = MinBranchFree(m1, MinBranchFree(m2, MinBranchFree(m3, m4)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int MinBranchFree(int x, int y)
        {
            return y + ((x - y) & ((x - y) >> (31)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int MinBranch(int x, int y)
        {
            return (x <= y) ? x : y;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint IsMinBranchFree(int x, int y)
        {
            var v = x - y;
            return 1 - 
                (1 ^ ((uint)v >> (31)));
        }
    }
}
