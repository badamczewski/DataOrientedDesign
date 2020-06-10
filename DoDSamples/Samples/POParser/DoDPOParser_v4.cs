using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using System.Text;

namespace DoDSamples.Samples
{
    public struct DoDPOLines_v4
    {
        public int[] ID;
        public double[] Amount;
        public string[] Date;
        public string[] Time;
        public string[] Note;

        public DoDPOLines_v4(int size)
        {
            ID = new int[size];
            Date = new string[size];
            Time = new string[size];
            Amount = new double[size];
            Note = new string[size];
        }
    }

    public class DoDPOParser_v4
    {
        private enum State
        {
            ID,
            DateTime,
            Amount,
            Note
        }

        public DoDPOLines_v4 Parse(string path, out int index) => Parse(File.ReadAllLines(path), out index);

        public DoDPOLines_v4 Parse(string[] lines, out int index)
        {
            DoDPOLines_v4 transactionLines = new DoDPOLines_v4(1000);

            index = 0;
            State state = State.ID;
            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line))
                {
                    if (state >= State.Note)
                    {
                        state = State.ID;
                        index++;
                    }

                    continue;
                }
                switch (state)
                {
                    case State.ID:
                        {
                            transactionLines.ID[index] = int.Parse(line);

                            state++;
                            break;
                        }
                    case State.DateTime:
                        {
                            var dt = line.Split('T');
                            transactionLines.Date[index] = dt[0];
                            transactionLines.Time[index] = dt[1];

                            state++;
                            break;
                        }
                    case State.Amount:
                        {
                            transactionLines.Amount[index] = double.Parse(line);

                            state++;
                            break;
                        }
                    case State.Note:
                        {
                            transactionLines.Note[index] += line;

                            state++;
                            break;
                        }
                }
            }

            return transactionLines;
        }


        public unsafe double SumAmount(DoDPOLines_v4 source, int len)
        {
            double result;

            fixed (double* pSource = source.Amount)
            {
                Vector128<double> vresult = Vector128<double>.Zero;

                int i = 0;
                int lastBlockIndex = len - (len % 2);

                while (i < lastBlockIndex)
                {
                    vresult = Sse2.Add(vresult, Sse2.LoadVector128(pSource + i));
                    i += 2;
                }
                
                vresult = Ssse3.HorizontalAdd(vresult, vresult);
                result = vresult.ToScalar();

                while (i < len)
                {
                    result += pSource[i];
                    i += 1;
                }
            }

            return result;
        }
    }
}
