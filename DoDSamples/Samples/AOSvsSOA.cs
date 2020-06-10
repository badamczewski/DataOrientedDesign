using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace DoDSamples
{
    public class AOSvsSOA
    {
        //
        // This is a Data Oriented Design test that will show how
        // Struct of arrays has a much better Data Access characteristics and data locality.
        //
        // SOA [V0,V0,V0,V1,V1,V1]
        // AOS [V0,V1,V0,V1,V0,V1]
        //
        // For AOS for Structs or Classes we will have an 80 pad between which means that we will update each cache line
        // to increment the v0 value:
        // <0x281AF76B740>	80	80
        // <0x281AF76B790>	80	80
        // <0x281AF76B7E0>	80	80
        // 
        // For SOA we will update each array value and reuse the same cache line:
        // Int32[]    <0x216493DB6B8>	32 792	32 792
        // Int32[]    <0x216493E36E8>	32 792	32 792
        // Int32[]    <0x216493EB718>	32 792	32 792
        // 
        // SOA is better in general for partial object updates while AOS is good when we want to contantly update entire 
        // objects wich is almost never :)
        //
        public void TestAOS()
        {
            int steps = 1024 * 1024;
            int soaSize = 1024 * 8;

            SimpleStruct[] arrayOfStructs = new SimpleStruct[soaSize];

            for (int i = 0; i < arrayOfStructs.Length; i++)
                arrayOfStructs[i] = new SimpleStruct();

            for (int s = 0; s < steps; s++)
            {
                for (int i = 0; i < soaSize; i++)
                {
                    arrayOfStructs[i].v0++;
                }
            }
        }

        public void TestSOA()
        {
            int steps = 1024 * 1024;
            int soaSize = 1024 * 8;

            SOAStruct soa = new SOAStruct(soaSize);

            for (int s = 0; s < steps; s++)
            {
                for (int i = 0; i < soaSize; i++)
                {
                    soa.v0[i]++;
                }
            }
        }
    }

    class SOAStruct
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
