using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text;

namespace DoDSamples.Samples
{

    public class DoDPOParser_v2a
    {
        private enum State
        {
            ID,
            DateTime,
            Amount,
            Note
        }

        public POLine_v2[] Parse(string path, out int index) => Parse(File.ReadAllLines(path), out index);

        public POLine_v2[] Parse(string[] lines, out int index)
        {
            index = 0;

            POLine_v2[] transactionLines = new POLine_v2[1000];
            POLine_v2 poLine = new POLine_v2();

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
