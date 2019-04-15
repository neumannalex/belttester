using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using MyBeltTestingProgram.Data.Models;
using MyBeltTestingProgram.Data.Repositories;
using MyBeltTestingProgram.Entities;
using MyBeltTestingProgram.Entities.Technique;
using MyBeltTestingProgram.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyBeltTestingProgram.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TechniquesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IDataRepository _repository;
        private IUrlHelper _urlHelper;

        public TechniquesController(IMapper mapper, IDataRepository repository, IUrlHelper urlHelper)
        {
            _mapper = mapper;
            _repository = repository;
            _urlHelper = urlHelper;
        }

        private string CreateTechniquesResourceUri(QueryResourceParameters parameters, ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return _urlHelper.Link("GetTechniques",
                        new
                        {
                            searchQuery = parameters.SearchQuery,
                            pageNumber = parameters.PageNumber - 1,
                            pageSize = parameters.PageSize
                        });
                case ResourceUriType.NextPage:
                    return _urlHelper.Link("GetTechniques",
                        new
                        {
                            searchQuery = parameters.SearchQuery,
                            pageNumber = parameters.PageNumber + 1,
                            pageSize = parameters.PageSize
                        });

                default:
                    return _urlHelper.Link("GetTechniques",
                    new
                    {
                        searchQuery = parameters.SearchQuery,
                        pageNumber = parameters.PageNumber,
                        pageSize = parameters.PageSize
                    });
            }
        }

        [HttpGet(Name = "GetTechniques")]
        public async Task<ActionResult<IEnumerable<TechniqueDTO>>> GetTechniques([FromQuery]QueryResourceParameters parameters)
        {
            var itemsFromRepo = await _repository.GetTechniques(parameters);

            var previousPageLink = itemsFromRepo.HasPrevious ?
                CreateTechniquesResourceUri(parameters,
                ResourceUriType.PreviousPage) : null;

            var nextPageLink = itemsFromRepo.HasNext ?
                CreateTechniquesResourceUri(parameters,
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

            var items = _mapper.Map<List<TechniqueDTO>>(itemsFromRepo);
            return Ok(items);
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
            catch(RepositoryItemNotFoundException)
            {
                return NotFound();
            }
            catch(RepositoryItemMismatchException)
            {
                return BadRequest("Id and ItemId do not match.");
            }
        }

        [HttpPatch("{id}")]
        public  async Task<ActionResult<TechniqueDTO>> PatchTechnique(int id, [FromBody]JsonPatchDocument<TechniqueDTOForUpdate> itemPatch)
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
        public async Task<ActionResult<TechniqueDTO>> PostTechnique([FromBody]TechniqueDTOForCreation itemForCreation)
        {
            if (itemForCreation == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return new UnprocessableEntityObjectResult(ModelState);

            var item = _mapper.Map<Technique>(itemForCreation);

            try
            {
                var success = await _repository.AddTechnique(item);
                if (success)
                    return CreatedAtAction("GetTechnique", new { id = item.ID }, _mapper.Map<TechniqueDTO>(item));
                else
                    return BadRequest("Saving item failed.");
            }
            catch(RepositoryItemAlreadyExistsException)
            {
                return BadRequest("Technique already exists.");
            }
        }

        [HttpDelete("{id}")]
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
            catch(RepositoryItemNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
