using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MyBeltTestingProgram.Data;
using MyBeltTestingProgram.Data.Models;
using MyBeltTestingProgram.Data.Repositories;
using NUnit.Framework;
using Sieve.Models;
using Sieve.Services;

namespace UnitTests
{
    public class RepositoryTests
    {
        private MyBeltTestingDBContext _context;
        private IDataRepository _repository;
        private ISieveProcessor _sieve;
        private ISieveCustomSortMethods _sieveCustomSort;
        private ISieveCustomFilterMethods _sieveCustomFilter;

        public RepositoryTests()
        {
            var configurationstring = "Server=(localdb)\\mssqllocaldb;Database=MyBeltTestingDB;Trusted_Connection=True;MultipleActiveResultSets=true";

            var builder = new DbContextOptionsBuilder<MyBeltTestingDBContext>()
                .UseSqlServer(configurationstring);

            _context = new MyBeltTestingDBContext(builder.Options);

            _sieveCustomSort = new SieveCustomSortMethods();
            _sieveCustomFilter = new SieveCustomFilterMethods();
            IOptions<SieveOptions> sieveOptions = Options.Create<SieveOptions>(new SieveOptions
            {
                CaseSensitive = false,
                DefaultPageSize = 10,
                MaxPageSize = 1000,
                ThrowExceptions = true
            });

            _sieve = new ApplicationSieveProcessor(sieveOptions, _sieveCustomSort, _sieveCustomFilter);
            _repository = new DataRepository(_context, _sieve);
        }

        [Test]
        public async Task GetStancesIsNotNull()
        {
            SieveModel model = new SieveModel
            {
                Filters = "",
                Sorts = "",
                Page = 1,
                PageSize = 100
            };

            var items = await _repository.GetStances(model);

            Assert.NotNull(items);
        }

        [Test]
        public async Task GetMovesIsNotNull()
        {
            SieveModel model = new SieveModel
            {
                Filters = "",
                Sorts = "",
                Page = 1,
                PageSize = 100
            };

            var items = await _repository.GetMoves(model);

            Assert.NotNull(items);
        }

        [Test]
        public async Task GetTechniquesIsNotNull()
        {
            SieveModel model = new SieveModel
            {
                Filters = "",
                Sorts = "",
                Page = 1,
                PageSize = 100
            };

            var items = await _repository.GetTechniques(model);

            Assert.NotNull(items);
        }

        [Test]
        public async Task AddNonExistingCombination()
        {
            var numStancesBefore = (await _repository.GetAllStances()).Count;
            var numMovesBefore = (await _repository.GetAllMoves()).Count;
            var numTechniquesBefore = (await _repository.GetAllTechniques()).Count;
            var numCombinationsBefore = (await _repository.GetAllCombinations()).Count;

            Combination combination = new Combination
            {
                ProgramId = 1,
                Motions = new List<Motion>
                {
                    new Motion
                    {
                        SequenceNumber = 1,
                        Stance = new Stance{Name = "Zenkutsu-Dachi", Symbol = "ZK"},
                        Move = new Move{Name = "vorwärts", Symbol = "=>"},
                        Technique = new Technique{Name = "Oi-Zuki", Level = LevelType.None, Weapon = WeaponType.Arm, Purpose = PurposeType.Attack}
                    }
                }
            };

            var createdCombination = await _repository.AddCombination(combination);

            Assert.NotNull(createdCombination);
            Assert.IsTrue(createdCombination.ID > 0);

            var numStancesAfter = (await _repository.GetAllStances()).Count;
            var numMovesAfter = (await _repository.GetAllMoves()).Count;
            var numTechniquesAfter = (await _repository.GetAllTechniques()).Count;
            var numCombinationsAfter = (await _repository.GetAllCombinations()).Count;

            Assert.IsTrue(numStancesAfter == numStancesBefore);
            Assert.IsTrue(numMovesAfter == numMovesBefore);
            Assert.IsTrue(numTechniquesAfter == numTechniquesBefore);
            Assert.IsTrue(numCombinationsAfter == numCombinationsBefore + 1);

            var success = await _repository.DeleteCombination(createdCombination.ID);
            Assert.IsTrue(success);

            var tryGetDeletedItem = await _repository.GetCombination(createdCombination.ID);
            Assert.IsNull(tryGetDeletedItem);
        }

