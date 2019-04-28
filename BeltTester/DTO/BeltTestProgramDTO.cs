using BeltTester.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeltTester.DTO
{
    public class BeltTestProgramDTO
    {
        public int ID { get; set; }
        public int Graduation { get; set; }
        public string GraduationType { get; set; }
        public string Name { get; set; }
        public string StyleName { get; set; }

        public List<CombinationDTO> KihonCombinations { get; set; } = new List<CombinationDTO>();
    }
}
