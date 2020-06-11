using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoDSamples.Samples.Benchmarks
{
  
    public class RefListVSList
    {
        private List<POLine_v1> list;
        private RefList<POLine_v1> refList;
        private const int size = 1_000_000;

        [IterationSetup(Target = nameof(List))]
        public void SetUpList()
        {
            list = new List<POLine_v1>(size);
            for (int i = 0; i < size; i++)
            {
                list.Add(new POLine_v1() { ID = i });
            }
        }

        [IterationSetup(Target = nameof(RefList))]
        public void SetUpRefList()
        {
            refList = new RefList<POLine_v1>(size);
            for (int i = 0; i < size; i++)
            {
                refList.Add(new POLine_v1() { ID = i });
            }
        }

        [Benchmark]
        public void List()
        {
            double sum = 0;

            foreach(var item in list)
            {
                sum += item.Amount;
            }
        }

        [Benchmark]
        public void RefList()
        {
            double sum = 0;

            foreach (var item in refList)
            {
                sum += item.Amount;
            }
        }
    }
}
