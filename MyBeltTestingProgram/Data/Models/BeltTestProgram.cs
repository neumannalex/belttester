using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBeltTestingProgram.Data.Models
{
    public class BeltTestProgram
    {
        public int ID { get; set; }
        public Graduation Graduation { get; set; }
        public string Name { get; set; }
        public Kihon Kihon { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            var header = Graduation.ToString();
            if (!string.IsNullOrEmpty(Name))
                header += " " + Name;

            builder.AppendLine(header);
            builder.Append(Kihon.ToString());

            return builder.ToString();
        }
    }
}
