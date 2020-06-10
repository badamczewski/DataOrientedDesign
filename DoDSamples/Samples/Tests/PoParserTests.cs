using System;
using System.Collections.Generic;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using System.Text;

namespace DoDSamples.Samples
{
    public class POParserTests
    {
        private static int samples = 100_000;
        public static void TestAmountPO_Regex()
        {
            var parser = new POParser();
            var lines = parser.ParseFromPath(@"C:\Users\Y700-17\Desktop\DataOrientedDesign\DoDSamples\DoDSamples\Samples\POParser\Sample.po");

            Utils.Measure(() =>
            {
                for (int s = 0; s < samples; s++)
                {
                    double sum = 0;

                    foreach (var line in lines)
                    {
                        sum += line.Amount;
                    }
                }
            });
        }

        public static void TestAmountPO_OOP_StateMachine()
        {
            var parser = new POParser_Fast();
            var lines = parser.Parse(@"C:\Users\Y700-17\Desktop\DataOrientedDesign\DoDSamples\DoDSamples\Samples\POParser\Sample.po");

            Utils.Measure(() =>
            {
                for (int s = 0; s < samples; s++)
                {
                    double sum = 0;

                    foreach (var line in lines)
                    {
                        sum += line.Amount;
                    }
                }
            });
        }

        public static void TestAmountPO_DoD_StateMachine()
        {
            var parser = new DoDPOParser_v1();
            var lines = parser.Parse(@"C:\Users\Y700-17\Desktop\DataOrientedDesign\DoDSamples\DoDSamples\Samples\POParser\Sample.po");

            Utils.Measure(() =>
            {
                for (int s = 0; s < samples; s++)
                {
                    double sum = 0;

                    foreach (var line in lines)
                    {
                        sum += line.Amount;
                    }
                }
            });
        }

        public static void TestAmountPO_DoD_StateMachineFix()
        {
            var parser = new DoDPOParser_v1a();
            var lines = parser.Parse(@"C:\Users\Y700-17\Desktop\DataOrientedDesign\DoDSamples\DoDSamples\Samples\POParser\Sample.po", out var index);

            Utils.Measure(() =>
            {
                for (int s = 0; s < samples; s++)
                {
                    double sum = 0;

                    for (int i = 0; i < index; i++)
                    {
                        sum += lines[i].Amount;
                    }
                }
            });
        }

        public static void TestAmountPO_DoD_StateMachine_Split()
        {
            var parser = new DoDPOParser_v2();
            var lines = parser.Parse(@"C:\Users\Y700-17\Desktop\DataOrientedDesign\DoDSamples\DoDSamples\Samples\POParser\Sample.po");

            Utils.Measure(() =>
            {
                for (int s = 0; s < samples; s++)
                {
                    double sum = 0;

                    foreach (var line in lines)
                    {
                        sum += line.Amount;
                    }
                }
            });
        }

        public static void TestAmountPO_DoD_SateMachine_SplitFix()
        {
            var parser = new DoDPOParser_v2a();
            var lines = parser.Parse(@"C:\Users\Y700-17\Desktop\DataOrientedDesign\DoDSamples\DoDSamples\Samples\POParser\Sample.po", out var index);

            Utils.Measure(() =>
            {
                for (int s = 0; s < samples; s++)
                {
                    double sum = 0;

                    for (int i = 0; i < index; i++)
                    {
                        sum += lines[i].Amount;
                    }
                }
            });
        }


        public static void TestAmountPO_DoD_SateMachine_HotColdSplit()
        {
            var parser = new DoDPOParser_v3();
            var lines = parser.Parse(@"C:\Users\Y700-17\Desktop\DataOrientedDesign\DoDSamples\DoDSamples\Samples\POParser\Sample.po");

            Utils.Measure(() =>
            {
                for (int s = 0; s < samples; s++)
                {
                    double sum = 0;

                    foreach (var line in lines)
                    {
                        sum += line.Amount;
                    }
                }
            });
        }

