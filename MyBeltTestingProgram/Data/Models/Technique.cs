using Sieve.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBeltTestingProgram.Data.Models
{
    public class Technique
    {
        public int ID { get; set; }
        [Sieve(CanFilter = true, CanSort = true)]
        public string Name { get; set; }
        public string Annotation { get; set; }
        public PurposeType Purpose { get; set; }
        public WeaponType Weapon { get; set; }

        public override bool Equals(object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Technique t = (Technique)obj;
                return Name == t.Name;
            }
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode() ^ Annotation.GetHashCode();
        }

        public Technique Clone()
        {
            return new Technique { Name = Name, Annotation = Annotation, Purpose = Purpose, Weapon = Weapon };
        }
    }

    public enum PurposeType
    {
        None,
        Attack,
        Defense
    }

    public enum WeaponType
    {
        None,
        Arm,
        Leg
    }
}
