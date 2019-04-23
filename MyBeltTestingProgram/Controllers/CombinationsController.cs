using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBeltTestingProgram.Data;
using MyBeltTestingProgram.Data.Models;
using MyBeltTestingProgram.Data.Repositories;
using MyBeltTestingProgram.Entities;
using MyBeltTestingProgram.Entities.Combination;
using MyBeltTestingProgram.Helpers;
using MyBeltTestingProgram.Services;
using Sieve.Models;

namespace MyBeltTestingProgram.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CombinationsController : ControllerBase
    {
        private readonly MyBeltTestingDBContext _context;
        private readonly IMapper _mapper;
        private readonly IDataRepository _repository;
        private readonly ISieveModelPreparer _sieveModelPreparer;
        private readonly IPagingLinkCreator _pagingLinkCreator;
        

        public CombinationsController(MyBeltTestingDBContext context, IMapper mapper, IDataRepository repository, ISieveModelPreparer sieveModelPreparer, IPagingLinkCreator pagingLinkCreator)
        {
            _context = context;
            _mapper = mapper;
            _repository = repository;
            _sieveModelPreparer = sieveModelPreparer;
            _pagingLinkCreator = pagingLinkCreator;
        }

        [HttpGet(Name = "GetCombinations")]
        public async Task<ActionResult<IEnumerable<CombinationDTO>>> GetCombinations([FromQuery]SieveModel sieveModel)
        {
            _sieveModelPreparer.SetMissingValues(ref sieveModel);

            try
            {
                var itemsFromRepo = await _repository.GetCombinations(sieveModel);

                var previousPageLink = itemsFromRepo.HasPrevious ? _pagingLinkCreator.CreatePreviousPageLink("GetCombinations", sieveModel) : null;
                var nextPageLink = itemsFromRepo.HasNext ? _pagingLinkCreator.CreateNextPageLink("GetCombinations", sieveModel) : null;

                var paginationMetadata = new PaginationMetadata
                {
                    TotalCount = itemsFromRepo.TotalCount,
                    PageSize = itemsFromRepo.PageSize,
                    CurrentPage = itemsFromRepo.CurrentPage,
                    TotalPages = itemsFromRepo.TotalPages,
                    PreviousPageLink = previousPageLink,
                    NextPageLink = nextPageLink
                };

                Response.Headers.Add("X-Pagination", Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

                var items = _mapper.Map<List<CombinationDTO>>(itemsFromRepo);
                return Ok(items);
            }
            catch(RepositoryFilterException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CombinationDTO>> GetCombination(int id)
        {
            var combination = await _context.Combinations.Where(x => x.ID == id)
                .Include(combo => combo.Motions)
                .Include("Motions.Stance")
                .Include("Motions.Move")
                .Include("Motions.Technique").FirstOrDefaultAsync();

            if (combination == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<CombinationDTO>(combination));
        }

        [HttpGet("{id}/pretty")]
        public async Task<ActionResult<string>> GetPrettyCombination(int id)
        {
            var combination = await _context.Combinations.Where(x => x.ID == id)
                .Include(combo => combo.Motions)
                .Include("Motions.Stance")
                .Include("Motions.Move")
                .Include("Motions.Technique").FirstOrDefaultAsync();

            if (combination == null)
            {
                return NotFound();
            }

            return Ok(combination.ToString());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCombination(int id, Combination combination)
        {
            if (id != combination.ID)
            {
                return BadRequest();
            }

            _context.Entry(combination).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CombinationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<CombinationDTO>> PostCombination(CombinationDTO itemForCreation)
        {
            if (itemForCreation == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return new UnprocessableEntityObjectResult(ModelState);

            if (itemForCreation.Motions.Count <= 0)
                return BadRequest("No motions given for combination.");

            var item = _mapper.Map<Combination>(itemForCreation);

            Combination combination = new Combination();

            foreach (var motion in item.Motions)
            {
                Stance stance;
                if (motion.Stance.ID > 0)
                    stance = await _repository.GetStance(motion.Stance.ID);
                else
                    stance = await _repository.GetStance(_mapper.Map<Stance>(motion.Stance));

                if (stance == null)
                    stance = await _repository.AddStance(stance);

                Move move;
                if (motion.Move.ID > 0)
                    move = await _repository.GetMove(motion.Move.ID);
                else
                    move = await _repository.GetMove(_mapper.Map<Move>(motion.Move));

                if (move == null)
                    move = await _repository.AddMove(move);

                Technique technique;
                if (motion.Technique.ID > 0)
                    technique = await _repository.GetTechnique(motion.Technique.ID);
                else
                    technique = await _repository.GetTechnique(_mapper.Map<Technique>(motion.Technique));

                if (technique == null)
                    technique = await _repository.AddTechnique(technique);

                if (stance == null || move == null || technique == null)
                    return BadRequest("Motion specified incorrect.");

                motion.Stance = stance;
                motion.Move = move;
                motion.Technique = technique;
            }

            combination = await _repository.AddCombination(item);
            if(combination == null)
                return BadRequest("Combination already exists.");
            else
                return CreatedAtAction("GetCombination", new { id = combination.ID }, _mapper.Map<CombinationDTO>(combination));
        }

        [HttpPost("create")]
        public async Task<ActionResult<CombinationDTO>> CreateCombination(CombinationDTOForCreation itemForCreation)
        {
            if (itemForCreation == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return new UnprocessableEntityObjectResult(ModelState);

            if (itemForCreation.Motions.Count <= 0)
                return BadRequest("No motions given for combination.");


            var combination = new Combination();

            foreach (var motion in itemForCreation.Motions)
            {
                var stance = await _repository.GetStanceBySymbol(motion.StanceSymbol);
                var move = await _repository.GetMoveBySymbol(motion.MoveSymbol);
                var technique = (await _repository.GetTechniquesByName(motion.TechniqueName)).FirstOrDefault();

                if (stance == null || move == null || technique == null)
                    return BadRequest("Stance, Move or Technique not found.");

                combination.Motions.Add(new Motion {
                    Stance = stance,
                    Move = move,
                    Technique = technique
                });
            }

            try
            {
                combination = await _repository.AddCombination(combination);
                if (combination == null)
                    return BadRequest("Error saving Combination.");
                else
                    return CreatedAtAction("GetCombination", new { id = combination.ID }, _mapper.Map<CombinationDTO>(combination));
            }
            catch(RepositoryItemAlreadyExistsException)
            {
                return BadRequest("Combination already exists.");
            }
        }

        [HttpPost("createwithids")]
        public async Task<ActionResult<CombinationDTO>> CreateCombinationWithIds(CombinationDTOForCreationWithIds itemForCreation)
        {
            if (itemForCreation == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return new UnprocessableEntityObjectResult(ModelState);

            if (itemForCreation.Motions.Count <= 0)
                return BadRequest("No motions given for combination.");

            var combination = new Combination();

            foreach (var motion in itemForCreation.Motions)
            {
                var stance = await _repository.GetStance(motion.StanceId);
                var move = await _repository.GetMove(motion.MoveId);
                var technique = await _repository.GetTechnique(motion.TechniqueId);

                if (stance == null || move == null || technique == null)
                    return BadRequest("Stance, Move or Technique not found.");

                combination.Motions.Add(new Motion
                {
                    Stance = stance,
                    Move = move,
                    Technique = technique
                });
            }

            try
            {
                combination = await _repository.AddCombination(combination);
                if (combination == null)
                    return BadRequest("Error saving Combination.");
                else
                    return CreatedAtAction("GetCombination", new { id = combination.ID }, _mapper.Map<CombinationDTO>(combination));
            }
            catch (RepositoryItemAlreadyExistsException)
            {
                return BadRequest("Combination already exists.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Combination>> DeleteCombination(int id)
        {
            var combination = await _context.Combinations.FindAsync(id);
            if (combination == null)
            {
                return NotFound();
            }

            _context.Combinations.Remove(combination);
            await _context.SaveChangesAsync();

            return combination;
        }

        private bool CombinationExists(int id)
        {
            return _context.Combinations.Any(e => e.ID == id);
        }
    }
}
