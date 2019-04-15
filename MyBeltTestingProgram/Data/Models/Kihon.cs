using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBeltTestingProgram.Data.Models
{
    public class Kihon
    {
        public List<Combination> Combinations { get; set; } = new List<Combination>();
        public List<Combination> OrderedCombinations
        {
            get
            {
                return Combinations.OrderBy(x => x.Index).ToList();
            }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine("Kihon");
            foreach (var combo in OrderedCombinations)
                builder.AppendLine(combo.ToString());

            return builder.ToString();
        }
    }
}
