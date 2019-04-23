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
     
        public LevelType Level { get; set; }
        public PurposeType Purpose { get; set; }
        public WeaponType Weapon { get; set; }

        public List<Motion> Motions { get; set; } = new List<Motion>();
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

    public enum LevelType
    {
        None,
        Jodan,
        Chudan,
        Gedan
    }
}
