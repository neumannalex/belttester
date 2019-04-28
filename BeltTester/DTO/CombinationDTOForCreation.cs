using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeltTester.DTO
{
    public class CombinationDTOForCreation
    {
        public int ID { get; set; }
        public int ProgramId { get; set; }
        public int SequenceNumber { get; set; }

        public List<MotionDTOForCreation> Motions { get; set; }
    }
}
