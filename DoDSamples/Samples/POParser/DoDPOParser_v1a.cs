using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DoDSamples.Samples
{
    public class DoDPOParser_v1a
    {
        private enum State
        {
            ID,
            DateTime,
            Amount,
            Note
        }

        public POLine_v1[] Parse(string path, out int index) => Parse(File.ReadAllLines(path), out index);

        public POLine_v1[] Parse(string[] lines, out int index)
        {
            index = 0;

            POLine_v1[] transactionLines = new POLine_v1[1000];
            POLine_v1 poLine = new POLine_v1();

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
                            poLine.Date = dt[0];
                            poLine.Time = dt[1];

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
