using BenchmarkDotNet.Jobs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Text;

namespace DoDSamples
{
    public static class Utils
    {
        public static long Measure(Action a)
        {
            //
            // Quick Wormup.
            //

            Console.WriteLine(" [1] WormUp  ... ");

            for (int i = 0; i < 2; i++)
            {
                a();
            }

            Console.WriteLine(" [2] Running ... ");

            Stopwatch w = new Stopwatch();
            w.Start();
            {
                a();
            }
            w.Stop();

            Console.Write($" [3] Took: {w.ElapsedMilliseconds}");
            return w.ElapsedMilliseconds;
        }

        public static long MeasureSimple(Action a)
        {
            Stopwatch w = new Stopwatch();
            w.Start();
            {
                a();
            }
            w.Stop();

            return w.ElapsedMilliseconds;
        }
    }
}
