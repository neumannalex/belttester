using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBeltTestingProgram.Data;
using MyBeltTestingProgram.Data.Models;
using MyBeltTestingProgram.Data.Repositories;
using MyBeltTestingProgram.Entities;
using MyBeltTestingProgram.Entities.Move;
using MyBeltTestingProgram.Helpers;
using MyBeltTestingProgram.Services;
using Sieve.Models;

namespace MyBeltTestingProgram.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IDataRepository _repository;
        private readonly ISieveModelPreparer _sieveModelPreparer;
        private readonly IPagingLinkCreator _pagingLinkCreator;

        public MovesController(IMapper mapper, IDataRepository repository, ISieveModelPreparer sieveModelPreparer, IPagingLinkCreator pagingLinkCreator)
        {
            _mapper = mapper;
            _repository = repository;
            _sieveModelPreparer = sieveModelPreparer;
            _pagingLinkCreator = pagingLinkCreator;
        }

        [HttpGet(Name = "GetMoves")]
        public async Task<ActionResult<IEnumerable<MoveDTO>>> GetMoves([FromQuery]SieveModel sieveModel)
        {
            _sieveModelPreparer.SetMissingValues(ref sieveModel);

            try
            { 
                var itemsFromRepo = await _repository.GetMoves(sieveModel);

                var previousPageLink = itemsFromRepo.HasPrevious ? _pagingLinkCreator.CreatePreviousPageLink("GetMoves", sieveModel) : null;
                var nextPageLink = itemsFromRepo.HasNext ? _pagingLinkCreator.CreateNextPageLink("GetMoves", sieveModel) : null;

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

                var items = _mapper.Map<List<MoveDTO>>(itemsFromRepo);
                return Ok(items);
            }
            catch (RepositoryFilterException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}", Name = "GetMove")]
        public async Task<ActionResult<MoveDTO>> GetMove(int id)
        {
            var item = await _repository.GetMove(id);

            if (item == null)
                return NotFound();

            return Ok(_mapper.Map<MoveDTO>(item));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<MoveDTO>> PutMove(int id, [FromBody]MoveDTOForUpdate itemForUpdate)
        {
            var item = _mapper.Map<Move>(itemForUpdate);

            try
            {
                var success = await _repository.UpdateMove(id, item);

                if (success)
                    return Ok(_mapper.Map<MoveDTO>(item));
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
        public async Task<ActionResult<MoveDTO>> PatchMove(int id, [FromBody]JsonPatchDocument<MoveDTOForUpdate> itemPatch)
        {
            var item = await _repository.GetMove(id);
            if (item == null)
                return NotFound();

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            var itemDTO = _mapper.Map<MoveDTOForUpdate>(item);

            itemPatch.ApplyTo(itemDTO);
            _mapper.Map(itemDTO, item);

            try
            {
                var success = await _repository.UpdateMove(id, item);

                if (success)
                    return Ok(_mapper.Map<MoveDTO>(item));
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
        public async Task<ActionResult<MoveDTO>> PostMove([FromBody]MoveDTOForCreation itemForCreation)
        {
            if (itemForCreation == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return new UnprocessableEntityObjectResult(ModelState);

            var item = _mapper.Map<Move>(itemForCreation);

            try
            {
                var addedItem = await _repository.AddMove(item);
                if (addedItem == null)
                    return BadRequest("Saving item failed.");
                else
                    return CreatedAtAction("GetMove", new { id = item.ID }, _mapper.Map<MoveDTO>(addedItem));
            }
            catch (RepositoryItemAlreadyExistsException)
            {
                return BadRequest("Item already exists.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMove(int id)
        {
            try
            {
                var success = await _repository.DeleteMove(id);
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
