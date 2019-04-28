using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeltTester.DTO
{
    public class StanceDTOForCreation
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Symbol { get; set; }
    }
}
