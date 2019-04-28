using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeltTester.DTO
{
    public class MotionDTOForCreation
    {
        public int SequenceNumber { get; set; }
        public string StanceSymbol { get; set; }
        public string MoveSymbol { get; set; }
        public string TechniqueName { get; set; }
        public string TechniqueAnnotation { get; set; }
    }
}
