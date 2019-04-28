using BeltTester.Data.Entities;
using BeltTester.Helpers;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeltTester.Data
{
    public class DataRepository : IDataRepository
    {
        private readonly BeltTesterDBContext _context;
        private readonly ISieveProcessor _sieveProcessor;
        public DataRepository(BeltTesterDBContext context, ISieveProcessor sieveProcessor)
        {
            _context = context;
            _sieveProcessor = sieveProcessor;
        }

        #region Techniques
        public async Task<bool> TechniqueExists(int id)
        {
            return await _context.Techniques.AnyAsync(x => x.ID == id);
        }

        public async Task<Technique> GetTechnique(int id)
        {
            var item = await _context.Techniques.FindAsync(id);

            if (item == null)
                return null;

            return item;
        }

        public async Task<Technique> GetTechnique(Technique technique)
        {
            var item = await _context.Techniques.Where(x => x.Name.ToLowerInvariant() == technique.Name.ToLowerInvariant() &&
                                                            x.Level == technique.Level &&
                                                            x.Purpose == technique.Purpose &&
                                                            x.Weapon == technique.Weapon).FirstOrDefaultAsync();

            return item;
        }

        public async Task<IEnumerable<Technique>> GetTechniquesByName(string name)
        {
            var item = await _context.Techniques.Where(x => x.Name.ToLowerInvariant() == name.ToLowerInvariant()).ToListAsync();

            return item;
        }

        public async Task<List<Technique>> GetAllTechniques()
        {
            var itemsBeforeProcessing = _context.Techniques.OrderBy(x => x.Name).AsQueryable();

            return await itemsBeforeProcessing.ToListAsync();
        }

        public async Task<PagedList<Technique>> GetTechniques(SieveModel sieveModel)
        {
            try
            {
                var items = _context.Techniques.AsQueryable();

                var processedItems = _sieveProcessor.Apply(sieveModel, items, applyPagination: false);

                return await PagedList<Technique>.Create(processedItems, sieveModel.Page.Value, sieveModel.PageSize.Value);
            }
            catch (Exception ex)
            {
                throw new RepositoryFilterException(ex.Message);
            }
        }

        public async Task<Technique> AddTechnique(Technique technique)
        {
            var existingTechnique = await GetTechnique(technique);

            if (existingTechnique != null)
                throw new RepositoryItemAlreadyExistsException();

            _context.Techniques.Add(technique);
            if (await _context.SaveChangesAsync() >= 0)
                return technique;
            else
                return null;
        }

        public async Task<bool> UpdateTechnique(int id, Technique technique)
        {
            if (!await TechniqueExists(id))
                throw new RepositoryItemNotFoundException();

            if (technique == null)
                return false;

            if (id != technique.ID)
                throw new RepositoryItemMismatchException();

            _context.Entry(technique).State = EntityState.Modified;

            try
            {
                return (await _context.SaveChangesAsync() >= 0);
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteTechnique(int id)
        {
            var item = await GetTechnique(id);
            if (item == null)
                throw new RepositoryItemNotFoundException();

            _context.Techniques.Remove(item);
            return (await _context.SaveChangesAsync() >= 0);
        }
        #endregion

        #region Stances
        public async Task<bool> StanceExists(int id)
        {
            return await _context.Stances.AnyAsync(x => x.ID == id);
        }

        public async Task<Stance> GetStance(int id)
        {
            var item = await _context.Stances.FindAsync(id);

            if (item == null)
                return null;

            return item;
        }

        public async Task<Stance> GetStance(Stance stance)
        {
            var item = await _context.Stances.Where(x => x.Name.ToLowerInvariant() == stance.Name.ToLowerInvariant() &&
                                                         x.Symbol.ToLowerInvariant() == stance.Symbol.ToLowerInvariant()).FirstOrDefaultAsync();

            return item;
        }

        public async Task<Stance> GetStanceBySymbol(string symbol)
        {
            var item = await _context.Stances.Where(x => x.Symbol.ToLowerInvariant() == symbol.ToLowerInvariant()).FirstOrDefaultAsync();

            return item;
        }

        public async Task<List<Stance>> GetAllStances()
        {
            var itemsBeforeProcessing = _context.Stances.OrderBy(x => x.Name).AsQueryable();
            return await itemsBeforeProcessing.ToListAsync();
        }

        public async Task<PagedList<Stance>> GetStances(SieveModel sieveModel)
        {
            try
            {
                var items = _context.Stances.AsQueryable();

                var processedItems = _sieveProcessor.Apply(sieveModel, items, applyPagination: false);

                return await PagedList<Stance>.Create(processedItems, sieveModel.Page.Value, sieveModel.PageSize.Value);
            }
            catch (Exception ex)
            {
                throw new RepositoryFilterException(ex.Message);
            }
        }

        public async Task<Stance> AddStance(Stance stance)
        {
            var existingItem = await GetStance(stance);

            if (existingItem != null)
                throw new RepositoryItemAlreadyExistsException();

            _context.Stances.Add(stance);
            if (await _context.SaveChangesAsync() >= 0)
                return stance;
            else
                return null;
        }

        public async Task<bool> UpdateStance(int id, Stance stance)
        {
            if (!await StanceExists(id))
                throw new RepositoryItemNotFoundException();

            if (stance == null)
                return false;

            if (id != stance.ID)
                throw new RepositoryItemMismatchException();

            _context.Entry(stance).State = EntityState.Modified;

            try
            {
                return (await _context.SaveChangesAsync() >= 0);
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteStance(int id)
        {
            var item = await GetStance(id);
            if (item == null)
                throw new RepositoryItemNotFoundException();

            _context.Stances.Remove(item);
            return (await _context.SaveChangesAsync() >= 0);
        }
        #endregion

        #region Moves
        public async Task<bool> MoveExists(int id)
        {
            return await _context.Moves.AnyAsync(x => x.ID == id);
        }

        public async Task<Move> GetMove(int id)
        {
            var item = await _context.Moves.FindAsync(id);

            if (item == null)
                return null;

            return item;
        }

        public async Task<Move> GetMove(Move move)
        {
            var item = await _context.Moves.Where(x => x.Name.ToLowerInvariant() == move.Name.ToLowerInvariant() &&
                                                       x.Symbol.ToLowerInvariant() == move.Symbol.ToLowerInvariant()).FirstOrDefaultAsync();

            return item;
        }

        public async Task<Move> GetMoveBySymbol(string symbol)
        {
            var item = await _context.Moves.Where(x => x.Symbol.ToLowerInvariant() == symbol.ToLowerInvariant()).FirstOrDefaultAsync();

            return item;
        }

        public async Task<List<Move>> GetAllMoves()
        {
            var itemsBeforeProcessing = _context.Moves.OrderBy(x => x.Name).AsQueryable();
            return await itemsBeforeProcessing.ToListAsync();
        }

        public async Task<PagedList<Move>> GetMoves(SieveModel sieveModel)
        {
            try
            {
                var items = _context.Moves.AsQueryable();

                var processedItems = _sieveProcessor.Apply(sieveModel, items, applyPagination: false);

                return await PagedList<Move>.Create(processedItems, sieveModel.Page.Value, sieveModel.PageSize.Value);
            }
            catch (Exception ex)
            {
                throw new RepositoryFilterException(ex.Message);
            }
        }

        public async Task<Move> AddMove(Move move)
        {
            var existingItem = await GetMove(move);

            if (existingItem != null)
                throw new RepositoryItemAlreadyExistsException();

            _context.Moves.Add(move);
            if (await _context.SaveChangesAsync() >= 0)
                return move;
            else
                return null;
        }

        public async Task<bool> UpdateMove(int id, Move move)
        {
            if (!await MoveExists(id))
                throw new RepositoryItemNotFoundException();

            if (move == null)
                return false;

            if (id != move.ID)
                throw new RepositoryItemMismatchException();

            _context.Entry(move).State = EntityState.Modified;

            try
            {
                return (await _context.SaveChangesAsync() >= 0);
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteMove(int id)
        {
            var item = await GetMove(id);
            if (item == null)
                throw new RepositoryItemNotFoundException();

            _context.Moves.Remove(item);
            return (await _context.SaveChangesAsync() >= 0);
        }
        #endregion

        #region Combinations
        public async Task<bool> CombinationExists(int id)
        {
            return await _context.Combinations.AnyAsync(x => x.ID == id);
        }

        public async Task<Combination> GetCombination(int id)
        {
            var item = await _context.Combinations.Where(x => x.ID == id)
                .Include(combo => combo.Motions)
                .Include("Motions.Stance")
                .Include("Motions.Move")
                .Include("Motions.Technique").FirstOrDefaultAsync();

            if (item == null)
                return null;

            return item;
        }

        public async Task<Combination> GetCombination(Combination combination)
        {
            var item = await _context.Combinations
                .Include(combo => combo.Motions)
                .Include("Motions.Stance")
                .Include("Motions.Move")
                .Include("Motions.Technique")
                //.Where(x => x.Hash == combination.Hash)
                .FirstOrDefaultAsync();

            return item;
        }

        public async Task<List<Combination>> GetAllCombinations()
        {
            var items = _context.Combinations
                .Include(x => x.Motions)
                .Include("Motions.Stance")
                .Include("Motions.Move")
                .Include("Motions.Technique")
                .OrderBy(x => x.ID)
                .AsQueryable();

            return await items.ToListAsync();
        }

        public async Task<PagedList<Combination>> GetCombinations(SieveModel sieveModel)
        {
            try
            {
                if (sieveModel.Filters != null && sieveModel.Filters.Length > 0)
                {
                    var motions = _context.Motions
                        .Include("Stance")
                        .Include("Move")
                        .Include("Technique")
                        .AsQueryable();

                    var filteredMotions = _sieveProcessor.Apply(sieveModel, motions, applyPagination: false);
                    var comboIds = filteredMotions.Select(x => x.CombinationId).Distinct().ToList();

                    var combos = _context.Combinations
                                        .Include(x => x.Motions)
                                        .Include("Motions.Stance")
                                        .Include("Motions.Move")
                                        .Include("Motions.Technique")
                                        .Where(x => comboIds.Contains(x.ID))
                                        .OrderBy(x => x.ID)
                                        .AsQueryable();

                    return await PagedList<Combination>.Create(combos, sieveModel.Page.Value, sieveModel.PageSize.Value);
                }
                else
                {
                    var combos = _context.Combinations
                                        .Include(x => x.Motions)
                                        .Include("Motions.Stance")
                                        .Include("Motions.Move")
                                        .Include("Motions.Technique")
                                        .OrderBy(x => x.ID)
                                        .AsQueryable();

                    return await PagedList<Combination>.Create(combos, sieveModel.Page.Value, sieveModel.PageSize.Value);
                }
            }
            catch (Exception ex)
            {
                throw new RepositoryFilterException(ex.Message);
            }
        }

        public async Task<Combination> AddCombination(Combination combination)
        {
            if (combination.Motions.Count <= 0)
                return null;

            foreach (var motion in combination.Motions)
            {
                Stance stance;
                Move move;
                Technique technique;

                if (motion.StanceId > 0)
                    stance = await GetStance(motion.StanceId);
                else
                    stance = await GetStance(motion.Stance);

                if (motion.MoveId > 0)
                    move = await GetMove(motion.MoveId);
                else
                    move = await GetMove(motion.Move);

                if (motion.TechniqueId > 0)
                    technique = await GetTechnique(motion.TechniqueId);
                else
                    technique = await GetTechnique(motion.Technique);

                if (stance == null || move == null || technique == null)
                {
                    string message = $"Invalid motion with SequenceNumber { motion.SequenceNumber}.\n";
                    message += stance == null ? "Stance is null\n" : "Stance is okay\n";
                    message += move == null ? "Move is null\n" : "Move is okay\n";
                    message += technique == null ? "Technique is null\n" : "Technique is okay\n";

                    throw new Exception(message);
                }

                motion.StanceId = stance.ID;
                motion.MoveId = move.ID;
                motion.TechniqueId = technique.ID;
            }

            _context.Combinations.Add(combination);
            if (await _context.SaveChangesAsync() >= 0)
                return combination;
            else
                return null;
        }

        public async Task<bool> DeleteCombination(int id)
        {
            var item = await GetCombination(id);
            if (item == null)
                throw new RepositoryItemNotFoundException();

            _context.Combinations.Remove(item);
            return (await _context.SaveChangesAsync() >= 0);
        }
        #endregion

        #region BeltTestPrograms
        public async Task<bool> BeltTestProgramExists(int id)
        {
            return await _context.BeltTestPrograms.AnyAsync(x => x.ID == id);
        }

        public async Task<BeltTestProgram> GetBeltTestProgram(int id)
        {
            var item = await _context.BeltTestPrograms.Where(x => x.ID == id)
                .Include(x => x.KihonCombinations)
                .Include("Combination")
                .Include("Motion")
                .Include("Motions.Stance")
                .Include("Motions.Move")
                .Include("Motions.Technique")
                .FirstOrDefaultAsync();

            if (item == null)
                return null;

            return item;
        }

        public async Task<BeltTestProgram> GetBeltTestProgram(BeltTestProgram program)
        {
            var item = await _context.BeltTestPrograms.Where(x => x.Name.ToLowerInvariant() == program.Name.ToLowerInvariant() &&
                                                       x.StyleName.ToLowerInvariant() == program.StyleName.ToLowerInvariant() &&
                                                       x.Graduation == program.Graduation &&
                                                       x.GraduationType == program.GraduationType
                                                       ).FirstOrDefaultAsync();

            return item;
        }

        public async Task<List<BeltTestProgram>> GetAllBeltTestPrograms()
        {
            var items = _context.BeltTestPrograms
                .Include(x => x.KihonCombinations)
                .Include("KihonCombinations.Motions")
                .Include("KihonCombinations.Motions.Stance")
                .Include("KihonCombinations.Motions.Move")
                .Include("KihonCombinations.Motions.Technique")
                .OrderBy(x => x.ID)
                .AsQueryable();

            return await items.ToListAsync();
        }

        public async Task<PagedList<BeltTestProgram>> GetBeltTestPrograms(SieveModel sieveModel)
        {
            // TODO: der Filter und das sortieren ist nicht komplett implementiert

            try
            {
                if (sieveModel.Filters != null && sieveModel.Filters.Length > 0)
                {
                    var motions = _context.Motions
                        .Include("Stance")
                        .Include("Move")
                        .Include("Technique")
                        .AsQueryable();

                    var filteredMotions = _sieveProcessor.Apply(sieveModel, motions, applyPagination: false);
                    var comboIds = filteredMotions.Select(x => x.CombinationId).Distinct().ToList();

                    var combos = _context.Combinations
                                        .Include(x => x.Motions)
                                        .Include("Motions.Stance")
                                        .Include("Motions.Move")
                                        .Include("Motions.Technique")
                                        .Where(x => comboIds.Contains(x.ID))
                                        .OrderBy(x => x.ID)
                                        .AsQueryable();

                    var programIds = combos.Select(x => x.ProgramId).Distinct().ToList();

                    var programs = _context.BeltTestPrograms
                                        .Include(x => x.KihonCombinations)
                                        .Include("KihonCombinations.Motions")
                                        .Include("KihonCombinations.Motions.Stance")
                                        .Include("KihonCombinations.Motions.Move")
                                        .Include("KihonCombinations.Motions.Technique")
                                        .Where(x => programIds.Contains(x.ID))
                                        .OrderBy(x => x.ID)
                                        .AsQueryable();
                    

                    return await PagedList<BeltTestProgram>.Create(programs, sieveModel.Page.Value, sieveModel.PageSize.Value);
                }
                else
                {
                    var programs = _context.BeltTestPrograms
                                        .Include(x => x.KihonCombinations)
                                        .Include("KihonCombinations.Motions")
                                        .Include("KihonCombinations.Motions.Stance")
                                        .Include("KihonCombinations.Motions.Move")
                                        .Include("KihonCombinations.Motions.Technique")
                                        .OrderBy(x => x.ID)
                                        .AsQueryable();

                    return await PagedList<BeltTestProgram>.Create(programs, sieveModel.Page.Value, sieveModel.PageSize.Value);
                }
            }
            catch (Exception ex)
            {
                throw new RepositoryFilterException(ex.Message);
            }
        }

        public async Task<BeltTestProgram> AddBeltTestProgram(BeltTestProgram program)
        {
            var existingItem = await GetBeltTestProgram(program);

            if (existingItem != null)
                throw new RepositoryItemAlreadyExistsException();


            foreach (var combination in program.KihonCombinations)
            {
                foreach (var motion in combination.Motions)
                {
                    Stance stance;
                    Move move;
                    Technique technique;

                    if (motion.StanceId > 0)
                        stance = await GetStance(motion.StanceId);
                    else
                        stance = await GetStance(motion.Stance);

                    if (motion.MoveId > 0)
                        move = await GetMove(motion.MoveId);
                    else
                        move = await GetMove(motion.Move);

                    if (motion.TechniqueId > 0)
                        technique = await GetTechnique(motion.TechniqueId);
                    else
                        technique = await GetTechnique(motion.Technique);

                    if (stance == null || move == null || technique == null)
                    {
                        string message = $"Invalid motion with SequenceNumber { motion.SequenceNumber}.\n";
                        message += stance == null ? "Stance is null\n" : "Stance is okay\n";
                        message += move == null ? "Move is null\n" : "Move is okay\n";
                        message += technique == null ? "Technique is null\n" : "Technique is okay\n";

                        throw new Exception(message);
                    }

                    motion.StanceId = stance.ID;
                    motion.MoveId = move.ID;
                    motion.TechniqueId = technique.ID;
                }
            }

            _context.BeltTestPrograms.Add(program);

            if (await _context.SaveChangesAsync() >= 0)
                return program;
            else
                return null;
        }

        public async Task<bool> DeleteBeltTestProgram(int id)
        {
            var item = await GetBeltTestProgram(id);
            if (item == null)
                throw new RepositoryItemNotFoundException();

            _context.BeltTestPrograms.Remove(item);
            return (await _context.SaveChangesAsync() >= 0);
        }
        #endregion
    }
}
