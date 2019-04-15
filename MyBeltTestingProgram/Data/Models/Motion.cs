using Sieve.Attributes;
using System.Collections.Generic;

namespace MyBeltTestingProgram.Data.Models
{
    public class Motion
    {
        public int ID { get; set; }
        public int CombinationId { get; set; }
        public Stance Stance { get; set; }
        public Move Move { get; set; }
        public Technique Technique { get; set; }

        public string ToString(bool suppressStance = false)
        {
            List<string> elements = new List<string>();

            if (!suppressStance)
            {
                if (Stance != null && !string.IsNullOrEmpty(Stance.Symbol))
                    elements.Add(Stance.Symbol);
            }

            if (Move != null && !string.IsNullOrEmpty(Move.Symbol))
                elements.Add(Move.Symbol);

            if(Technique != null && !string.IsNullOrEmpty(Technique.Name))
            {
                elements.Add(Technique.Name);

                if (!string.IsNullOrEmpty(Technique.Annotation))
                    elements.Add($"({Technique.Annotation})");
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
                Motion m = (Motion)obj;

                bool equals1 = false;
                bool equals2 = false;
                bool equals3 = false;

                if (Stance != null && m.Stance != null)
                    equals1 = Stance.Equals(m.Stance);

                if (Move != null && m.Move != null)
                    equals2 = Move.Equals(m.Move);

                if (Technique != null && m.Technique != null)
                    equals3 = Technique.Equals(m.Technique);

                return (equals1 && equals2 && equals3);
            }
        }

        public override int GetHashCode()
        {
            return Stance.GetHashCode() ^ Move.GetHashCode() ^ Technique.GetHashCode();
        }

        public Motion Clone()
        {
            return new Motion
            {
                //StanceId = StanceId,
                Stance = Stance.Clone(),
                //MoveId = MoveId,
                Move = Move.Clone(),
                //TechniqueId = TechniqueId,
                Technique = Technique.Clone()
            };
        }
    }
}
