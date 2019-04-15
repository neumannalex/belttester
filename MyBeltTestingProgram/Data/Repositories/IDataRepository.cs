using MyBeltTestingProgram.Data.Models;
using MyBeltTestingProgram.Entities;
using MyBeltTestingProgram.Entities.Technique;
using MyBeltTestingProgram.Helpers;
using Sieve.Models;
using System.Threading.Tasks;

namespace MyBeltTestingProgram.Data.Repositories
{
    public interface IDataRepository
    {
        Task<bool> TechniqueExists(int id);
        Task<Technique> GetTechnique(int id);
        Task<PagedList<Technique>> GetAllTechniques(QueryResourceParameters parameters);
        Task<PagedList<Technique>> GetTechniques(QueryResourceParameters parameters);
        Task<bool> AddTechnique(Technique technique);
        Task<bool> UpdateTechnique(int id, Technique technique);
        Task<bool> DeleteTechnique(int id);

        Task<bool> StanceExists(int id);
        Task<Stance> GetStance(int id);
        Task<PagedList<Stance>> GetAllStances(QueryResourceParameters parameters);
        Task<PagedList<Stance>> GetStances(QueryResourceParameters parameters);
        Task<bool> AddStance(Stance stance);
        Task<bool> UpdateStance(int id, Stance stance);
        Task<bool> DeleteStance(int id);

        Task<bool> MoveExists(int id);
        Task<Move> GetMove(int id);
        Task<PagedList<Move>> GetAllMoves(QueryResourceParameters parameters);
        Task<PagedList<Move>> GetMoves(QueryResourceParameters parameters);
        Task<bool> AddMove(Move move);
        Task<bool> UpdateMove(int id, Move move);
        Task<bool> DeleteMove(int id);

        Task<Combination> GetCombination(int id);
        Task<PagedList<Combination>> GetAllCombinations(QueryResourceParameters parameters);
        Task<PagedList<Combination>> GetCombinations(QueryResourceParameters parameters);
        Task<PagedList<Combination>> GetCombinations(SieveModel sieveModel);
    }
}