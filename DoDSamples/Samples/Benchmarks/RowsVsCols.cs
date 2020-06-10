using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoDSamples
{

    //
    // This example demonstrates the performance
    // of Row vs Col access using a flat array.
    // It also shows how much performance can improve if we simply
    // use our cache line to the full extent.
    // 
    // It shows how proper datta access patterns are important,
    // even in a single threaded enviroment.
    //
    public class RowsVsCols
    {
        private const int rows = 10_000;
        private const int cols = 10_000;
        private int[] array = new int[rows * cols];

        [IterationSetup]
        public void SetUp()
        {
            array = new int[rows * cols];
        }

        [Benchmark]
        public void Rows()
        {
            for(int i = 0; i < cols; i++)
            {
                for(int j = 0; j < rows; j++)
                {
                    array[(i * cols) + j]++;
                }
            }
        }

        [Benchmark]
        public void Cols()
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    array[(j * cols) + i]++;
                }
            }
        }

        [Benchmark]
        public void ColsStride()
        {
            for (int i = 0; i < rows; i += 8)
            {
                for (int j = 0; j < cols; j++)
                {
                    array[(j * cols) + i + 0]++;
                    array[(j * cols) + i + 1]++;
                    array[(j * cols) + i + 2]++;
                    array[(j * cols) + i + 3]++;
                    array[(j * cols) + i + 4]++;
                    array[(j * cols) + i + 5]++;
                    array[(j * cols) + i + 6]++;
                    array[(j * cols) + i + 7]++;
                    //Console.WriteLine((j * cols) + i);
                }
            }
        }
    }
}
