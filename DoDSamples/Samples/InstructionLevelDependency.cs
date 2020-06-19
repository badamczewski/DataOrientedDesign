using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
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

        public static void TestTrueIndependenceAVX(int[] x)
        {
            var min = 0;
            var m1 = 0;
            var m2 = 0;
            var m3 = 0;
            var m4 = 0;

            for (int i = 0; i < x.Length; i += 4)
            {
                m1 = AVXMin(m1, x[i]);
                m2 = AVXMin(m2, x[i + 1]);
                m3 = AVXMin(m3, x[i + 2]);
                m4 = AVXMin(m4, x[i + 3]);
            }

            min = AVXMin(m1, AVXMin(m2, AVXMin(m3, m4)));
        }

        public static int TestTrueIndependenceAVXVec(int[] x)
        {
            return AVXVecMin(x);
        }
        
        public static int TestTrueIndependenceAVXVecIndependant(int[] x)
        {
            return AVXVecMinIndependent(x);
        }

        public static void TestTrueDependenceBranchFree(int[] x)
        {
            int min = int.MaxValue, inc = 0;
            for (int i = 0; i < x.Length; i++)
            {
                inc = IsMinBranchFree(min, x[i]);
                var delta = min - x[i];
                min = min - (delta * inc);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IsMinBranchFree(int x, int y)
        {
            var v = x - y;
            return (int)((1 ^ ((uint)v >> (31))));
        }


        public static void TestTrueIndependenceDelta(int[] x)
        {
            var max = int.MaxValue;
            int[] m = new int[4] { max, max, max, max };
            var inc = 0;
            for (int i = 0; i < x.Length; i += 4)
            {
                for (int w = 0; w < m.Length; w++)
                {
                    inc = IsMinBranchFree(m[w], x[i + w]);
                    var delta = m[w] - x[i + w];
                    m[w] = m[w] - (delta * inc);
                }
            }
            // todo: residuals + min(m[0],m[1],m[2],m[3])
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int MinBranchFree(int x, int y)
        {
            return y + ((x - y) & ((x - y) >> (31)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int AVXMin(int x, int y)
        {
            var v1 = Vector128.CreateScalarUnsafe(x);
            var v2 = Vector128.CreateScalarUnsafe(y);
            return Avx.Min(v1, v2).ToScalar();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int AVXVecMin(int[] x)
        {
            int len = x.Length;
            var min = Vector128.Create(int.MaxValue);
            fixed (int* pSource = x)
            {
                int i = 0;
                int lastBlockIndex = len - (len % 4);

                while (i < lastBlockIndex) {
                    min = Avx.Min(min, Avx.LoadVector128(pSource + i));
                    i += 4;
                }
                var minValue = min.ToScalar();
                while (i < len) {
                    minValue = MinBranchFree(minValue, pSource[i]);
                    i += 1;
                }
                return minValue;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int AVXVecMinIndependent(int[] x)
        {
            int len = x.Length;
            var min1 = Vector128.Create(int.MaxValue);
            var min2 = Vector128.Create(int.MaxValue);

            fixed (int* pSource = x)
            {
                int i = 0;
                int lastBlockIndex = len - (len % 8);

                while (i < lastBlockIndex)
                {
                    min1 = Avx.Min(min1, Avx.LoadVector128(pSource + i));
                    min2 = Avx.Min(min2, Avx.LoadVector128(pSource + i + 4));

                    i += 8;
                }
                var minValue = min1.ToScalar() + min2.ToScalar();
                while (i < len)
                {
                    minValue = MinBranchFree(minValue, pSource[i]);
                    i += 1;
                }
                return minValue;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int MinBranch(int x, int y)
        {
            return (x <= y) ? x : y;
        }
    }
}

