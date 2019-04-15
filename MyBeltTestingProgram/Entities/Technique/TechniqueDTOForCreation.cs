using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyBeltTestingProgram.Entities.Technique
{
    public class TechniqueDTOForCreation
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Purpose { get; set; }
        [Required]
        public string Weapon { get; set; }
        public string Annotation { get; set; }
    }
}
