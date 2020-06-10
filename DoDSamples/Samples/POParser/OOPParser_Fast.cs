using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DoDSamples.Samples
{
    public class POLine
    {
        public int ID { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public double Amount { get; set; }
        public string Note { get; set; }
    }

    public class POParser_Fast
    {
        private enum State
        {
            ID,
            DateTime,
            Amount,
            Note
        }

        public List<POLine> Parse(string path)
        {
            var lines = File.ReadAllLines(path);
            return Parse(lines);
        }

        public List<POLine> Parse(string[] lines)
        {
            List<POLine> transactionLines = new List<POLine>(1000);
            POLine poLine = new POLine();

            State state = State.ID;
            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line))
                {
                    if (state >= State.Note)
                    {
                        transactionLines.Add(poLine);
                        poLine = new POLine();
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
