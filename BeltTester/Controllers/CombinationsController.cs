using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BeltTester.Data;
using BeltTester.Data.Entities;
using BeltTester.DTO;
using BeltTester.Helpers;
using BeltTester.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;

namespace BeltTester.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CombinationsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IDataRepository _repository;
        private readonly ISieveModelPreparer _sieveModelPreparer;
        private readonly IPagingLinkCreator _pagingLinkCreator;

        public CombinationsController(IMapper mapper, IDataRepository repository, ISieveModelPreparer sieveModelPreparer, IPagingLinkCreator pagingLinkCreator)
        {
            _mapper = mapper;
            _repository = repository;
            _sieveModelPreparer = sieveModelPreparer;
            _pagingLinkCreator = pagingLinkCreator;
        }

        [HttpGet("all", Name = "GetAllCombinations")]
        public async Task<ActionResult<IEnumerable<CombinationDTO>>> GetAllCombinations()
        {
            try
            {
                var itemsFromRepo = await _repository.GetAllCombinations();

                var items = _mapper.Map<List<CombinationDTO>>(itemsFromRepo);
                return Ok(items);
            }
            catch (RepositoryFilterException ex)
            {
                return BadRequest(ex.Message);
            }
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
            catch (RepositoryFilterException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}", Name = "GetCombination")]
        public async Task<ActionResult<CombinationDTO>> GetCombination(int id)
        {
            var item = await _repository.GetCombination(id);

            if (item == null)
                return NotFound();

            return Ok(_mapper.Map<CombinationDTO>(item));
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<CombinationDTO>> PostCombination([FromBody]CombinationDTOForCreation itemForCreation)
        {
            if (itemForCreation == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return new UnprocessableEntityObjectResult(ModelState);

            var item = _mapper.Map<Combination>(itemForCreation);

            try
            {
                var addedItem = await _repository.AddCombination(item);
                if (addedItem == null)
                    return BadRequest("Saving item failed.");
                else
                    return CreatedAtAction("GetCombination", new { id = item.ID }, _mapper.Map<CombinationDTO>(addedItem));
            }
            catch (RepositoryItemAlreadyExistsException)
            {
                return BadRequest("Item already exists.");
            }
        }

        [HttpPost("create")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<CombinationDTO>> CreateCombination([FromBody]CombinationDTOForCreation itemForCreation)
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

        [HttpPost("createwithids")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<CombinationDTO>> CreateCombinationWithIds([FromBody]CombinationDTOForCreationWithIds itemForCreation)
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteMove(int id)
        {
            try
            {
                var success = await _repository.DeleteCombination(id);
                if (success)
                    return NoContent();
                else
                    return BadRequest("Internal error deleting item.");

            }
            catch (RepositoryItemNotFoundException)
            {
                return NotFound();
            }
        }
    }
}