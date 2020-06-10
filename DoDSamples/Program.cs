using DoDSamples.Samples;
using System;
using System.Collections.Generic;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace DoDSamples
{
    class Program
    {
        static void Main(string[] args)
        {
            /////////////////
            // Rows vs Cols.
            /////////////////

            //
            // Columns Array Traversal.
            //
            //RowsVsColsTests.Cols();
            //
            // Rows Array Traversal.
            //
            //RowsVsColsTests.Rows();

            /////////////////////////////////////
            // PO File Parser - Sum the Amount.
            /////////////////////////////////////

            //
            // Regex parser.
            //
            //POParserTests.TestAmountPO_Regex();
            //
            // State Machine OOP Parser.
            //
            //POParserTests.TestAmountPO_OOP_StateMachine();
            //
            // State Machine DoD Parser.
            //
            //POParserTests.TestAmountPO_DoD_StateMachine();
            //
            // State Machine DoD Parser using split.
            //
            //POParserTests.TestAmountPO_DoD_StateMachine_Split();
            //
            // State Machine DoD Parser using hot/cold split.
            //
            //POParserTests.TestAmountPO_DoD_SateMachine_HotColdSplit();
            //
            // Fixed DoD Parsers
            //
            //POParserTests.TestAmountPO_DoD_StateMachineFix();
            //POParserTests.TestAmountPO_DoD_SateMachine_SplitFix();
            //POParserTests.TestAmountPO_DoD_SateMachine_HotColdSplitFix();

            //
            // DoD SOA
            //
            //POParserTests.TestAmountPO_DoD_SoA();
            //POParserTests.TestAmountPO_DoD_SoA_SIMD();


            //////////////////////////////////////////////
            // PO File Parser - Notes Split And To Lower.
            //////////////////////////////////////////////

            //
            // State Machine OOP Split
            //
            //POParserTests.TestNotesPO_OOP_Split();
            //
            // State Machine DoD Split 
            //
            //POParserTests.TestNotesPO_DoD_Split();

            //
            // State Machine OOP ToLower
            //
            //POParserTests.TestNotesToLowerPO_OOP();
            //
            // State Machine DoD ToLower 
            //
            //POParserTests.TestNotesToLowerPO_DoD();


            //var summary = BenchmarkRunner.Run<RowsVsCols>();
            //var summary = BenchmarkRunner.Run<OOPvsDoDParsers>();
            //var summary = BenchmarkRunner.Run<OOPvsDoDParsersToLower>();


            //SIMDSum simd = new SIMDSum();

            // Utils.Measure(() => simd.Sum(new int[128*1024*1024]));
            // Utils.Measure(() => simd.SumVectorizedSse2(array));


            //MemoryModelDeadlock memoryModelDeadlock = new MemoryModelDeadlock();
            //Utils.Measure(memoryModelDeadlock.SpinLockTest);

            //AOSvsSOA soa = new AOSvsSOA();
            //Utils.Measure(soa.TestSOA);

            //CacheSize c = new CacheSize();

            //InstructionLevelDependency ld = new InstructionLevelDependency();
            //Utils.Measure(ld.TestInDependant);

            //int steps = 65536 * 1024;
            //int[] array = new int[1024 * 1024 * 1];

            //var e =  Utils.Measure(() => c.TestK(steps, array));
            //e = e * 1000;
            //Console.WriteLine((double)e / (double)steps);

            //var n = new POGenerator().Generate();

            //RowsVsCols test = new RowsVsCols();
            //Utils.Measure(test.Cols);
            //Console.ReadKey();
        }
    }
}

