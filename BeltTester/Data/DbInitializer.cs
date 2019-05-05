using BeltTester.Data.Entities;
using BeltTester.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace BeltTester.Data
{
    public class DbInitializer
    {
        private readonly BeltTesterDBContext _context;
        private readonly IConfiguration _configuration;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(BeltTesterDBContext context, IConfiguration configuration, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _configuration = configuration;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task InitialzeAsync()
        {
            _context.Database.EnsureCreated();

            await InitRoles();
            await InitUsers();
            await InitStances();
            await InitMoves();
            await InitTechniques();
            await InitPrograms();
        }

        public async Task InitRoles()
        {
            string[] roleNames = { UserRoleName.Admin, UserRoleName.Manager, UserRoleName.Member };

            foreach(var role in roleNames)
            {
                var roleExists = await _roleManager.RoleExistsAsync(role);
                if(!roleExists)
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        public async Task InitUsers()
        {
            var adminSettings = _configuration.GetSection("AdminUser");

            if (adminSettings == null)
                throw new KeyNotFoundException("AdminUser is appsettings.json not found.");

            var firstname = adminSettings.GetValue<string>("Firstname", "Admin");
            var lastname = adminSettings.GetValue<string>("Lastname", "Admin");
            var email = adminSettings.GetValue<string>("Email", "admin@localhost");
            var password = adminSettings.GetValue<string>("Password", "Qaywsx12()");

            AppUser admin = await _userManager.FindByEmailAsync(email);
            if (admin == null)
            {
                admin = new AppUser { FirstName = firstname, LastName = lastname, Email = email, UserName = email };

                var result = await _userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRolesAsync(admin, new string[] { UserRoleName.Admin, UserRoleName.Manager, UserRoleName.Member });
                }
                else
                {
                    throw new InvalidOperationException("Could not create Admin user in Seeding");
                }
            }

            var managerEmail = "manager@localhost";
            var managerPassword = "Qaywsx12()";
            AppUser manager = await _userManager.FindByEmailAsync(managerEmail);
            if (manager == null)
            {
                manager = new AppUser { FirstName = "Manager", LastName = "Manager", Email = managerEmail, UserName = managerEmail };

                var result = await _userManager.CreateAsync(manager, managerPassword);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(manager, UserRoleName.Manager);
                }
                else
                {
                    throw new InvalidOperationException("Could not create Admin user in Seeding");
                }
            }

            var userEmail = "user@localhost";
            var userPassword = "Qaywsx12()";
            AppUser user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                user = new AppUser { FirstName = "John", LastName = "Doe", Email = userEmail, UserName = userEmail };

                var result = await _userManager.CreateAsync(user, userPassword);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, UserRoleName.Member);
                }
                else
                {
                    throw new InvalidOperationException("Could not create Admin user in Seeding");
                }
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
            var path =  Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),"Data", "Seed");
            var files = Directory.EnumerateFiles(path, "*.txt");

            foreach(var file in files)
            {
                var json = File.ReadAllText(file, System.Text.Encoding.Default);

                try
                {
                    var program = ParseBeltTestProgram(json);

                    if (_context.BeltTestPrograms.Where(x => x.Graduation == program.Graduation &&
                                                     x.GraduationType == program.GraduationType &&
                                                     x.StyleName.ToLower() == program.StyleName.ToLower())
                                                    .Count() <= 0)
                    {
                        await _context.BeltTestPrograms.AddAsync(program);
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            await _context.SaveChangesAsync();
        }

        private BeltTestProgram ParseBeltTestProgram(string json)
        {
            BeltTestProgram program = new BeltTestProgram();

            JObject o = JObject.Parse(json);
            program.Graduation = (int)o["graduation"];
            program.GraduationType = Enum.Parse<GraduationType>((string)o["graduationType"], true);
            program.Name = (string)o["name"];
            program.StyleName = (string)o["styleName"];
            program.GraduationColor = (string)o["graduationColor"];

            JArray combinations = (JArray)o["kihonCombinations"];
            foreach(var c in combinations)
            {
                Combination combo = new Combination();
                combo.SequenceNumber = (int)c["sequenceNumber"];

                JArray motions = (JArray)c["motions"];
                foreach(var m in motions)
                {
                    int sequenceNumber = (int)m["sequenceNumber"];
                    string stanceSymbol = (string)m["stance"];
                    string moveSymbol = (string)m["move"];
                    string techniqueName = (string)m["technique"];
                    string annotation = (string)m["annotation"];
                    LevelType techniqueLevel = LevelType.None;

                    if(m["techniqueLevel"] != null)
                    {
                        if (!Enum.TryParse<LevelType>((string)m["techniqueLevel"], out techniqueLevel))
                            techniqueLevel = LevelType.None;
                    }

                    var stance = _context.Stances.Where(x => x.Symbol.ToLower() == stanceSymbol.ToLower()).FirstOrDefault();
                    var move = _context.Moves.Where(x => x.Symbol == moveSymbol).FirstOrDefault();
                    var technique = _context.Techniques.Where(x => x.Name.ToLower() == techniqueName.ToLower() && x.Level == techniqueLevel).FirstOrDefault();

                    if (stance == null)
                        throw new ArgumentOutOfRangeException($"Stance '{stanceSymbol}' does not exist");

                    if (move == null)
                        throw new ArgumentOutOfRangeException($"Move '{moveSymbol}' does not exist");

                    if (technique == null)
                        throw new ArgumentOutOfRangeException($"Technique '{techniqueName}' does not exist");

                    combo.Motions.Add(new Motion
                    {
                        SequenceNumber = sequenceNumber,
                        Stance = stance,
                        StanceId = stance.ID,
                        Move = move,
                        MoveId = move.ID,
                        Technique = technique,
                        TechniqueId = technique.ID,
                        Annotation = annotation
                    });
                }

                program.KihonCombinations.Add(combo);
            }

            return program;
        }
    }
}