        [Test]
        public async Task AddExistingCombination()
        {
            var numStancesBefore = (await _repository.GetAllStances()).Count;
            var numMovesBefore = (await _repository.GetAllMoves()).Count;
            var numTechniquesBefore = (await _repository.GetAllTechniques()).Count;
            var numCombinationsBefore = (await _repository.GetAllCombinations()).Count;

            Combination combination1 = new Combination
            {
                ProgramId = 1,
                Motions = new List<Motion>
                {
                    new Motion
                    {
                        SequenceNumber = 1,
                        Stance = new Stance{Name = "Zenkutsu-Dachi", Symbol = "ZK"},
                        Move = new Move{Name = "vorwärts", Symbol = "=>"},
                        Technique = new Technique{Name = "Oi-Zuki", Level = LevelType.None, Weapon = WeaponType.Arm, Purpose = PurposeType.Attack}
                    }
                }
            };

            var createdCombination1 = await _repository.AddCombination(combination1);

            Assert.NotNull(createdCombination1);
            Assert.IsTrue(createdCombination1.ID > 0);

            Combination combination2 = new Combination
            {
                ProgramId = 1,
                Motions = new List<Motion>
                {
                    new Motion
                    {
                        SequenceNumber = 1,
                        Stance = new Stance{Name = "Zenkutsu-Dachi", Symbol = "ZK"},
                        Move = new Move{Name = "vorwärts", Symbol = "=>"},
                        Technique = new Technique{Name = "Oi-Zuki", Level = LevelType.None, Weapon = WeaponType.Arm, Purpose = PurposeType.Attack}
                    }
                }
            };

            var createdCombination2 = await _repository.AddCombination(combination2);

            Assert.NotNull(createdCombination2);
            Assert.IsTrue(createdCombination2.ID > 0);

            var success1 = await _repository.DeleteCombination(createdCombination1.ID);
            Assert.IsTrue(success1);

            var success2 = await _repository.DeleteCombination(createdCombination2.ID);
            Assert.IsTrue(success2);
        }

        [Test]
        public async Task AddNonExistingLongCombination()
        {
            var numStancesBefore = (await _repository.GetAllStances()).Count;
            var numMovesBefore = (await _repository.GetAllMoves()).Count;
            var numTechniquesBefore = (await _repository.GetAllTechniques()).Count;
            var numCombinationsBefore = (await _repository.GetAllCombinations()).Count;

            Combination combination = new Combination
            {
                ProgramId = 1,
                Motions = new List<Motion>
                {
                    new Motion
                    {
                        SequenceNumber = 1,
                        Stance = new Stance{Name = "Zenkutsu-Dachi", Symbol = "ZK"},
                        Move = new Move{Name = "vorwärts", Symbol = "=>"},
                        Technique = new Technique{Name = "Oi-Zuki", Level = LevelType.Jodan, Weapon = WeaponType.Arm, Purpose = PurposeType.Attack}
                    },
                    new Motion
                    {
                        SequenceNumber = 2,
                        Stance = new Stance{Name = "Zenkutsu-Dachi", Symbol = "ZK"},
                        Move = new Move{Name = "rückwärts", Symbol = "<="},
                        Technique = new Technique{Name = "Gedan-Barai", Level = LevelType.None, Weapon = WeaponType.Arm, Purpose = PurposeType.Defense}
                    },
                    new Motion
                    {
                        SequenceNumber = 3,
                        Stance = new Stance{Name = "Zenkutsu-Dachi", Symbol = "ZK"},
                        Move = new Move{Name = "vorwärts", Symbol = "=>"},
                        Technique = new Technique{Name = "Oi-Zuki", Level = LevelType.Chudan, Weapon = WeaponType.Arm, Purpose = PurposeType.Attack}
                    }
                }
            };

            var createdCombination = await _repository.AddCombination(combination);

            Assert.NotNull(createdCombination);
            Assert.IsTrue(createdCombination.ID > 0);

            var numStancesAfter = (await _repository.GetAllStances()).Count;
            var numMovesAfter = (await _repository.GetAllMoves()).Count;
            var numTechniquesAfter = (await _repository.GetAllTechniques()).Count;
            var numCombinationsAfter = (await _repository.GetAllCombinations()).Count;

            Assert.IsTrue(numStancesAfter == numStancesBefore);
            Assert.IsTrue(numMovesAfter == numMovesBefore);
            Assert.IsTrue(numTechniquesAfter == numTechniquesBefore);
            Assert.IsTrue(numCombinationsAfter == numCombinationsBefore + 1);

            var success = await _repository.DeleteCombination(createdCombination.ID);
            Assert.IsTrue(success);

            var tryGetDeletedItem = await _repository.GetCombination(createdCombination.ID);
            Assert.IsNull(tryGetDeletedItem);
        }

