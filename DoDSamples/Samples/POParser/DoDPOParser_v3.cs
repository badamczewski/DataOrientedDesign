using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace DoDSamples.Samples
{

    //
    // You might thing that this is better setting the pack but if you think about it, this data set breaks
    // aligment, and thus all potential gains will be nullified by missaligment.
    //
    //[StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct POLine_v3
    {
        public double Amount;
        public POData POData;
    }

    public struct POData
    {
        public int ID;
        public string Date;
        public string Time;
        public string Note;
    }

    public class DoDPOParser_v3
    {
        private enum State
        {
            ID,
            DateTime,
            Amount,
            Note
        }

        public List<POLine_v3> Parse(string path) => Parse(File.ReadAllLines(path));

        public List<POLine_v3> Parse(string[] lines)
        {
            List<POLine_v3> transactionLines = new List<POLine_v3>(1000);
            POLine_v3 poLine = new POLine_v3();

            State state = State.ID;
            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line))
                {
                    if (state >= State.Note)
                    {
                        transactionLines.Add(poLine);
                        state = State.ID;
                    }

                    continue;
                }
                switch (state)
                {
                    case State.ID:
                        {
                            poLine.POData.ID = int.Parse(line);

                            state++;
                            break;
                        }
                    case State.DateTime:
                        {
                            var dt = line.Split('T');
                            poLine.POData.Date = dt[0];
                            poLine.POData.Time = dt[1];

                            state++;
                            break;
                        }
                    case State.Amount:
                        {
                            poLine.Amount = double.Parse(line);

                            state++;
                            break;
                        }
                    case State.Note:
                        {
                            poLine.POData.Note += line;

                            state++;
                            break;
                        }
                }
            }

            return transactionLines;
        }
    }
}
