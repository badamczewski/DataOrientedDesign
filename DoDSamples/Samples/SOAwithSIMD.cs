using System;
using System.Collections.Generic;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using System.Text;

namespace DoDSamples.Samples
{
    public class SOAwithSIMD
    {
        public int[] GetArray()
        {
            var array = new int[128 * 1024 * 1024];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = i;
            }

            return array;
        }

        public int Sum(ReadOnlySpan<int> source)
        {
            int result = 0;
            for (int i = 0; i < source.Length; i++)
            {
                result += source[i];
            }

            return result;
        }

        public unsafe int SumVectorizedSse2(SOAStruct source)
        {
            int result;

            fixed (int* pSource = source.v0)
            {
                Vector128<int> vresult = Vector128<int>.Zero;

                int i = 0;
                int lastBlockIndex = source.v0.Length - (source.v0.Length % 4);

                while (i < lastBlockIndex)
                {
                    vresult = Sse2.Add(vresult, Sse2.LoadVector128(pSource + i));
                    i += 4;
                }

                if (Ssse3.IsSupported)
                {
                    vresult = Ssse3.HorizontalAdd(vresult, vresult);
                    vresult = Ssse3.HorizontalAdd(vresult, vresult);
                }
                else
                {
                    vresult = Sse2.Add(vresult, Sse2.Shuffle(vresult, 0x4E));
                    vresult = Sse2.Add(vresult, Sse2.Shuffle(vresult, 0xB1));
                }
                result = vresult.ToScalar();

                while (i < source.v0.Length)
                {
                    result += pSource[i];
                    i += 1;
                }
            }

            return result;
        }
    }


    public struct SOAStruct
    {
        public int[] v0;
        public int[] v1;
        public int[] v2;
        public int[] v3;
        public int[] v4;
        public int[] v5;
        public int[] v6;
        public int[] v7;
        public int[] v8;
        public int[] v9;
        public int[] v10;
        public int[] v11;
        public int[] v12;
        public int[] v13;
        public int[] v14;
        public int[] v15;

        public SOAStruct(int n)
        {
            v0 = new int[n];
            v1 = new int[n];
            v2 = new int[n];
            v3 = new int[n];
            v4 = new int[n];
            v5 = new int[n];
            v6 = new int[n];
            v7 = new int[n];
            v8 = new int[n];
            v9 = new int[n];
            v10 = new int[n];
            v11 = new int[n];
            v12 = new int[n];
            v13 = new int[n];
            v14 = new int[n];
            v15 = new int[n];
        }

    }

    class SimpleStruct
    {
        public int v0;
        public int v1;
        public int v2;
        public int v3;
        public int v4;
        public int v5;
        public int v6;
        public int v7;
        public int v8;
        public int v9;
        public int v10;
        public int v11;
        public int v12;
        public int v13;
        public int v14;
        public int v15;
    }
}
