using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeltTester.DTO
{
    public class MotionDTO
    {
        public int ID { get; set; }
        public int SequenceNumber { get; set; }
        public StanceDTO Stance { get; set; }
        public MoveDTO Move { get; set; }
        public TechniqueDTO Technique { get; set; }
        public string Annotation { get; set; }
    }
}
