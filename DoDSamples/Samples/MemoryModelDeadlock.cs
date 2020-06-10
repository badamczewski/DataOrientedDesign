using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace DoDSamples
{
    public class MemoryModelDeadlock
    {
        public void Test()
        {
            bool isSignaled = false;

            Thread master = new Thread(() =>
            {
                Console.WriteLine($"{DateTime.UtcNow} [Thread:{Thread.CurrentThread.ManagedThreadId}] Waiting 1000ms and setting the Signal Variable");
                Thread.Sleep(1000);

                Console.WriteLine($"{DateTime.UtcNow} [Thread:{Thread.CurrentThread.ManagedThreadId}] Signal!");
                isSignaled = true;
            });

            master.Start();

            Thread slave = new Thread(() =>
            {
                while (isSignaled == false) { }
                Console.WriteLine($"{DateTime.UtcNow} [Thread:{Thread.CurrentThread.ManagedThreadId}] I Was Signaled");

            });

            slave.Start();

            //slave.Join();
            //master.Join();
        }

        SpinLock spinLock = new SpinLock();

        public void Spin()
        {
            bool taken = false;
            for (int i = 0; i < 1000; i++)
            {
                taken = false;
                     
                spinLock.Enter(ref taken);
                {
                    int d = 0;
                    for (int k = 0; k < 1000000; k++)
                        d++;
                }
                spinLock.Exit(true);

                if (taken == false)
                    Console.WriteLine(taken);
            }
        }

        public void SpinLockTest()
        {
            Thread[] threads = new Thread[32];
            for(int i = 0; i < threads.Length; i++)
            {
                threads[i] = new Thread(() => Spin());
            }

            for (int i = 0; i < threads.Length; i++)
            {
                threads[i].Start();
            }

            for (int i = 0; i < threads.Length; i++)
            {
                threads[i].Join();
            }

            Console.WriteLine("[Done]");
        }
    }
}
