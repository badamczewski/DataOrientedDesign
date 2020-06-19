using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoDSamples
{
    [Orderer(SummaryOrderPolicy.SlowestToFastest, MethodOrderPolicy.Declared)]
    public class InstructionAndDataDependency
    {
        
        //[Benchmark]
        //public void InsDependency()
        //{
        //    InstructionLevelDependency.TestDependency();
        //}

        //[Benchmark]
        //public void InsIndependency()
        //{
        //    InstructionLevelDependency.TestIndependency();
        //}

        [Benchmark]
        public void InsIndependencySumUnroll()
        {
            InstructionLevelDependency.TestIndependencyUnrolledSum();
        }

        [Benchmark]
        public void InsDependencySumUnroll()
        {
            InstructionLevelDependency.TestDependencyUnrolledSum();
        }

        [Benchmark]
        public void InsDependencySum()
        {
            InstructionLevelDependency.TestSum();
        }
    }
}