        public static void TestAmountPO_DoD_SateMachine_HotColdSplitFix()
        {
            var parser = new DoDPOParser_v3a();
            var lines = parser.Parse(@"C:\Users\Y700-17\Desktop\DataOrientedDesign\DoDSamples\DoDSamples\Samples\POParser\Sample.po", out var index);

            Utils.Measure(() =>
            {
                for (int s = 0; s < samples; s++)
                {
                    double sum = 0;

                    for (int i = 0; i < index; i++)
                    {
                        sum += lines[i].Amount;
                    }
                }
            });
        }

        public static void TestAmountPO_DoD_SoA()
        {
            var parser = new DoDPOParser_v4();
            var lines = parser.Parse(@"C:\Users\Y700-17\Desktop\DataOrientedDesign\DoDSamples\DoDSamples\Samples\POParser\Sample.po", out var index);

            Utils.Measure(() =>
            {
                for (int s = 0; s < samples; s++)
                {
                    double sum = 0;

                    for (int i = 0; i < index; i++)
                    {
                        sum += lines.Amount[i];
                    }
                }
            });
        }

        public static void TestAmountPO_DoD_SoA_SIMD()
        {
            var parser = new DoDPOParser_v4();
            var lines = parser.Parse(@"C:\Users\Y700-17\Desktop\DataOrientedDesign\DoDSamples\DoDSamples\Samples\POParser\Sample.po", out var index);

            Utils.Measure(() =>
            {
                for (int s = 0; s < samples; s++)
                {
                    double sum = 0;
                    sum = parser.SumAmount(lines, index);
                }
            });
        }

        public static void TestNotesPO_OOP_Split()
        {
            samples = 10000;

            POParser parser = new POParser();
            var lines = parser.ParseFromPath(@"C:\Users\Y700-17\Desktop\DataOrientedDesign\DoDSamples\DoDSamples\Samples\POParser\Sample.po");

            Utils.Measure(() =>
            {
                for (int s = 0; s < samples; s++)
                {
                    List<string> words = new List<string>(1000);

                    foreach (var line in lines)
                    {
                        words.AddRange(line.Note.Split(" "));
                    }
                }
            });
        }

        public static void TestNotesPO_DoD_Split()
        {
            samples = 10000;

            var parser = new DoDPOParser_v5();
            var lines = parser.Parse(@"C:\Users\Y700-17\Desktop\DataOrientedDesign\DoDSamples\DoDSamples\Samples\POParser\Sample.po", out var index);

            Utils.Measure(() =>
            {
                for (int s = 0; s < samples; s++)
                {
                    List<string> words = new List<string>(1000);
                    words.AddRange(lines.AllNotes.ToString().Split(" "));
                }
            });
        }

        public static void TestNotesToLowerPO_OOP()
        {
            samples = 10000;

            var parser = new DoDPOParser_v5();
            var lines = parser.Parse(@"C:\Users\Y700-17\Desktop\DataOrientedDesign\DoDSamples\DoDSamples\Samples\POParser\Sample.po", out var index);

            Utils.Measure(() =>
            {
                for (int s = 0; s < samples; s++)
                {
                    var all = lines.AllNotes.ToString().ToLower();
                }
            });
        }

        public static void TestNotesToLowerPO_DoD()
        {
            samples = 10000;

            var parser = new DoDPOParser_v5();
            var lines = parser.Parse(@"C:\Users\Y700-17\Desktop\DataOrientedDesign\DoDSamples\DoDSamples\Samples\POParser\Sample.po", out var index);

            Utils.Measure(() =>
            {
                for (int s = 0; s < samples; s++)
                {
                    ToLowerASCIIInPlace_SIMD(lines.AllNotes.ToString());
                }
            });
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

        public static unsafe void ToUpperASCIIInPlace_SIMD(string text)
        {
            //0b_01111111_11011111_11111111_11011111;
            const int upperIntOffset = 2145386463;
            const int upperCharOffset = 95;

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
                    //0b_01111111_11011111_11111111_11011111;
                    vresult = Sse2.And(vresult, add);

                    Sse2.Store(c, vresult);

                    i += 8;
                }

                while (i < len)
                {
                    char* c = (char*)(pSource + i);
                    *c = (Char)(*c & upperCharOffset);

                    i += 1;
                }
            }
        }

    }
}
