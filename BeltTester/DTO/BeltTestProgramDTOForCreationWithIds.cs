using BeltTester.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeltTester.DTO
{
    public class BeltTestProgramDTOForCreationWithIds
    {
        public int ID { get; set; }
        public int Graduation { get; set; }
        public string GraduationType { get; set; }
        public string GraduationColor { get; set; }
        public string Name { get; set; }
        public string StyleName { get; set; }

        public List<CombinationDTOForCreationWithIds> KihonCombinations { get; set; } = new List<CombinationDTOForCreationWithIds>();
    }
}
