using System.Collections.Generic;
using System.Linq;

namespace MyBeltTestingProgram.Data.Models
{
    public class Combination
    {
        public int ID { get; set; }
        public int Index { get; set; }
        public List<Motion> Motions { get; set; } = new List<Motion>();
        public int NumberOfMotions
        {
            get
            {
                return Motions.Count;
            }
        }

        public override string ToString()
        {
            List<string> elements = new List<string>();

            if (Index > 0)
                elements.Add(Index.ToString() + ".");

            Motion lastMotion = null;
            foreach(var motion in Motions)
            {
                bool suppress = false;

                if(lastMotion != null)
                {
                    if (motion.Stance.Symbol == lastMotion.Stance.Symbol)
                        suppress = true;
                }

                elements.Add(motion.ToString(suppress));

                lastMotion = motion;
            }

            return string.Join(" ", elements);
        }

        public override bool Equals(object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Combination combo = (Combination)obj;
                if (combo.NumberOfMotions != NumberOfMotions)
                    return false;

                for(int i = 0; i < Motions.Count; i++)
                {
                    if (!Motions[i].Equals(combo.Motions[i]))
                        return false;
                }

                return true;
            }
        }

        public override int GetHashCode()
        {
            return Index.GetHashCode() ^ Motions.GetHashCode();
        }

        public Combination Clone()
        {
            Combination combo = new Combination { Index = Index };
            foreach (var motion in Motions)
                combo.Motions.Add(motion.Clone());

            return combo;
        }
    }
}
