using System;
using System.Collections.Generic;
using System.Text;

namespace DoDSamples
{
    public class CacheSize
    {
        //
        // This class will test the cache line sizes for various levels
        // The idea is simple: Move through the array loading a new cache line on every step
        // if we exceed the size of the array we start from the begining and continue.
        // We do this for a set amount of steps and mesure the assigment time, if we move to the 
        // next cache level there will be a significant slowdown.
        //
        // How to Perform the test:
        //
        //  int steps = 65536 * 1024;
        //  int[] array = new int[1024 * 1024 * 1];
        //
        //  var e = Utils.Measure(() => c.TestK(steps, array));
        //  e = e* 1000; //Scale the time
        //  Console.WriteLine((double) e / (double) steps);
        //
        public void TestK(int steps, int[] array)
        {
            int lengthMod = array.Length - 1;

            for (int i = 0; i < steps; i++)
            {
                array[(i * 16) & lengthMod]++;
            }
        }
    }
}
