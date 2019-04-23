using MyBeltTestingProgram.Entities.Move;
using MyBeltTestingProgram.Entities.Stance;
using MyBeltTestingProgram.Entities.Technique;
using Sieve.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBeltTestingProgram.Entities.Motion
{
    public class MotionDTO
    {
        public int ID { get; set; }
        public StanceDTO Stance { get; set; }
        public MoveDTO Move { get; set; }
        public TechniqueDTO Technique { get; set; }
    }
}
