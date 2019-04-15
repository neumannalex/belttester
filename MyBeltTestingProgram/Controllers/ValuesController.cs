﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyBeltTestingProgram.Data;
using MyBeltTestingProgram.Data.Models;

namespace MyBeltTestingProgram.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly MyBeltTestingDBContext _context;

        public ValuesController(MyBeltTestingDBContext context)
        {
            _context = context;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<string> Get()
        {
            BeltTestProgram programKyu9 = new BeltTestProgram
            {
                Name = "Weißer Gürtel",
                Graduation = new Graduation { Grade = 9, Type = GradeType.Kyu },
                Kihon = new Kihon
                {
                    Combinations = new List<Combination>
                    {
                        new Combination
                        {
                            Index = 1,
                            Motions = new List<Motion>
                            {
                                new Motion
                                {
                                    Stance = new Stance{ Name = "Zenkutsu-Dachi", Symbol = "ZK" },
                                    Move = new Move{ Name = "vorwärts", Symbol = "=>" },
                                    Technique = new Technique{ Name = "Oi-Zuki" }
                                }
                            }
                        },
                        new Combination
                        {
                            Index = 2,
                            Motions = new List<Motion>
                            {
                                new Motion
                                {
                                    Stance = new Stance{ Name = "Zenkutsu-Dachi", Symbol = "ZK" },
                                    Move = new Move{ Name = "vorwärts", Symbol = "=>" },
                                    Technique = new Technique{ Name = "Gyaku-Zuki" }
                                }
                            }
                        },
                        new Combination
                        {
                            Index = 3,
                            Motions = new List<Motion>
                            {
                                new Motion
                                {
                                    Stance = new Stance{ Name = "Zenkutsu-Dachi", Symbol = "ZK" },
                                    Move = new Move{ Name = "vorwärts", Symbol = "=>" },
                                    Technique = new Technique{ Name = "Age-Uke" }
                                }
                            }
                        },
                        new Combination
                        {
                            Index = 4,
                            Motions = new List<Motion>
                            {
                                new Motion
                                {
                                    Stance = new Stance{ Name = "Zenkutsu-Dachi", Symbol = "ZK" },
                                    Move = new Move{ Name = "vorwärts", Symbol = "=>" },
                                    Technique = new Technique{ Name = "Soto-Ude-Uke" }
                                }
                            }
                        },
                        new Combination
                        {
                            Index = 5,
                            Motions = new List<Motion>
                            {
                                new Motion
                                {
                                    Stance = new Stance{ Name = "Zenkutsu-Dachi", Symbol = "ZK" },
                                    Move = new Move{ Name = "vorwärts", Symbol = "=>" },
                                    Technique = new Technique{ Name = "Gedan-Barai" }
                                }
                            }
                        },
                        new Combination
                        {
                            Index = 6,
                            Motions = new List<Motion>
                            {
                                new Motion
                                {
                                    Stance = new Stance{ Name = "Zenkutsu-Dachi", Symbol = "ZK" },
                                    Move = new Move{ Name = "vorwärts", Symbol = "=>" },
                                    Technique = new Technique{ Name = "Mae-Geri Chudan", Annotation = "aus Chudan Kamae" }
                                }
                            }
                        }
                    }
                }
            };

            return Ok(programKyu9.ToString());
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<Combination> Get(int id)
        {
            var stance = _context.Stances.Where(x => x.Symbol == "ZK").FirstOrDefault();
            var move = _context.Moves.Where(x => x.Symbol == "=>").FirstOrDefault();
            var technique = _context.Techniques.Where(x => x.Name == "Oi-Zuki").FirstOrDefault();

            var motion = new Motion{
                Stance = stance,
                Move = move,
                Technique = technique
            };

            var combo = new Combination();
            combo.Motions.Add(motion);

            return Ok(combo);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
