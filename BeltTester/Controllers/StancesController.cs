using AutoMapper;
using BeltTester.Data;
using BeltTester.Data.Entities;
using BeltTester.DTO;
using BeltTester.Helpers;
using BeltTester.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BeltTester.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StancesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IDataRepository _repository;
        private readonly ISieveModelPreparer _sieveModelPreparer;
        private readonly IPagingLinkCreator _pagingLinkCreator;

        public StancesController(IMapper mapper, IDataRepository repository, ISieveModelPreparer sieveModelPreparer, IPagingLinkCreator pagingLinkCreator)
        {
            _mapper = mapper;
            _repository = repository;
            _sieveModelPreparer = sieveModelPreparer;
            _pagingLinkCreator = pagingLinkCreator;
        }

        [HttpGet("all", Name = "GetAllStances")]
        public async Task<ActionResult<IEnumerable<StanceDTO>>> GetAllStances()
        {
            try
            {
                var itemsFromRepo = await _repository.GetAllStances();

                var items = _mapper.Map<List<StanceDTO>>(itemsFromRepo);
                return Ok(items);
            }
            catch (RepositoryFilterException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(Name = "GetStances")]
        public async Task<ActionResult<IEnumerable<StanceDTO>>> GetStances([FromQuery]SieveModel sieveModel)
        {
            _sieveModelPreparer.SetMissingValues(ref sieveModel);

            try
            {
                var itemsFromRepo = await _repository.GetStances(sieveModel);

                var previousPageLink = itemsFromRepo.HasPrevious ? _pagingLinkCreator.CreatePreviousPageLink("GetStances", sieveModel) : null;
                var nextPageLink = itemsFromRepo.HasNext ? _pagingLinkCreator.CreateNextPageLink("GetStances", sieveModel) : null;

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

                var items = _mapper.Map<List<StanceDTO>>(itemsFromRepo);
                return Ok(items);
            }
            catch (RepositoryFilterException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}", Name = "GetStance")]
        public async Task<ActionResult<StanceDTO>> GetStance(int id)
        {
            var item = await _repository.GetStance(id);

            if (item == null)
                return NotFound();

            return Ok(_mapper.Map<StanceDTO>(item));
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Manager")]
        public async Task<ActionResult<StanceDTO>> PutStance(int id, [FromBody]StanceDTOForUpdate itemForUpdate)
        {
            var item = _mapper.Map<Stance>(itemForUpdate);

            try
            {
                var success = await _repository.UpdateStance(id, item);

                if (success)
                    return Ok(_mapper.Map<StanceDTO>(item));
                else
                    return BadRequest("Updating item failed.");
            }
            catch (RepositoryItemNotFoundException)
            {
                return NotFound();
            }
            catch (RepositoryItemMismatchException)
            {
                return BadRequest("Id and ItemId do not match.");
            }
        }

        [HttpPatch("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Manager")]
        public async Task<ActionResult<StanceDTO>> PatchTechnique(int id, [FromBody]JsonPatchDocument<StanceDTOForUpdate> itemPatch)
        {
            var item = await _repository.GetStance(id);
            if (item == null)
                return NotFound();

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            var itemDTO = _mapper.Map<StanceDTOForUpdate>(item);

            itemPatch.ApplyTo(itemDTO);
            _mapper.Map(itemDTO, item);

            try
            {
                var success = await _repository.UpdateStance(id, item);

                if (success)
                    return Ok(_mapper.Map<StanceDTO>(item));
                else
                    return BadRequest("Updating item failed.");
            }
            catch (RepositoryItemNotFoundException)
            {
                return NotFound();
            }
            catch (RepositoryItemMismatchException)
            {
                return BadRequest("Id and ItemId do not match.");
            }
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Manager")]
        public async Task<ActionResult<StanceDTO>> PostStance([FromBody]StanceDTOForCreation itemForCreation)
        {
            if (itemForCreation == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return new UnprocessableEntityObjectResult(ModelState);

            var item = _mapper.Map<Stance>(itemForCreation);

            try
            {
                var addedItem = await _repository.AddStance(item);
                if (addedItem == null)
                    return BadRequest("Saving item failed.");
                else
                    return CreatedAtAction("GetStance", new { id = item.ID }, _mapper.Map<StanceDTO>(addedItem));
            }
            catch (RepositoryItemAlreadyExistsException)
            {
                return BadRequest("Technique already exists.");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Manager")]
        public async Task<IActionResult> DeleteStance(int id)
        {
            try
            {
                var success = await _repository.DeleteStance(id);
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