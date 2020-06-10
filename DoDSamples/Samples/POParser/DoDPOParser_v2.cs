using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text;

namespace DoDSamples.Samples
{

    //
    // You might thing that this is better setting the pack but if you think about it, this data set breaks
    // aligment, and thus all potential gains will be nullified by missaligment.
    //
    //[StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct POLine_v2
    {
        public int ID;
        public double Amount;
        public PODateTime PODateTime;
        public string Note;
    }

    public struct PODateTime
    {
        public string Date;
        public string Time;
    }


    public class DoDPOParser_v2
    {
        private enum State
        {
            ID,
            DateTime,
            Amount,
            Note
        }

        public List<POLine_v2> Parse(string path) => Parse(File.ReadAllLines(path));

        public List<POLine_v2> Parse(string[] lines)
        {
            List<POLine_v2> transactionLines = new List<POLine_v2>(1000);

            POLine_v2 poLine = new POLine_v2();

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
                            poLine.ID = int.Parse(line);

                            state++;
                            break;
                        }
                    case State.DateTime:
                        {
                            var dt = line.Split('T');
                            poLine.PODateTime.Date = dt[0];
                            poLine.PODateTime.Time = dt[1];

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
                            poLine.Note += line;

                            state++;
                            break;
                        }
                }
            }

            return transactionLines;
        }
    }
}
