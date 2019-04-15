using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyBeltTestingProgram.Entities.Move
{
    public class MoveDTOForCreation
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Symbol { get; set; }
    }
}
