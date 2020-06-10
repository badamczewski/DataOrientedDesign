using System;
using System.Collections.Generic;
using System.Text;

namespace DoDSamples.Samples
{
    public class RowsVsColsTests
    {
        private const int rows = 10_000;
        private const int cols = 10_000;

        public static void Rows()
        {
            var array = new int[rows * cols];

            Utils.Measure(() =>
            {
                for (int i = 0; i < cols; i++)
                {
                    for (int j = 0; j < rows; j++)
                    {
                        array[(i * cols) + j]++;
                    }
                }
            });
        }

        public static void Cols()
        {
            var array = new int[rows * cols];

            Utils.Measure(() =>
            {
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        array[(j * cols) + i]++;
                    }
                }
            });
        }

        public static void ColsStride()
        {
            var array = new int[rows * cols];

            Utils.Measure(() =>
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
                    }
                }
            });
        }

    }

}
