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
        public async Task<ActionResult<IEnumerable<CombinationDTO>>> Test([FromQuery]SieveModel sieveModel)
        {
            _sieveModelPreparer.SetMissingValues(ref sieveModel);

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

        // GET: api/Combinations
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<CombinationDTO>>> GetCombinations([FromQuery]QueryResourceParameters parameters)
        //{
        //    var itemsFromRepo = await _repository.GetCombinations(parameters);

        //    var previousPageLink = itemsFromRepo.HasPrevious ?
        //        CreateCombinationsResourceUri(parameters,
        //        ResourceUriType.PreviousPage) : null;

        //    var nextPageLink = itemsFromRepo.HasNext ?
        //        CreateCombinationsResourceUri(parameters,
        //        ResourceUriType.NextPage) : null;

        //    var paginationMetadata = new PaginationMetadata
        //    {
        //        TotalCount = itemsFromRepo.TotalCount,
        //        PageSize = itemsFromRepo.PageSize,
        //        CurrentPage = itemsFromRepo.CurrentPage,
        //        TotalPages = itemsFromRepo.TotalPages,
        //        PreviousPageLink = previousPageLink,
        //        NextPageLink = nextPageLink
        //    };

        //    Response.Headers.Add("X-Pagination", Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

        //    var items = _mapper.Map<List<CombinationDTO>>(itemsFromRepo);
        //    return Ok(items);
        //}

        // GET: api/Combinations/5
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

        // GET: api/Combinations/5
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

        // PUT: api/Combinations/5
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

        // POST: api/Combinations
        [HttpPost]
        public async Task<ActionResult<CombinationDTO>> PostCombination(CombinationDTOForCreation combinationForCreation)
        {
            if (combinationForCreation.Motions.Count <= 0)
                return BadRequest("No motions given for combination.");

            Combination combination = new Combination();

            foreach (var motion in combinationForCreation.Motions)
            {
                var stance = await _context.Stances.Where(x => x.ID == motion.StanceId).FirstOrDefaultAsync();
                var move = await _context.Moves.Where(x => x.ID == motion.MoveId).FirstOrDefaultAsync();
                var technique = await _context.Techniques.Where(x => x.ID == motion.TechniqueId).FirstOrDefaultAsync();

                if (stance == null || move == null || technique == null)
                    return BadRequest("Motion specified incorrect.");

                combination.Motions.Add(new Motion
                {
                    Stance = stance,
                    Move = move,
                    Technique = technique
                });
            }

            _context.Combinations.Add(combination);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCombination", new { id = combination.ID }, _mapper.Map<CombinationDTO>(combination));
        }

        // DELETE: api/Combinations/5
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
