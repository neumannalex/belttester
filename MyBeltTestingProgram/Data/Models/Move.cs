using Sieve.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBeltTestingProgram.Data.Models
{
    public class Move
    {
        public int ID { get; set; }
        [Sieve(CanFilter = true, CanSort = true)]
        public string Name { get; set; }
        [Sieve(CanFilter = true, CanSort = true)]
        public string Symbol { get; set; }

        public override bool Equals(object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Move m = (Move)obj;
                return (Name == m.Name) && (Symbol == m.Symbol);
            }
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode() ^ Symbol.GetHashCode();
        }

        public Move Clone()
        {
            return new Move { Name = Name, Symbol = Symbol };
        }
    }
}
