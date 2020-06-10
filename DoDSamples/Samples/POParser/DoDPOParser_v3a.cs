using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace DoDSamples.Samples
{
    public class DoDPOParser_v3a
    {
        private enum State
        {
            ID,
            DateTime,
            Amount,
            Note
        }

        public POLine_v3[] Parse(string path, out int index) => Parse(File.ReadAllLines(path), out index);

        public POLine_v3[] Parse(string[] lines, out int index)
        {
            index = 0;

            var transactionLines = new POLine_v3[1000];
            POLine_v3 poLine = new POLine_v3();

            State state = State.ID;
            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line))
                {
                    if (state >= State.Note)
                    {
                        transactionLines[index] = poLine;
                        state = State.ID;
                        index++;
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
