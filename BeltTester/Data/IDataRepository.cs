using BeltTester.Data.Entities;
using BeltTester.Helpers;
using Sieve.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeltTester.Data
{
    public interface IDataRepository
    {
        Task<bool> TechniqueExists(int id);
        Task<Technique> GetTechnique(int id);
        Task<Technique> GetTechnique(Technique technique);
        Task<IEnumerable<Technique>> GetTechniquesByName(string name);
        Task<List<Technique>> GetAllTechniques();
        Task<PagedList<Technique>> GetTechniques(SieveModel sieveModel);
        Task<Technique> AddTechnique(Technique technique);
        Task<bool> UpdateTechnique(int id, Technique technique);
        Task<bool> DeleteTechnique(int id);

        Task<bool> StanceExists(int id);
        Task<Stance> GetStance(int id);
        Task<Stance> GetStance(Stance stance);
        Task<Stance> GetStanceBySymbol(string symbol);
        Task<List<Stance>> GetAllStances();
        Task<PagedList<Stance>> GetStances(SieveModel sieveModel);
        Task<Stance> AddStance(Stance stance);
        Task<bool> UpdateStance(int id, Stance stance);
        Task<bool> DeleteStance(int id);

        Task<bool> MoveExists(int id);
        Task<Move> GetMove(int id);
        Task<Move> GetMove(Move move);
        Task<Move> GetMoveBySymbol(string symbol);
        Task<List<Move>> GetAllMoves();
        Task<PagedList<Move>> GetMoves(SieveModel sieveModel);
        Task<Move> AddMove(Move move);
        Task<bool> UpdateMove(int id, Move move);
        Task<bool> DeleteMove(int id);

        Task<Combination> GetCombination(int id);
        Task<Combination> GetCombination(Combination combination);
        Task<List<Combination>> GetAllCombinations();
        Task<PagedList<Combination>> GetCombinations(SieveModel sieveModel);
        Task<Combination> AddCombination(Combination combination);
        Task<bool> DeleteCombination(int id);
    }
}
