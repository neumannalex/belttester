using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBeltTestingProgram.Data.Models
{
    public class Graduation
    {
        public int ID { get; set; }
        public int Grade { get; set; }
        public GradeType? Type { get; set; }

        public override string ToString()
        {
            return $"{Grade}. {Type.ToString()}";
        }
    }

    public enum GradeType
    {
        Kyu,
        Dan
    }
}
