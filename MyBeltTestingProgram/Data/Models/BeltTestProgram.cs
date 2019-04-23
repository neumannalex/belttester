using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBeltTestingProgram.Data.Models
{
    public class BeltTestProgram
    {
        public int ID { get; set; }
        public int Graduation { get; set; }
        public GraduationType GraduationType { get; set; }
        public string Name { get; set; }
        public string StyleName { get; set; }

        public List<Combination> KihonCombinations { get; set; } = new List<Combination>();
    }

    public enum GraduationType
    {
        Kyu,
        Dan
    }
}
