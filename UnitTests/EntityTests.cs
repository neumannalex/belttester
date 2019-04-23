using MyBeltTestingProgram.Data.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace UnitTests
{
    public class EntityTests
    {
        Combination simpleCombination1;
        Combination complexCombination1;
        Combination complexCombination2;
        Combination complexCombination3;

        [SetUp]
        public void Setup()
        {
            //CreateSimpleCombination1();
            //CreateComplexCombination1();
            //CreateComplexCombination2();
            //CreateComplexCombination3();
        }

        //private void CreateSimpleCombination1()
        //{
        //    simpleCombination1 = new Combination
        //    {
        //        Motions = new List<Motion>
        //        {
        //            new Motion
        //            {
        //                Stance = new Stance { Name = "Zenkutsu-Dachi", Symbol = "ZK" },
        //                Move = new Move { Name = "vorwärts", Symbol = "=>" },
        //                Technique = new Technique { Name = "Oi-Zuki" }
        //            }
        //        }
        //    };
        //}

        //private void CreateComplexCombination1()
        //{
        //    complexCombination1 = new Combination
        //    {
        //        Index = 1,
        //        Motions = new List<Motion>
        //        {
        //            new Motion
        //            {
        //                Stance = new Stance { Name = "Zenkutsu-Dachi", Symbol = "ZK" },
        //                Move = new Move { Name = "vorwärts", Symbol = "=>" },
        //                Technique = new Technique { Name = "Uchi-Ude-Kue" }
        //            },
        //            new Motion
        //            {
        //                Stance = new Stance { Name = "Zenkutsu-Dachi", Symbol = "ZK" },
        //                Move = new Move { Name = "ohne Schritt", Symbol = "/" },
        //                Technique = new Technique { Name = "Kizami-Zuki" }
        //            },
        //            new Motion
        //            {
        //                Stance = new Stance { Name = "Zenkutsu-Dachi", Symbol = "ZK" },
        //                Move = new Move { Name = "ohne Schritt", Symbol = "/" },
        //                Technique = new Technique { Name = "Gyaku-Zuki" }
        //            }
        //        }
        //    };
        //}
        //private void CreateComplexCombination2()
        //{
        //    complexCombination2 = new Combination
        //    {
        //        Index = 1,
        //        Motions = new List<Motion>
        //        {
        //            new Motion
        //            {
        //                Stance = new Stance { Name = "Kiba-Dachi", Symbol = "KB" },
        //                Move = new Move { Name = "seitwärts", Symbol = "<=>" },
        //                Technique = new Technique { Name = "Yoko-Geri Keage", Annotation = "übersetzen" }
        //            },
        //            new Motion
        //            {
        //                Stance = new Stance { Name = "Kiba-Dachi", Symbol = "KB" },
        //                Move = new Move { Name = "ohne Schritt", Symbol = "/" },
        //                Technique = new Technique { Name = "Yoko-Geri Kekomi", Annotation = "Drehung" }
        //            }
        //        }
        //    };
        //}
        //private void CreateComplexCombination3()
        //{
        //    complexCombination3 = new Combination
        //    {
        //        Index = 1,
        //        Motions = new List<Motion>
        //        {
        //            new Motion
        //            {
        //                Stance = new Stance { Name = "Kiba-Dachi", Symbol = "KB" },
        //                Move = new Move { Name = "ohne Schritt", Symbol = "/" },
        //                Technique = new Technique { Name = "Yoko-Geri Kekomi", Annotation = "Drehung" }
        //            },
        //            new Motion
        //            {
        //                Stance = new Stance { Name = "Kiba-Dachi", Symbol = "KB" },
        //                Move = new Move { Name = "seitwärts", Symbol = "<=>" },
        //                Technique = new Technique { Name = "Yoko-Geri Keage", Annotation = "übersetzen" }
        //            }
        //        }
        //    };
        //}

        //[Test]
        //public void TechniqueEqualityShouldBeTrue()
        //{
        //    Technique t1 = new Technique { Name = "Oi-Zuki", Annotation = "aus Chudan Kamae" };
        //    Technique t2 = new Technique { Name = "Oi-Zuki", Annotation = "aus Chudan Kamae" };

        //    Assert.IsTrue(t1.Equals(t2));
        //}

        //[Test]
        //public void TechniqueEqualityShouldBeFals()
        //{
        //    Technique t1 = new Technique { Name = "Oi-Zuki", Annotation = "aus Chudan Kamae" };
        //    Technique t2 = new Technique { Name = "Gyaku-Zuki", Annotation = "aus Chudan Kamae" };

        //    Assert.IsFalse(t1.Equals(t2));
        //}

        //[Test]
        //public void MoveEqualityShouldBeTrue()
        //{
        //    Move m1 = new Move { Name = "vorwärts", Symbol = "=>" };
        //    Move m2 = new Move { Name = "vorwärts", Symbol = "=>" };

        //    Assert.IsTrue(m1.Equals(m2));
        //}

        //[Test]
        //public void MoveEqualityShouldBeFalse()
        //{
        //    Move m1 = new Move { Name = "vorwärts", Symbol = "=>" };
        //    Move m2 = new Move { Name = "vorwärts", Symbol = "<=>" };

        //    Assert.IsFalse(m1.Equals(m2));
        //}

        //[Test]
        //public void StanceEqualityShouldBeTrue()
        //{
        //    Stance s1 = new Stance { Name = "Zenkutsu-Dachi", Symbol = "ZK"};
        //    Stance s2 = new Stance { Name = "Zenkutsu-Dachi", Symbol = "ZK" };

        //    Assert.IsTrue(s1.Equals(s2));
        //}

        //[Test]
        //public void StanceEqualityShouldBeFalse()
        //{
        //    Stance s1 = new Stance { Name = "Zenkutsu-Dachi", Symbol = "ZK" };
        //    Stance s2 = new Stance { Name = "Zenkutsu-Dachi", Symbol = "KK" };

        //    Assert.IsFalse(s1.Equals(s2));
        //}

        //[Test]
        //public void MotionToStringWithoutAnnotation()
        //{
        //    Motion m1 = new Motion
        //    {
        //        Stance = new Stance { Name = "Zenkutsu-Dachi", Symbol = "ZK" },
        //        Move = new Move { Name = "vorwärts", Symbol = "=>" },
        //        Technique = new Technique { Name = "Oi-Zuki" }
        //    };

        //    var template = "ZK => Oi-Zuki";
        //    var text = m1.ToString();

        //    Assert.IsTrue(template == text);
        //}

        //[Test]
        //public void MotionToStringWithoutAnnotationAndSuppressedStance()
        //{
        //    Motion m1 = new Motion
        //    {
        //        Stance = new Stance { Name = "Zenkutsu-Dachi", Symbol = "ZK" },
        //        Move = new Move { Name = "vorwärts", Symbol = "=>" },
        //        Technique = new Technique { Name = "Oi-Zuki" }
        //    };

        //    var template = "=> Oi-Zuki";
        //    var text = m1.ToString(true);

        //    Assert.IsTrue(template == text);
        //}

        //[Test]
        //public void MotionToStringWithAnnotation()
        //{
        //    Motion m1 = new Motion
        //    {
        //        Stance = new Stance { Name = "Zenkutsu-Dachi", Symbol = "ZK" },
        //        Move = new Move { Name = "vorwärts", Symbol = "=>" },
        //        Technique = new Technique { Name = "Oi-Zuki", Annotation = "aus Chudan Kamae" }
        //    };

        //    var template = "ZK => Oi-Zuki (aus Chudan Kamae)";
        //    var text = m1.ToString();

        //    Assert.IsTrue(template == text);
        //}

        //[Test]
        //public void MotionEqualityShouldBeTrue()
        //{
        //    Motion m1 = new Motion
        //    {
        //        Stance = new Stance { Name = "Zenkutsu-Dachi", Symbol = "ZK" },
        //        Move = new Move { Name = "vorwärts", Symbol = "=>" },
        //        Technique = new Technique { Name = "Oi-Zuki" }
        //    };

        //    Motion m2 = new Motion
        //    {
        //        Stance = new Stance { Name = "Zenkutsu-Dachi", Symbol = "ZK" },
        //        Move = new Move { Name = "vorwärts", Symbol = "=>" },
        //        Technique = new Technique { Name = "Oi-Zuki" }
        //    };

        //    Assert.IsTrue(m1.Equals(m2));
        //}

        //[Test]
        //public void MotionEqualityShouldBeFalse()
        //{
        //    Motion m1 = new Motion
        //    {
        //        Stance = new Stance { Name = "Zenkutsu-Dachi", Symbol = "ZK" },
        //        Move = new Move { Name = "vorwärts", Symbol = "=>" },
        //        Technique = new Technique { Name = "Oi-Zuki" }
        //    };

        //    Motion m2 = new Motion
        //    {
        //        Stance = new Stance { Name = "Kokutsu-Dachi", Symbol = "KK" },
        //        Move = new Move { Name = "vorwärts", Symbol = "=>" },
        //        Technique = new Technique { Name = "Shuto-Uchi" }
        //    };

        //    Assert.IsFalse(m1.Equals(m2));
        //}

        //[Test]
        //public void CombinationToStringWithoutAnnotation()
        //{
        //    Motion m1 = new Motion
        //    {
        //        Stance = new Stance { Name = "Zenkutsu-Dachi", Symbol = "ZK" },
        //        Move = new Move { Name = "vorwärts", Symbol = "=>" },
        //        Technique = new Technique { Name = "Oi-Zuki" }
        //    };

        //    Motion m2 = new Motion
        //    {
        //        Stance = new Stance { Name = "Zenkutsu-Dachi", Symbol = "ZK" },
        //        Move = new Move { Name = "ohne Schritt", Symbol = "/" },
        //        Technique = new Technique { Name = "Gyaku-Zuki" }
        //    };

        //    Combination combo = new Combination();
        //    combo.Index = 1;
        //    combo.Motions.Add(m1);
        //    combo.Motions.Add(m2);

        //    var template = "1. ZK => Oi-Zuki / Gyaku-Zuki";
        //    var text = combo.ToString();

        //    Assert.IsTrue(template == text);
        //}

        //[Test]
        //public void CombinationEqualityShouldBeTrue()
        //{
        //    Combination combo1 = complexCombination1.Clone();
        //    Combination combo2 = complexCombination1.Clone();

        //    Assert.IsTrue(combo1.Equals(combo2));
        //}

        //[Test]
        //public void CombinationEqualityShouldBeFalse1()
        //{
        //    Combination combo1 = complexCombination1.Clone();
        //    Combination combo2 = complexCombination1.Clone();
        //    combo2.Motions[1].Stance = new Stance { Name = "Kokutsu-Dachi", Symbol = "KK" };

        //    Assert.IsFalse(combo1.Equals(combo2));
        //}

        //[Test]
        //public void CombinationEqualityShouldBeFalse2()
        //{
        //    Combination combo1 = complexCombination1.Clone();
        //    Combination combo2 = simpleCombination1.Clone();

        //    Assert.IsFalse(combo1.Equals(combo2));
        //}

        //[Test]
        //public void CombinationEqualityShouldBeFalse3()
        //{
        //    Combination combo1 = complexCombination2.Clone();
        //    Combination combo2 = complexCombination3.Clone();

        //    Assert.IsFalse(combo1.Equals(combo2));
        //}
    }
}