using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MyBeltTestingProgram.Data.Models;
using MyBeltTestingProgram.Entities;
using MyBeltTestingProgram.Entities.Technique;
using MyBeltTestingProgram.Helpers;
using MyBeltTestingProgram.Services;
using Sieve.Models;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBeltTestingProgram.Data.Repositories
{
    public class DataRepository : IDataRepository
    {
        private readonly MyBeltTestingDBContext _context;
        private readonly ISieveProcessor _sieveProcessor;
        public DataRepository(MyBeltTestingDBContext context, ISieveProcessor sieveProcessor)
        {
            _context = context;
            _sieveProcessor = sieveProcessor;            
        }

        #region Techniques
        private IQueryable<Technique> BuildFilterForTechniques(IQueryable<Technique> items, string query)
        {
            if (query.Contains("="))
            {
                var pairs = query.Split("&").ToList();
                foreach (var pair in pairs)
                {
                    var keyAndValue = pair.Split("=");
                    if (keyAndValue.Count() > 1)
                    {
                        var key = keyAndValue[0].Trim().ToLowerInvariant();
                        var value = keyAndValue[1].Trim().ToLowerInvariant();

                        switch (key)
                        {
                            case "name":
                                items = items.Where(x => x.Name.ToLowerInvariant().Contains(value));
                                break;
                            case "annotation":
                                items = items.Where(x => x.Annotation.ToLowerInvariant().Contains(value));
                                break;
                            case "purpose":
                                items = items.Where(x => x.Purpose.ToString().ToLowerInvariant().Contains(value));
                                break;
                            case "weapon":
                                items = items.Where(x => x.Weapon.ToString().ToLowerInvariant().Contains(value));
                                break;
                        }
                    }
                }
            }
            else
            {
                items = items.Where(x => x.Name.ToLowerInvariant().Contains(query) ||
                                         x.Annotation.ToLowerInvariant().Contains(query) ||
                                         x.Purpose.ToString().ToLowerInvariant().Contains(query) ||
                                         x.Weapon.ToString().ToLowerInvariant().Contains(query));
            }

            return items;
        }

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

        public async Task<PagedList<Technique>> GetAllTechniques(QueryResourceParameters parameters)
        {
            var itemsBeforeProcessing = _context.Techniques.OrderBy(x => x.Name).AsQueryable();

            return await PagedList<Technique>.Create(itemsBeforeProcessing, parameters.PageNumber, parameters.PageSize);
        }

        public async Task<PagedList<Technique>> GetTechniques(QueryResourceParameters parameters)
        {
            var itemsBeforeProcessing = _context.Techniques.OrderBy(x => x.Name).AsQueryable();

            if(!string.IsNullOrEmpty(parameters.SearchQuery))
            {
                var query = parameters.SearchQuery.Trim().ToLowerInvariant();
                itemsBeforeProcessing = BuildFilterForTechniques(itemsBeforeProcessing, parameters.SearchQuery);
            }

            return await PagedList<Technique>.Create(itemsBeforeProcessing, parameters.PageNumber, parameters.PageSize);
        }

        public async Task<bool> AddTechnique(Technique technique)
        {
            var existingTechnique = await _context.Techniques.Where(x => x.Name.ToLower() == technique.Name.ToLower() &&
                                                                    x.Annotation.ToLower() == technique.Annotation.ToLower() &&
                                                                    x.Purpose == technique.Purpose &&
                                                                    x.Weapon == technique.Weapon).FirstOrDefaultAsync();

            if (existingTechnique != null)
                throw new RepositoryItemAlreadyExistsException();

            _context.Techniques.Add(technique);
            return (await _context.SaveChangesAsync() >= 0);
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
        private IQueryable<Stance> BuildFilterForStances(IQueryable<Stance> items, string query)
        {
            if (query.Contains("="))
            {
                var pairs = query.Split("&").ToList();
                foreach (var pair in pairs)
                {
                    var keyAndValue = pair.Split("=");
                    if (keyAndValue.Count() > 1)
                    {
                        var key = keyAndValue[0].Trim().ToLowerInvariant();
                        var value = keyAndValue[1].Trim().ToLowerInvariant();

                        switch (key)
                        {
                            case "name":
                                items = items.Where(x => x.Name.ToLowerInvariant().Contains(value));
                                break;
                            case "symbol":
                                items = items.Where(x => x.Symbol.ToLowerInvariant().Contains(value));
                                break;
                        }
                    }
                }
            }
            else
            {
                items = items.Where(x => x.Name.ToLowerInvariant().Contains(query) ||
                                         x.Symbol.ToLowerInvariant().Contains(query));
            }

            return items;
        }

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

        public async Task<PagedList<Stance>> GetAllStances(QueryResourceParameters parameters)
        {
            var itemsBeforeProcessing = _context.Stances.OrderBy(x => x.Name).AsQueryable();

            return await PagedList<Stance>.Create(itemsBeforeProcessing, parameters.PageNumber, parameters.PageSize);
        }

        public async Task<PagedList<Stance>> GetStances(QueryResourceParameters parameters)
        {
            var itemsBeforeProcessing = _context.Stances.OrderBy(x => x.Name).AsQueryable();

            if (!string.IsNullOrEmpty(parameters.SearchQuery))
            {
                var query = parameters.SearchQuery.Trim().ToLowerInvariant();
                itemsBeforeProcessing = BuildFilterForStances(itemsBeforeProcessing, parameters.SearchQuery);
            }

            return await PagedList<Stance>.Create(itemsBeforeProcessing, parameters.PageNumber, parameters.PageSize);
        }

        public async Task<bool> AddStance(Stance stance)
        {
            var existingItem = await _context.Stances.Where(x => x.Name.ToLower() == stance.Name.ToLower() &&
                                                                    x.Symbol.ToLower() == stance.Symbol.ToLower()).FirstOrDefaultAsync();

            if (existingItem != null)
                throw new RepositoryItemAlreadyExistsException();

            _context.Stances.Add(stance);
            return (await _context.SaveChangesAsync() >= 0);
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
        private IQueryable<Move> BuildFilterForMoves(IQueryable<Move> items, string query)
        {
            if (query.Contains("="))
            {
                var pairs = query.Split("&").ToList();
                foreach (var pair in pairs)
                {
                    var keyAndValue = pair.Split("=");
                    if (keyAndValue.Count() > 1)
                    {
                        var key = keyAndValue[0].Trim().ToLowerInvariant();
                        var value = keyAndValue[1].Trim().ToLowerInvariant();

                        switch (key)
                        {
                            case "name":
                                items = items.Where(x => x.Name.ToLowerInvariant().Contains(value));
                                break;
                            case "symbol":
                                items = items.Where(x => x.Symbol.ToLowerInvariant().Contains(value));
                                break;
                        }
                    }
                }
            }
            else
            {
                items = items.Where(x => x.Name.ToLowerInvariant().Contains(query) ||
                                         x.Symbol.ToLowerInvariant().Contains(query));
            }

            return items;
        }

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

        public async Task<PagedList<Move>> GetAllMoves(QueryResourceParameters parameters)
        {
            var itemsBeforeProcessing = _context.Moves.OrderBy(x => x.Name).AsQueryable();

            return await PagedList<Move>.Create(itemsBeforeProcessing, parameters.PageNumber, parameters.PageSize);
        }

        public async Task<PagedList<Move>> GetMoves(QueryResourceParameters parameters)
        {
            var itemsBeforeProcessing = _context.Moves.OrderBy(x => x.Name).AsQueryable();

            if (!string.IsNullOrEmpty(parameters.SearchQuery))
            {
                var query = parameters.SearchQuery.Trim().ToLowerInvariant();
                itemsBeforeProcessing = BuildFilterForMoves(itemsBeforeProcessing, parameters.SearchQuery);
            }

            return await PagedList<Move>.Create(itemsBeforeProcessing, parameters.PageNumber, parameters.PageSize);
        }

        public async Task<bool> AddMove(Move move)
        {
            var existingItem = await _context.Stances.Where(x => x.Name.ToLower() == move.Name.ToLower() &&
                                                                    x.Symbol.ToLower() == move.Symbol.ToLower()).FirstOrDefaultAsync();

            if (existingItem != null)
                throw new RepositoryItemAlreadyExistsException();

            _context.Moves.Add(move);
            return (await _context.SaveChangesAsync() >= 0);
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
        private IQueryable<Combination> BuildFilterForCombinations(IQueryable<Combination> items, string query)
        {
            if (query.Contains("="))
            {
                var pairs = query.Split("&").ToList();
                foreach (var pair in pairs)
                {
                    var keyAndValue = pair.Split("=");
                    if (keyAndValue.Count() > 1)
                    {
                        var key = keyAndValue[0].Trim().ToLowerInvariant();
                        var value = keyAndValue[1].Trim().ToLowerInvariant();

                        switch (key)
                        {
                            case "stance":
                                items = items.Where(x => x.Motions.Where(y => y.Stance.Name.ToLowerInvariant().Contains(value) ||
                                                                              y.Stance.Symbol.ToLowerInvariant().Contains(value)).Count() > 0);
                                break;
                            case "move":
                                items = items.Where(x => x.Motions.Where(y => y.Move.Name.ToLowerInvariant().Contains(value) ||
                                                                              y.Move.Symbol.ToLowerInvariant().Contains(value)).Count() > 0);
                                break;
                            case "technique":
                                items = items.Where(x => x.Motions.Where(y => y.Technique.Name.ToLowerInvariant().Contains(value) ||
                                                                              y.Technique.Purpose.ToString().ToLowerInvariant().Contains(value) ||
                                                                              y.Technique.Weapon.ToString().ToLowerInvariant().Contains(value)).Count() > 0);
                                break;
                        }
                    }
                }
            }
            else
            {
                query = query.Trim().ToLowerInvariant();

                items = items.Where(x => x.Motions.Where(y => y.Technique.Name.ToLowerInvariant().Contains(query) ||
                                                              y.Technique.Purpose.ToString().ToLowerInvariant().Contains(query) ||
                                                              y.Technique.Weapon.ToString().ToLowerInvariant().Contains(query) ||
                                                              y.Stance.Name.ToLowerInvariant().Contains(query) ||
                                                              y.Stance.Symbol.ToLowerInvariant().Contains(query) ||
                                                              y.Move.Name.ToLowerInvariant().Contains(query) ||
                                                              y.Move.Symbol.ToLowerInvariant().Contains(query)).Count() > 0);
            }

            return items;
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

        public async Task<PagedList<Combination>> GetAllCombinations(QueryResourceParameters parameters)
        {
            var items = _context.Combinations
                .Include(x => x.Motions)
                .Include("Motions.Stance")
                .Include("Motions.Move")
                .Include("Motions.Technique")
                .OrderBy(x => x.ID)
                .AsQueryable();

            return await PagedList<Combination>.Create(items, parameters.PageNumber, parameters.PageSize);
        }

        public async Task<PagedList<Combination>> GetCombinations(QueryResourceParameters parameters)
        {
            var items = _context.Combinations
                .Include(x => x.Motions)
                .Include("Motions.Stance")
                .Include("Motions.Move")
                .Include("Motions.Technique")
                .OrderBy(x => x.ID)
                .AsQueryable();

            if (!string.IsNullOrEmpty(parameters.SearchQuery))
            {
                var query = parameters.SearchQuery.Trim().ToLowerInvariant();
                items = BuildFilterForCombinations(items, parameters.SearchQuery);
            }

            return await PagedList<Combination>.Create(items, parameters.PageNumber, parameters.PageSize);
        }

        public async Task<PagedList<Combination>> GetCombinations(SieveModel sieveModel)
        {
            var motions = _context.Motions
                .Include("Stance")
                .Include("Move")
                .Include("Technique")
                .AsQueryable();

            try
            {
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
            catch(Exception ex)
            {
                //Console.WriteLine(ex.Message);
                return null;
            }
        }
        #endregion
    }
}
