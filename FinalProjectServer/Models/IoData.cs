using System.Collections.Generic;
using System.Linq;

namespace FinalProjectServer.Models
{
    public class IoData
    {
        public IoData(string data)
        {
            var rows = data.Split('\n', System.StringSplitOptions.RemoveEmptyEntries);

            foreach (var row in rows)
            {
                var splitPair = row.Split(new string[] { "(", ")", ")(" }, System.StringSplitOptions.RemoveEmptyEntries);
                var input = splitPair[0];
                var output = splitPair[1];

                Data.Add(new IoPair()
                {
                    Input = input.Split(',').Select(double.Parse).ToList(),
                    Output = output.Split(',').Select(double.Parse).ToList()
                });
            }
        }

        public IList<IoPair> Data
        {
            get;
            set;
        }
    }
}