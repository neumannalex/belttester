using MyBeltTestingProgram.Data.Models;
using Sieve.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBeltTestingProgram.Entities.Technique
{
    public class TechniqueDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Annotation { get; set; }
        public string Purpose { get; set; }
        public string Weapon { get; set; }
    }
}
