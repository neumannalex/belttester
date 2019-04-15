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

namespace MyBeltTestingProgram.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IDataRepository _repository;
        private IUrlHelper _urlHelper;

        public MovesController(IMapper mapper, IDataRepository repository, IUrlHelper urlHelper)
        {
            _mapper = mapper;
            _repository = repository;
            _urlHelper = urlHelper;
        }

        private string CreateMovesResourceUri(QueryResourceParameters parameters, ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return _urlHelper.Link("GetMoves",
                        new
                        {
                            searchQuery = parameters.SearchQuery,
                            pageNumber = parameters.PageNumber - 1,
                            pageSize = parameters.PageSize
                        });
                case ResourceUriType.NextPage:
                    return _urlHelper.Link("GetMoves",
                        new
                        {
                            searchQuery = parameters.SearchQuery,
                            pageNumber = parameters.PageNumber + 1,
                            pageSize = parameters.PageSize
                        });

                default:
                    return _urlHelper.Link("GetMoves",
                    new
                    {
                        searchQuery = parameters.SearchQuery,
                        pageNumber = parameters.PageNumber,
                        pageSize = parameters.PageSize
                    });
            }
        }

        // GET: api/Moves
        [HttpGet(Name = "GetMoves")]
        public async Task<ActionResult<IEnumerable<MoveDTO>>> GetMoves([FromQuery]QueryResourceParameters parameters)
        {
            var itemsFromRepo = await _repository.GetMoves(parameters);

            var previousPageLink = itemsFromRepo.HasPrevious ?
                CreateMovesResourceUri(parameters,
                ResourceUriType.PreviousPage) : null;

            var nextPageLink = itemsFromRepo.HasNext ?
                CreateMovesResourceUri(parameters,
                ResourceUriType.NextPage) : null;

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

        // GET: api/Moves/5
        [HttpGet("{id}", Name = "GetMove")]
        public async Task<ActionResult<MoveDTO>> GetMove(int id)
        {
            var item = await _repository.GetMove(id);

            if (item == null)
                return NotFound();

            return Ok(_mapper.Map<MoveDTO>(item));
        }

        // PUT: api/Moves/5
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

        // POST: api/Moves
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
                var success = await _repository.AddMove(item);
                if (success)
                    return CreatedAtAction("GetMove", new { id = item.ID }, _mapper.Map<MoveDTO>(item));
                else
                    return BadRequest("Saving item failed.");
            }
            catch (RepositoryItemAlreadyExistsException)
            {
                return BadRequest("Item already exists.");
            }
        }

        // DELETE: api/Moves/5
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
