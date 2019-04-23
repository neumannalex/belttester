using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyBeltTestingProgram.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBeltTestingProgram.Data
{
    public class DbInitializer
    {
        private readonly MyBeltTestingDBContext _context;
        private readonly UserManager<AppUser> _userManager;

        public DbInitializer(MyBeltTestingDBContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task InitialzeAsync()
        {
            _context.Database.EnsureCreated();

            await InitUsers();
            await InitStances();
            await InitMoves();
            await InitTechniques();
            await InitPrograms();

            //await InitFirstKyuProgram();
        }

        public async Task InitUsers()
        {
            AppUser user = await _userManager.FindByEmailAsync("admin@localhost");
            if (user == null)
            {
                user = new AppUser { FirstName = "Alexander", LastName = "Neumann", Email = "admin@localhost", UserName = "admin" };

                var result = await _userManager.CreateAsync(user, "M109a3gh()");
                if (result != IdentityResult.Success)
                    throw new InvalidOperationException("Could not create Admin user in Seeding");
            }
        }

        public async Task InitStances()
        {
            if (await _context.Stances.AnyAsync())
                return;

            var items = new Stance[]
            {
                new Stance{ Name = "Zenkutsu-Dachi", Symbol = "ZK" },
                new Stance{ Name = "Kokutsu-Dachi", Symbol = "KK" },
                new Stance{ Name = "Kiba-Dachi", Symbol = "KB" },
                new Stance{ Name = "Neko-Ashi-Dachi", Symbol = "NK" }
            };

            foreach (var item in items)
                await _context.Stances.AddAsync(item);

            await _context.SaveChangesAsync();
        }

        public async Task InitMoves()
        {
            if (await _context.Moves.AnyAsync())
                return;

            var items = new Move[]
            {
                new Move{ Name = "ohne Schritt", Symbol = "/" },
                new Move{ Name = "vorwärts", Symbol = "=>" },
                new Move{ Name = "rückwärts", Symbol = "<=" },
                new Move{ Name = "seitwärts", Symbol = "<=>" }
            };

            foreach (var item in items)
                await _context.Moves.AddAsync(item);

            await _context.SaveChangesAsync();
        }

        public async Task InitTechniques()
        {
            if (await _context.Techniques.AnyAsync())
                return;

            var items = new Technique[]
            {
                // Abwehrtechniken
                new Technique{ Name = "Age-Uke", Level = LevelType.None, Purpose = PurposeType.Defense, Weapon = WeaponType.Arm },
                new Technique{ Name = "Soto-Ude-Uke", Level = LevelType.None, Purpose = PurposeType.Defense, Weapon = WeaponType.Arm },
                new Technique{ Name = "Gedan-Barai", Level = LevelType.None, Purpose = PurposeType.Defense, Weapon = WeaponType.Arm },
                new Technique{ Name = "Shuto-Uke", Level = LevelType.None, Purpose = PurposeType.Defense, Weapon = WeaponType.Arm },
                new Technique{ Name = "Morote-Uchi-Ude-Uke", Level = LevelType.None, Purpose = PurposeType.Defense, Weapon = WeaponType.Arm },
                new Technique{ Name = "Gedan-Nagashi-Uke", Level = LevelType.None, Purpose = PurposeType.Defense, Weapon = WeaponType.Arm },
                new Technique{ Name = "Uchi-Ude-Uke", Level = LevelType.None, Purpose = PurposeType.Defense, Weapon = WeaponType.Arm},

                // Angriff, Fausttechniken
                new Technique{ Name = "Oi-Zuki", Level = LevelType.None, Purpose = PurposeType.Attack, Weapon = WeaponType.Arm },
                new Technique{ Name = "Oi-Zuki", Level = LevelType.Chudan, Purpose = PurposeType.Attack, Weapon = WeaponType.Arm },
                new Technique{ Name = "Oi-Zuki", Level = LevelType.Jodan, Purpose = PurposeType.Attack, Weapon = WeaponType.Arm },
                new Technique{ Name = "Gyaku-Zuki", Level = LevelType.None, Purpose = PurposeType.Attack, Weapon = WeaponType.Arm },
                new Technique{ Name = "Ren-Zuki", Level = LevelType.None, Purpose = PurposeType.Attack, Weapon = WeaponType.Arm },
                new Technique{ Name = "Sanbon-Zuki", Level = LevelType.None, Purpose = PurposeType.Attack, Weapon = WeaponType.Arm },
                new Technique{ Name = "Kizami-Zuki", Level = LevelType.None, Purpose = PurposeType.Attack, Weapon = WeaponType.Arm },
                new Technique{ Name = "Kizami-Zuki", Level = LevelType.Jodan, Purpose = PurposeType.Attack, Weapon = WeaponType.Arm },
                new Technique{ Name = "Yoko-Empi", Level = LevelType.None, Purpose = PurposeType.Attack, Weapon = WeaponType.Arm },
                new Technique{ Name = "Shuto-Uchi", Level = LevelType.Jodan, Purpose = PurposeType.Attack, Weapon = WeaponType.Arm },
                new Technique{ Name = "Shuto-Uchi", Level = LevelType.None, Purpose = PurposeType.Attack, Weapon = WeaponType.Arm },
                new Technique{ Name = "Gyaku-Haito-Uchi", Level = LevelType.Jodan, Purpose = PurposeType.Attack, Weapon = WeaponType.Arm },
                new Technique{ Name = "Uraken-Uchi", Level = LevelType.Jodan, Purpose = PurposeType.Attack, Weapon = WeaponType.Arm },

                new Technique{ Name = "Yoko-Uraken", Level = LevelType.Jodan, Purpose = PurposeType.Attack, Weapon = WeaponType.Arm },
                new Technique{ Name = "Gyaku-Tate-Nukite", Level = LevelType.None, Purpose = PurposeType.Attack, Weapon = WeaponType.Arm },
                new Technique{ Name = "Gyaku-Mae-Empi", Level = LevelType.None, Purpose = PurposeType.Attack, Weapon = WeaponType.Arm },
                new Technique{ Name = "Gyaku-Shuto-Uchi", Level = LevelType.Jodan, Purpose = PurposeType.Attack, Weapon = WeaponType.Arm },

                // Angriff, Fußtechniken
                new Technique{ Name = "Mae-Geri", Level = LevelType.Chudan, Purpose = PurposeType.Attack, Weapon = WeaponType.Leg },
                new Technique{ Name = "Mae-Geri", Level = LevelType.Jodan, Purpose = PurposeType.Attack, Weapon = WeaponType.Leg },
                new Technique{ Name = "Mae-Geri", Level = LevelType.None, Purpose = PurposeType.Attack, Weapon = WeaponType.Leg },
                
                new Technique{ Name = "Yoko-Geri Kekomi", Level = LevelType.None, Purpose = PurposeType.Attack, Weapon = WeaponType.Leg },
                new Technique{ Name = "Yoko-Geri Keage", Level = LevelType.None, Purpose = PurposeType.Attack, Weapon = WeaponType.Leg },

                new Technique{ Name = "Mawashi-Geri", Level = LevelType.None, Purpose = PurposeType.Attack, Weapon = WeaponType.Leg },
                new Technique{ Name = "Mae-Mawashi-Geri", Level = LevelType.None, Purpose = PurposeType.Attack, Weapon = WeaponType.Leg },
                new Technique{ Name = "Mae-Ashi-Geri", Level = LevelType.None, Purpose = PurposeType.Attack, Weapon = WeaponType.Leg },
                new Technique{ Name = "Ashi-Barai", Level = LevelType.None, Purpose = PurposeType.Attack, Weapon = WeaponType.Leg },

                new Technique{ Name = "Ushiro-Geri", Level = LevelType.None, Purpose = PurposeType.Attack, Weapon = WeaponType.Leg },
            };

            foreach (var item in items)
                await _context.Techniques.AddAsync(item);

            await _context.SaveChangesAsync();
        }

        public async Task InitPrograms()
        {
            if (await _context.BeltTestPrograms.AnyAsync())
                return;

            var items = new BeltTestProgram[]
            {
                new BeltTestProgram
                {
                    Name = "Weißer Gürtel",
                    Graduation = 9,
                    GraduationType = GraduationType.Kyu,
                    StyleName = "Shotokan"
                }
            };

            foreach (var item in items)
                await _context.BeltTestPrograms.AddAsync(item);

            await _context.SaveChangesAsync();
        }
    }
}