        [Test]
        public async Task AddNonExistingCombinationWithWrongStanceShouldFail()
        {
            var numStancesBefore = (await _repository.GetAllStances()).Count;
            var numMovesBefore = (await _repository.GetAllMoves()).Count;
            var numTechniquesBefore = (await _repository.GetAllTechniques()).Count;
            var numCombinationsBefore = (await _repository.GetAllCombinations()).Count;

            Combination combination = new Combination
            {
                Motions = new List<Motion>
                {
                    new Motion
                    {
                        SequenceNumber = 1,
                        Stance = new Stance{Name = "Zenkutsu-Dachi", Symbol = "??"},
                        Move = new Move{Name = "vorwärts", Symbol = "=>"},
                        Technique = new Technique{Name = "Oi-Zuki", Level = LevelType.None, Weapon = WeaponType.Arm, Purpose = PurposeType.Attack}
                    }
                }
            };

            Assert.ThrowsAsync<Exception>(() => _repository.AddCombination(combination));            
        }

        [Test]
        public async Task AddNonExistingLongCombinationsWithoutCleanup()
        {
            var numStancesBefore = (await _repository.GetAllStances()).Count;
            var numMovesBefore = (await _repository.GetAllMoves()).Count;
            var numTechniquesBefore = (await _repository.GetAllTechniques()).Count;
            var numCombinationsBefore = (await _repository.GetAllCombinations()).Count;

            Combination combination1 = new Combination
            {
                ProgramId = 1,
                Motions = new List<Motion>
                {
                    new Motion
                    {
                        SequenceNumber = 1,
                        Stance = new Stance{Name = "Zenkutsu-Dachi", Symbol = "ZK"},
                        Move = new Move{Name = "vorwärts", Symbol = "=>"},
                        Technique = new Technique{Name = "Oi-Zuki", Level = LevelType.Jodan, Weapon = WeaponType.Arm, Purpose = PurposeType.Attack}
                    },
                    new Motion
                    {
                        SequenceNumber = 2,
                        Stance = new Stance{Name = "Zenkutsu-Dachi", Symbol = "ZK"},
                        Move = new Move{Name = "rückwärts", Symbol = "<="},
                        Technique = new Technique{Name = "Gedan-Barai", Level = LevelType.None, Weapon = WeaponType.Arm, Purpose = PurposeType.Defense}
                    },
                    new Motion
                    {
                        SequenceNumber = 3,
                        Stance = new Stance{Name = "Zenkutsu-Dachi", Symbol = "ZK"},
                        Move = new Move{Name = "vorwärts", Symbol = "=>"},
                        Technique = new Technique{Name = "Oi-Zuki", Level = LevelType.Chudan, Weapon = WeaponType.Arm, Purpose = PurposeType.Attack}
                    }
                }
            };

            var createdCombination1 = await _repository.AddCombination(combination1);

            Assert.NotNull(createdCombination1);
            Assert.IsTrue(createdCombination1.ID > 0);

            Combination combination2 = new Combination
            {
                ProgramId = 1,
                Motions = new List<Motion>
                {
                    new Motion
                    {
                        SequenceNumber = 1,
                        Stance = new Stance{Name = "Kokutsu-Dachi", Symbol = "KK"},
                        Move = new Move{Name = "rückwärts", Symbol = "<="},
                        Technique = new Technique{Name = "Uchi-Ude-Uke", Level = LevelType.None, Weapon = WeaponType.Arm, Purpose = PurposeType.Defense}
                    },
                    new Motion
                    {
                        SequenceNumber = 2,
                        Stance = new Stance{Name = "Kokutsu-Dachi", Symbol = "KK"},
                        Move = new Move{Name = "ohne Schritt", Symbol = "/"},
                        Technique = new Technique{Name = "Yoko-Geri Kekomi", Level = LevelType.None, Weapon = WeaponType.Leg, Purpose = PurposeType.Attack},
                        Annotation = "vorderes Bein"
                    },
                    new Motion
                    {
                        SequenceNumber = 3,
                        Stance = new Stance{Name = "Zenkutsu-Dachi", Symbol = "ZK"},
                        Move = new Move{Name = "ohne Schritt", Symbol = "/"},
                        Technique = new Technique{Name = "Gyaku-Zuki", Level = LevelType.None, Weapon = WeaponType.Arm, Purpose = PurposeType.Attack}
                    }
                }
            };

            var createdCombination2 = await _repository.AddCombination(combination2);

            Assert.NotNull(createdCombination2);
            Assert.IsTrue(createdCombination2.ID > 0);


            var numStancesAfter = (await _repository.GetAllStances()).Count;
            var numMovesAfter = (await _repository.GetAllMoves()).Count;
            var numTechniquesAfter = (await _repository.GetAllTechniques()).Count;
            var numCombinationsAfter = (await _repository.GetAllCombinations()).Count;

            Assert.IsTrue(numStancesAfter == numStancesBefore);
            Assert.IsTrue(numMovesAfter == numMovesBefore);
            Assert.IsTrue(numTechniquesAfter == numTechniquesBefore);
            Assert.IsTrue(numCombinationsAfter == numCombinationsBefore + 2);
        }
    }
}
