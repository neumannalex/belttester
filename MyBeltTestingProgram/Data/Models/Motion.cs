using Sieve.Attributes;
using System.Collections.Generic;

namespace MyBeltTestingProgram.Data.Models
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
