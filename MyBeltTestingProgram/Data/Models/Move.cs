﻿using Sieve.Attributes;
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

        public List<Motion> Motions { get; set; } = new List<Motion>();
    }
}
