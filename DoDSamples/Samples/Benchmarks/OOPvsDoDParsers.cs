using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Order;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using System.Text;

namespace DoDSamples.Samples
{

    [Orderer(SummaryOrderPolicy.SlowestToFastest, MethodOrderPolicy.Declared)]
    public class OOPvsDoDParsersToLower
    {
        [Params(100)]
        public int Samples = 1;

        private string[] lines = null;

        [GlobalSetup(Targets = new[]
        {
            nameof(ToLower),
            nameof(ToLowerASCIIInPlace_SIMD)
        })]

        public void GlobalSetup()
        {
            lines = File.ReadAllLines(@"C:\Users\Y700-17\Desktop\DataOrientedDesign\DoDSamples\DoDSamples\Samples\POParser\Sample.po");
        }

        [Benchmark]
        public void ToLowerASCIIInPlace_SIMD()
        {
            DoDPOParser_v5 parser = new DoDPOParser_v5();
            var dodLines_v5 = parser.Parse(lines, out var index);

            for (int s = 0; s < Samples; s++)
            {
                ToLowerASCIIInPlace_SIMD(dodLines_v5.AllNotes.ToString());
            }
        }

        [Benchmark]
        public void ToLower()
        {
            DoDPOParser_v5 parser = new DoDPOParser_v5();
            var dodLines_v5 = parser.Parse(lines, out var index);

            for (int s = 0; s < Samples; s++)
            {
                dodLines_v5.AllNotes.ToString().ToLower();
            }
        }

        public static unsafe void ToLowerASCIIInPlace_SIMD(string text)
        {
            const int upperIntOffset = 2097184;
            const int upperCharOffset = 32;

            Vector128<int> vresult = Vector128<int>.Zero;
            Vector128<int> add = Vector128.Create(upperIntOffset);

            var len = text.Length;

            fixed (char* pSource = text)
            {
                int i = 0;
                int lastBlockIndex = len - (len % 8);

                while (i < lastBlockIndex)
                {
                    int* c = (int*)(pSource + i);

                    vresult = Sse2.LoadVector128(c);
                    vresult = Sse2.Or(vresult, add);

                    Sse2.Store(c, vresult);

                    i += 8;
                }

                while (i < len)
                {
                    char* c = (char*)(pSource + i);
                    *c = (Char)(*c | upperCharOffset);

                    i += 1;
                }
            }
        }

    }


    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    [CategoriesColumn]
    [Orderer(SummaryOrderPolicy.SlowestToFastest, MethodOrderPolicy.Declared)]
    public class OOPvsDoDParsers
    {
        [Params(100)]
        public int Samples = 1;

        private string[] lines = null;
        private string text = null;

        [GlobalSetup(Targets = new[]
        {
            //nameof(DoD_Parser_ListOfStructs),
            nameof(DoD_Parser_ArrayOfStructs),
            //nameof(DoD_Parser_ListOfStructs_Split),
            nameof(DoD_Parser_ArrayOfStructs_Split),
            nameof(DoD_Parser_ArrayOfStructs_HotColdSplit),
            nameof(DoD_Parser_StructOfArrays),
            nameof(DoD_Parser_StructOfArrays_SIMD),
            nameof(OOP_Parser_StateMachine),

        })]

        public void GlobalSetup()
        {
            lines = File.ReadAllLines(@"C:\Users\Y700-17\Desktop\DataOrientedDesign\DoDSamples\DoDSamples\Samples\POParser\Sample.po");
        }

        [GlobalSetup(Targets = new[] { nameof(OOP_Parser_Regex) })]
        public void GlobalSetupA()
        {
            text = File.ReadAllText(@"C:\Users\Y700-17\Desktop\DataOrientedDesign\DoDSamples\DoDSamples\Samples\POParser\Sample.po");
        }

        [BenchmarkCategory("OOP")]
        [Benchmark(Baseline = true)]
        public void OOP_Parser_Regex()
        {
            POParser parser = new POParser();
            var lines_v0 = parser.Parse(text);

            for (int s = 0; s < Samples; s++)
            {
                double sum = 0;
                foreach (var line in lines_v0)
                {
                    sum += line.Amount;
                }
            }
        }

