using MyBeltTestingProgram.Entities.Motion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBeltTestingProgram.Entities.Combination
{
    public class CombinationDTO
    {
        public int ID { get; set; }
        public string Hash { get; set; }
        public List<MotionDTO> Motions { get; set; }
    }
}
