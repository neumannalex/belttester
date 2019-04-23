using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeltTester.Data.Entities
{
    public class Motion
    {
        public int ID { get; set; }

        public int SequenceNumber { get; set; }

        public int CombinationId { get; set; }
        public Combination Combination { get; set; }

        public int StanceId { get; set; }
        public Stance Stance { get; set; }

        public int MoveId { get; set; }
        public Move Move { get; set; }

        public int TechniqueId { get; set; }
        public Technique Technique { get; set; }

        public string Annotation { get; set; }
    }
}
