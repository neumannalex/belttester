using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeltTester.Data.Entities
{
    public class Combination
    {
        public int ID { get; set; }

        public int ProgramId { get; set; }
        public BeltTestProgram Program { get; set; }

        public int SequenceNumber { get; set; }

        public List<Motion> Motions { get; set; } = new List<Motion>();
    }
}