        [BenchmarkCategory("OOP")]
        [Benchmark]
        public void OOP_Parser_StateMachine()
        {
            POParser_Fast parser = new POParser_Fast();
            var lines_v0 = parser.Parse(lines);

            for (int s = 0; s < Samples; s++)
            {
                double sum = 0;
                foreach (var line in lines_v0)
                {
                    sum += line.Amount;
                }
            }
        }

        //[BenchmarkCategory("DoD")]
        //[Benchmark]
        //public void DoD_Parser_ListOfStructs()
        //{
        //    DoDPOParser_v1 parser = new DoDPOParser_v1();
        //    var lines_v1 = parser.Parse(lines);

        //    for (int s = 0; s < Samples; s++)
        //    {
        //        double sum = 0;

        //        foreach (var line in lines_v1)
        //        {
        //            sum += line.Amount;
        //        }
        //    }
        //}

        [BenchmarkCategory("DoD")]
        [Benchmark(Baseline = true)]
        public void DoD_Parser_ArrayOfStructs()
        {
            DoDPOParser_v1a parser = new DoDPOParser_v1a();
            var lines_v1 = parser.Parse(lines, out int index);

            for (int s = 0; s < Samples; s++)
            {
                double sum = 0;

                foreach (var line in lines_v1)
                {
                    sum += line.Amount;
                }
            }
        }

        //[BenchmarkCategory("DoD")]
        //[Benchmark]
        //public void DoD_Parser_ListOfStructs_Split()
        //{
        //    DoDPOParser_v2 parser = new DoDPOParser_v2();
        //    var lines_v2 = parser.Parse(lines);

        //    for (int s = 0; s < Samples; s++)
        //    {
        //        double sum = 0;

        //        foreach (var line in lines_v2)
        //        {
        //            sum += line.Amount;
        //        }
        //    }
        //}

        [BenchmarkCategory("DoD")]
        [Benchmark]
        public void DoD_Parser_ArrayOfStructs_Split()
        {
            DoDPOParser_v2a parser = new DoDPOParser_v2a();
            var lines_v2 = parser.Parse(lines, out int index);

            for (int s = 0; s < Samples; s++)
            {
                double sum = 0;

                foreach (var line in lines_v2)
                {
                    sum += line.Amount;
                }
            }
        }

        [BenchmarkCategory("DoD")]
        [Benchmark]
        public void DoD_Parser_ArrayOfStructs_HotColdSplit()
        {
            DoDPOParser_v3 parser = new DoDPOParser_v3();
            var lines_v3 = parser.Parse(lines);

            for (int s = 0; s < Samples; s++)
            {
                double sum = 0;

                for (int i = 0; i < lines_v3.Count; i++)
                {
                    sum += lines_v3[i].Amount;
                }
            }
        }

        [BenchmarkCategory("DoD")]
        [Benchmark]
        public void DoD_Parser_StructOfArrays()
        {
            DoDPOParser_v4 parser = new DoDPOParser_v4();
            var dodLines_v4 = parser.Parse(lines, out var index);

            for (int s = 0; s < Samples; s++)
            {
                double sum = 0;
                for (int i = 0; i < index; i++)
                {
                    sum += dodLines_v4.Amount[i];
                }
            }
        }

        [BenchmarkCategory("DoD")]
        [Benchmark]
        public void DoD_Parser_StructOfArrays_SIMD()
        {
            DoDPOParser_v4 parser = new DoDPOParser_v4();
            var dodLines_v4 = parser.Parse(lines, out var index);

            for (int s = 0; s < Samples; s++)
            {
                double sum = SumAmount(dodLines_v4, index);
            }
        }

     
        public unsafe double SumAmount(DoDPOLines_v4 source, int len)
        {
            double result;

            fixed (double* pSource = source.Amount)
            {
                Vector128<double> vresult = Vector128<double>.Zero;

                int i = 0;
                int lastBlockIndex = len - (len % 2);

                while (i < lastBlockIndex)
                {
                    vresult = Sse2.Add(vresult, Sse2.LoadVector128(pSource + i));
                    i += 2;
                }

                vresult = Ssse3.HorizontalAdd(vresult, vresult);
                result = vresult.ToScalar();

                while (i < len)
                {
                    result += pSource[i];
                    i += 1;
                }
            }

            return result;
        }
    }
}
