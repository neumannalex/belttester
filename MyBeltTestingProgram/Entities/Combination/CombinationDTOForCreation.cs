using MyBeltTestingProgram.Entities.Motion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBeltTestingProgram.Entities.Combination
{
    public class CombinationDTOForCreation
    {
        public List<MotionDTOForCreation> Motions { get; set; }
    }
}
