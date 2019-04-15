using MyBeltTestingProgram.Data.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace UnitTests
{
    public class SectionTests
    {
        BeltTestProgram programKyu9;

        [SetUp]
        public void Setup()
        {
            programKyu9 = new BeltTestProgram
            {
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
        }

        [Test]
        public void Test1()
        {
            Assert.IsTrue(true);
        }
    }
}
