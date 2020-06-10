using System;
using System.Collections.Generic;
using System.Text;

namespace DoDSamples
{
    //
    // Instruction Level Dependency:
    // Good compilers will do it's best to break dependency chains
    // but sometimes it's just impossible and it's up to the user to break non trival chains.
    //

    public class InstructionLevelDependency
    {
        public void TestDependant()
        {
            int steps = 64 * 1024 * 1024;
            int a = 0;

            for (int i = 0; i < steps; i++)
            {
                a++;
                a++;
            }
        }

        public void TestInDependant()
        {
            int steps = 64 * 1024 * 1024;
            int a = 0;
            int b = 0;

            for (int i = 0; i < steps; i++)
            {
                a++;
                b++;
            }
        }
    }
}
