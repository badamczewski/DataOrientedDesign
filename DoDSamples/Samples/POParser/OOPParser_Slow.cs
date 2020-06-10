using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace DoDSamples.Samples
{
    public class POParser
    {
        private static Regex matchPOLine =
            new Regex(
            @"(?<id>\d)\r\n(?<date>[0-9]{4}-[0-9]{2}-[0-9]{2})T(?<time>[0-9]{2}:[0-9]{2}:[0-9]{2})\r\n(?<amount>\d+)\r\n(?<note>[a-zA-Z0-9 ;,-.]+(\r\n([a-zA-Z0-9]+)){0,1})",
            RegexOptions.Compiled | RegexOptions.Multiline);


        public List<POLine> ParseFromPath(string path)
        {
            var text = File.ReadAllText(path);
            return Parse(text);
        }

        public List<POLine> Parse(string text)
        {
            List<POLine> transactionLines = new List<POLine>();

            var groups = matchPOLine.GetGroupNames();

            foreach (Match m in matchPOLine.Matches(text))
            {
                POLine line = new POLine();

                var id = m.Groups["id"].Value;
                var date = m.Groups["date"].Value;
                var time = m.Groups["time"].Value;
                var amount = m.Groups["amount"].Value;
                var note = m.Groups["note"].Value;

                line.ID = int.Parse(id);
                line.Date = date;
                line.Time = time;
                line.Amount = double.Parse(amount);
                line.Note = note;

                transactionLines.Add(line);
            }

            return transactionLines;
        }
    }
}
