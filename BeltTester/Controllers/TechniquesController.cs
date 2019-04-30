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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeltTester.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TechniquesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IDataRepository _repository;
        private readonly ISieveModelPreparer _sieveModelPreparer;
        private readonly IPagingLinkCreator _pagingLinkCreator;

        public TechniquesController(IMapper mapper, IDataRepository repository, ISieveModelPreparer sieveModelPreparer, IPagingLinkCreator pagingLinkCreator)
        {
            _mapper = mapper;
            _repository = repository;
            _sieveModelPreparer = sieveModelPreparer;
            _pagingLinkCreator = pagingLinkCreator;
        }

        [HttpGet("all", Name = "GetAllTechniques")]
        public async Task<ActionResult<IEnumerable<TechniqueDTO>>> GetAllTechniques()
        {
            try
            {
                var itemsFromRepo = await _repository.GetAllTechniques();

                var items = _mapper.Map<List<TechniqueDTO>>(itemsFromRepo);
                return Ok(items);
            }
            catch (RepositoryFilterException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(Name = "GetTechniques")]
        public async Task<ActionResult<IEnumerable<TechniqueDTO>>> GetTechniques([FromQuery]SieveModel sieveModel)
        {
            _sieveModelPreparer.SetMissingValues(ref sieveModel);

            try
            {
                var itemsFromRepo = await _repository.GetTechniques(sieveModel);

                var previousPageLink = itemsFromRepo.HasPrevious ? _pagingLinkCreator.CreatePreviousPageLink("GetTechniques", sieveModel) : null;
                var nextPageLink = itemsFromRepo.HasNext ? _pagingLinkCreator.CreateNextPageLink("GetTechniques", sieveModel) : null;

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

                var items = _mapper.Map<List<TechniqueDTO>>(itemsFromRepo);
                return Ok(items);
            }
            catch (RepositoryFilterException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}", Name = "GetTechnique")]
        public async Task<ActionResult<TechniqueDTO>> GetTechnique(int id)
        {
            var item = await _repository.GetTechnique(id);

            if (item == null)
                return NotFound();

            return Ok(_mapper.Map<TechniqueDTO>(item));
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Manager")]
        public async Task<ActionResult<TechniqueDTO>> PutTechnique(int id, [FromBody]TechniqueDTOForUpdate itemForUpdate)
        {
            var item = _mapper.Map<Technique>(itemForUpdate);

            try
            {
                var success = await _repository.UpdateTechnique(id, item);

                if (success)
                    return Ok(_mapper.Map<TechniqueDTO>(item));
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
        public async Task<ActionResult<TechniqueDTO>> PatchTechnique(int id, [FromBody]JsonPatchDocument<TechniqueDTOForUpdate> itemPatch)
        {
            var item = await _repository.GetTechnique(id);
            if (item == null)
                return NotFound();

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            var itemDTO = _mapper.Map<TechniqueDTOForUpdate>(item);

            itemPatch.ApplyTo(itemDTO);
            _mapper.Map(itemDTO, item);

            try
            {
                var success = await _repository.UpdateTechnique(id, item);

                if (success)
                    return Ok(_mapper.Map<TechniqueDTO>(item));
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
        public async Task<ActionResult<TechniqueDTO>> PostTechnique([FromBody]TechniqueDTOForCreation itemForCreation)
        {
            if (itemForCreation == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return new UnprocessableEntityObjectResult(ModelState);

            var item = _mapper.Map<Technique>(itemForCreation);

            try
            {
                var addedItem = await _repository.AddTechnique(item);
                if (addedItem == null)
                    return BadRequest("Saving item failed.");
                else
                    return CreatedAtAction("GetTechnique", new { id = item.ID }, _mapper.Map<TechniqueDTO>(addedItem));
            }
            catch (RepositoryItemAlreadyExistsException)
            {
                return BadRequest("Technique already exists.");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Manager")]
        public async Task<IActionResult> DeleteTechnique(int id)
        {
            try
            {
                var success = await _repository.DeleteTechnique(id);
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
