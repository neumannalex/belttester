using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeltTester.Data.Entities
{
    public class Combination
    {
        public int ID { get; set; }

        public int ProgramId { get; set; }
        public BeltTestProgram Program { get; set; }

        public int SequenceNumber { get; set; }

        public List<Motion> Motions { get; set; } = new List<Motion>();

        public override string ToString()
        {
            List<string> friendlyMotions = new List<string>();

            Motion lastMotion = null;
            foreach(var motion in Motions.OrderBy(x => x.SequenceNumber))
            {
                string annotation = string.Empty;
                if (!string.IsNullOrEmpty(motion.Annotation))
                    annotation = $" ({motion.Annotation})";

                if (lastMotion == null)
                {
                    friendlyMotions.Add($"{motion.Stance.Symbol} {motion.Move.Symbol} {motion.Technique.Name}{annotation}");
                }
                else
                {
                    if (motion.Stance.Symbol == lastMotion.Stance.Symbol)
                    {
                        friendlyMotions.Add($"{motion.Move.Symbol} {motion.Technique.Name}{annotation}");
                    }
                    else
                    {
                        friendlyMotions.Add($"{motion.Move.Symbol} {motion.Stance.Symbol} {motion.Technique.Name}{annotation}");
                    }
                }

                lastMotion = motion;
            }

            return $"{SequenceNumber}. " + string.Join(" ", friendlyMotions);
        }
    }
}
