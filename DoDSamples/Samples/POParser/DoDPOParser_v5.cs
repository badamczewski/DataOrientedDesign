using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DoDSamples.Samples
{
    public struct DoDPOLines_v5
    {
        public int[] ID;
        public double[] Amount;
        public string[] Date;
        public string[] Time;
        public int[] NoteOffset;
        public StringBuilder AllNotes;

        public DoDPOLines_v5(int size)
        {
            ID = new int[size];
            Date = new string[size];
            Time = new string[size];
            Amount = new double[size];
            NoteOffset = new int[size];
            AllNotes = new StringBuilder(size);
        }
    }

    public class DoDPOParser_v5
    {
        private enum State
        {
            ID,
            DateTime,
            Amount,
            Note
        }

        public DoDPOLines_v5 Parse(string path, out int index) => Parse(File.ReadAllLines(path), out index);

        public DoDPOLines_v5 Parse(string[] lines, out int index)
        {
            DoDPOLines_v5 transactionLines = new DoDPOLines_v5(1000);

            int offset = 0;
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
                            offset += line.Length;

                            transactionLines.NoteOffset[index] = offset;
                            transactionLines.AllNotes.Append(line);

                            state++;
                            break;
                        }
                }
            }

            return transactionLines;
        }
    }
}
