using MyBeltTestingProgram.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBeltTestingProgram.Data
{
    public static class DbInitializer
    {
        public static void Initialze(MyBeltTestingDBContext context)
        {
            context.Database.EnsureCreated();

            InitStances(context);
            InitMoves(context);
            InitTechniques(context);
        }

        public static void InitStances(MyBeltTestingDBContext context)
        {
            if (context.Stances.Any())
                return;

            var items = new Stance[]
            {
                new Stance{ Name = "Zenkutsu-Dachi", Symbol = "ZK" },
                new Stance{ Name = "Kokutsu-Dachi", Symbol = "KK" },
                new Stance{ Name = "Kiba-Dachi", Symbol = "KB" },
                new Stance{ Name = "Neko-Ashi-Dachi", Symbol = "NK" }
            };

            foreach (var item in items)
                context.Stances.Add(item);

            context.SaveChanges();
        }

        public static void InitMoves(MyBeltTestingDBContext context)
        {
            if (context.Moves.Any())
                return;

            var items = new Move[]
            {
                new Move{ Name = "ohne Schritt", Symbol = "/" },
                new Move{ Name = "vorwärts", Symbol = "=>" },
                new Move{ Name = "rückwärts", Symbol = "<=" },
                new Move{ Name = "seitwärts", Symbol = "<=>" }
            };

            foreach (var item in items)
                context.Moves.Add(item);

            context.SaveChanges();
        }

        public static void InitTechniques(MyBeltTestingDBContext context)
        {
            if (context.Techniques.Any())
                return;

            var items = new Technique[]
            {
                // Abwehrtechniken
                new Technique{ Name = "Age-Uke", Annotation = "", Purpose = PurposeType.Defense, Weapon = WeaponType.Arm },
                new Technique{ Name = "Soto-Ude-Uke", Annotation = "", Purpose = PurposeType.Defense, Weapon = WeaponType.Arm },
                new Technique{ Name = "Gedan-Barai", Annotation = "", Purpose = PurposeType.Defense, Weapon = WeaponType.Arm },
                new Technique{ Name = "Shuto-Uke", Annotation = "", Purpose = PurposeType.Defense, Weapon = WeaponType.Arm },
                new Technique{ Name = "Morote-Uchi-Ude-Uke", Annotation = "", Purpose = PurposeType.Defense, Weapon = WeaponType.Arm },
                new Technique{ Name = "Gedan-Nagashi-Uke", Annotation = "", Purpose = PurposeType.Defense, Weapon = WeaponType.Arm },

                // Angriff, Fausttechniken
                new Technique{ Name = "Oi-Zuki", Annotation = "", Purpose = PurposeType.Attack, Weapon = WeaponType.Arm },
                new Technique{ Name = "Oi-Zuki Chudan", Annotation = "", Purpose = PurposeType.Attack, Weapon = WeaponType.Arm },
                new Technique{ Name = "Oi-Zuki Jodan", Annotation = "", Purpose = PurposeType.Attack, Weapon = WeaponType.Arm },
                new Technique{ Name = "Gyaku-Zuki", Annotation = "", Purpose = PurposeType.Attack, Weapon = WeaponType.Arm },
                new Technique{ Name = "Gyaku-Zuki", Annotation = "im Stand", Purpose = PurposeType.Attack, Weapon = WeaponType.Arm },
                new Technique{ Name = "Ren-Zuki", Annotation = "", Purpose = PurposeType.Attack, Weapon = WeaponType.Arm },
                new Technique{ Name = "Sanbon-Zuki", Annotation = "", Purpose = PurposeType.Attack, Weapon = WeaponType.Arm },
                new Technique{ Name = "Kizami-Zuki", Annotation = "", Purpose = PurposeType.Attack, Weapon = WeaponType.Arm },
                new Technique{ Name = "Kizami-Zuki Jodan", Annotation = "", Purpose = PurposeType.Attack, Weapon = WeaponType.Arm },
                new Technique{ Name = "Yoko-Empi", Annotation = "", Purpose = PurposeType.Attack, Weapon = WeaponType.Arm },
                new Technique{ Name = "Shuto-Uchi Jodan", Annotation = "vorderer Arm", Purpose = PurposeType.Attack, Weapon = WeaponType.Arm },
                new Technique{ Name = "Shuto-Uchi", Annotation = "von Innen", Purpose = PurposeType.Attack, Weapon = WeaponType.Arm },
                new Technique{ Name = "Gyaku-Haito-Uchi Jodan", Annotation = "", Purpose = PurposeType.Attack, Weapon = WeaponType.Arm },
                new Technique{ Name = "Uraken-Uchi Jodan", Annotation = "", Purpose = PurposeType.Attack, Weapon = WeaponType.Arm },

                new Technique{ Name = "Yoko-Uraken Jodan", Annotation = "", Purpose = PurposeType.Attack, Weapon = WeaponType.Arm },
                new Technique{ Name = "Gyaku-Tate-Nukite", Annotation = "", Purpose = PurposeType.Attack, Weapon = WeaponType.Arm },
                new Technique{ Name = "Gyaku-Mae-Empi", Annotation = "", Purpose = PurposeType.Attack, Weapon = WeaponType.Arm },
                new Technique{ Name = "Gyaku-Shuto-Uchi Jodan", Annotation = "", Purpose = PurposeType.Attack, Weapon = WeaponType.Arm },

                // Angriff, Fußtechniken
                new Technique{ Name = "Mae-Geri Chudan", Annotation = "", Purpose = PurposeType.Attack, Weapon = WeaponType.Leg },
                new Technique{ Name = "Mae-Geri Jodan", Annotation = "", Purpose = PurposeType.Attack, Weapon = WeaponType.Leg },
                new Technique{ Name = "Mae-Geri", Annotation = "aus Chudan Kamae", Purpose = PurposeType.Attack, Weapon = WeaponType.Leg },
                new Technique{ Name = "Mae-Geri", Annotation = "hinten absetzen", Purpose = PurposeType.Attack, Weapon = WeaponType.Leg },
                
                new Technique{ Name = "Yoko-Geri Kekomi", Annotation = "", Purpose = PurposeType.Attack, Weapon = WeaponType.Leg },
                new Technique{ Name = "Yoko-Geri Kekomi", Annotation = "Drehung", Purpose = PurposeType.Attack, Weapon = WeaponType.Leg },
                new Technique{ Name = "Yoko-Geri Kekomi", Annotation = "aus der Drehung", Purpose = PurposeType.Attack, Weapon = WeaponType.Leg },
                new Technique{ Name = "Yoko-Geri Kekomi", Annotation = "vorderes Bein", Purpose = PurposeType.Attack, Weapon = WeaponType.Leg },
                new Technique{ Name = "Yoko-Geri Keage", Annotation = "", Purpose = PurposeType.Attack, Weapon = WeaponType.Leg },
                new Technique{ Name = "Yoko-Geri Keage", Annotation = "übersetzen", Purpose = PurposeType.Attack, Weapon = WeaponType.Leg },
                new Technique{ Name = "Yoko-Geri Keage", Annotation = "ohne Absetzen - gleiches Bein", Purpose = PurposeType.Attack, Weapon = WeaponType.Leg },

                new Technique{ Name = "Mawashi-Geri", Annotation = "", Purpose = PurposeType.Attack, Weapon = WeaponType.Leg },
                new Technique{ Name = "Mae-Mawashi-Geri", Annotation = "", Purpose = PurposeType.Attack, Weapon = WeaponType.Leg },
                new Technique{ Name = "Ura-Mawashi-Geri", Annotation = "", Purpose = PurposeType.Attack, Weapon = WeaponType.Leg },
                new Technique{ Name = "Mae-Ashi-Geri", Annotation = "", Purpose = PurposeType.Attack, Weapon = WeaponType.Leg },
                new Technique{ Name = "Mae-Ashi-Geri", Annotation = "vorderes Bein", Purpose = PurposeType.Attack, Weapon = WeaponType.Leg },
                new Technique{ Name = "Ashi-Barai", Annotation = "", Purpose = PurposeType.Attack, Weapon = WeaponType.Leg },
                new Technique{ Name = "Ashi-Barai", Annotation = "vorderer Fuß", Purpose = PurposeType.Attack, Weapon = WeaponType.Leg },

                new Technique{ Name = "Ushiro-Geri", Annotation = "", Purpose = PurposeType.Attack, Weapon = WeaponType.Leg },
                new Technique{ Name = "Ushiro-Geri", Annotation = "hinten absetzen", Purpose = PurposeType.Attack, Weapon = WeaponType.Leg },
            };

            foreach (var item in items)
                context.Techniques.Add(item);

            context.SaveChanges();
        }
    }
}
