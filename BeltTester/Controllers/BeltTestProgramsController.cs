using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BeltTester.Data;
using BeltTester.Data.Entities;
using BeltTester.DTO;
using BeltTester.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BeltTester.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BeltTestProgramsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IDataRepository _repository;
        private readonly ISieveModelPreparer _sieveModelPreparer;
        private readonly IPagingLinkCreator _pagingLinkCreator;

        public BeltTestProgramsController(IMapper mapper, IDataRepository repository, ISieveModelPreparer sieveModelPreparer, IPagingLinkCreator pagingLinkCreator)
        {
            _mapper = mapper;
            _repository = repository;
            _sieveModelPreparer = sieveModelPreparer;
            _pagingLinkCreator = pagingLinkCreator;
        }

        [HttpGet("all", Name = "GetAllBeltTestPrograms")]
        public async Task<ActionResult<IEnumerable<BeltTestProgramDTO>>> GetAllBeltTestPrograms()
        {
            try
            {
                var itemsFromRepo = await _repository.GetAllBeltTestPrograms();

                var items = _mapper.Map<List<BeltTestProgramDTO>>(itemsFromRepo);
                return Ok(items);
            }
            catch (RepositoryFilterException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}", Name = "GetBeltTestProgram")]
        public async Task<ActionResult<BeltTestProgramDTO>> GetBeltTestProgram(int id)
        {
            var item = await _repository.GetBeltTestProgram(id);

            if (item == null)
                return NotFound();

            return Ok(_mapper.Map<BeltTestProgramDTO>(item));
        }

        [HttpPost("createwithids")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Manager")]
        public async Task<ActionResult<BeltTestProgramDTO>> CreateCombinationWithIds([FromBody]BeltTestProgramDTOForCreationWithIds itemForCreation)
        {
            if (itemForCreation == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return new UnprocessableEntityObjectResult(ModelState);

            if(itemForCreation.KihonCombinations.Count <= 0)
                return BadRequest("No Kihon combinations given for Belt Test Program.");

            var program = new BeltTestProgram();
            program.ID = 0;
            program.Name = itemForCreation.Name;
            program.StyleName = itemForCreation.StyleName;
            program.Graduation = itemForCreation.Graduation;

            GraduationType gt = GraduationType.Kyu;
            Enum.TryParse<GraduationType>(itemForCreation.GraduationType, true, out gt);
            program.GraduationType = gt;

            foreach (var combinationForCreation in itemForCreation.KihonCombinations.OrderBy(x => x.SequenceNumber))
            {
                var combination = new Combination();
                combination.SequenceNumber = combinationForCreation.SequenceNumber;
                combination.ProgramId = combinationForCreation.ProgramId;

                foreach (var motion in combinationForCreation.Motions)
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
                        Technique = technique,
                        SequenceNumber = motion.SequenceNumber,
                        Annotation = motion.Annotation
                    });
                }

                program.KihonCombinations.Add(combination);
            }

            

            try
            {
                program = await _repository.AddBeltTestProgram(program);
                if (program == null)
                    return BadRequest("Error saving Program.");
                else
                    return CreatedAtAction("GetBeltTestProgram", new { id = program.ID }, _mapper.Map<BeltTestProgramDTO>(program));
            }
            catch (RepositoryItemAlreadyExistsException)
            {
                return BadRequest("Program already exists.");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Manager")]
        public async Task<IActionResult> DeleteBeltTestProgram(int id)
        {
            try
            {
                var success = await _repository.DeleteBeltTestProgram(id);
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