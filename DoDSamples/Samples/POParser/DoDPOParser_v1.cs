using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DoDSamples.Samples
{
    public struct POLine_v1
    {
        public int ID;
        public double Amount;
        public string Date;
        public string Time;
        public string Note;
    }

    public class DoDPOParser_v1
    {
     
        private enum State
        {
            ID,
            DateTime,
            Amount,
            Note
        }

        public List<POLine_v1> Parse(string path) => Parse(File.ReadAllLines(path));

        public List<POLine_v1> Parse(string[] lines)
        {
            List<POLine_v1> transactionLines = new List<POLine_v1>(1000);
            POLine_v1 poLine = new POLine_v1();

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
