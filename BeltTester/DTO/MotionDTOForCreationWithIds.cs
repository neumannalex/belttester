using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeltTester.DTO
{
    public class MotionDTOForCreationWithIds
    {
        public int SequenceNumber { get; set; }
        public int StanceId { get; set; }
        public int MoveId { get; set; }
        public int TechniqueId { get; set; }
        public string Annotation { get; set; }
    }
}
