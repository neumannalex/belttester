using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyBeltTestingProgram.Data.Models
{
    public class Combination
    {
        public int ID { get; set; }

        public int ProgramId { get; set; }
        public BeltTestProgram Program { get; set; }

        public int SequenceNumber { get; set; }
        
        public List<Motion> Motions { get; set; } = new List<Motion>();

        //public string Hash { get; set; }
        //public static string CreateHash(Combination combination)
        //{
        //    StringBuilder hash = new StringBuilder();

        //    foreach (var motion in combination.Motions)
        //    {
        //        hash.Append(motion.Stance.ID.ToString());
        //        hash.Append(motion.Move.ID.ToString());
        //        hash.Append(motion.Technique.ID.ToString());
        //    }

        //    return hash.ToString();
        //}
    }
}
