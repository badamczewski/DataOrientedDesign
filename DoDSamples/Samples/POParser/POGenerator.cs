using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace DoDSamples.Samples
{
    public class POGenerator
    {
        string[] words;

        public POGenerator()
        {
            words = text
                .Replace(".", string.Empty)
                .Replace(",", string.Empty)
                .Split(' ');
        }

        POLine line = new POLine();

        public string Generate()
        {
            StringBuilder builder = new StringBuilder();
            Random rnd = new Random();

            for (int i = 0; i < 600; i++)
            {
                builder.AppendLine((i + 1).ToString());

                var now = DateTime.Now;
                now = now.AddHours(i);
                now = now.AddMinutes(rnd.Next(0, 60));
                now = now.AddSeconds(rnd.Next(0, 60));

                string date =
                    $"{now.Year}-{now.Month.ToString().PadLeft(2, '0')}-{now.Day.ToString().PadLeft(2, '0')}T{now.Hour.ToString().PadLeft(2, '0')}:{now.Minute.ToString().PadLeft(2, '0')}:{now.Second.ToString().PadLeft(2, '0')}";

                builder.AppendLine(date);
                builder.AppendLine(rnd.Next(100, 900).ToString());

                for (int s = 0; s < rnd.Next(1, 3); s++)
                {
                    var slice = words.Skip(rnd.Next(0, 100)).Take(rnd.Next(3, 15));
                    builder.AppendLine(string.Join(" ", slice));
                }

                builder.AppendLine();
            }

            return builder.ToString();
        }


        private string text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque gravida, dolor at tincidunt eleifend, tellus quam venenatis nibh, quis condimentum est lectus ac turpis. Integer pretium gravida gravida. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia curae; Pellentesque bibendum dolor massa, non sagittis velit faucibus eu. Quisque sed sem ac ipsum elementum bibendum id id tellus. Pellentesque lobortis fringilla orci vitae fermentum. Nulla a mauris eu sapien luctus viverra nec eleifend diam. Duis quis arcu vel massa bibendum porta. Ut vehicula sed purus nec ornare. Maecenas et tempus dolor. Aliquam non convallis quam. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. ";
    }
}
