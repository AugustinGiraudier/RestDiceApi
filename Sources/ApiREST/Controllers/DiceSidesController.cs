using DTO;
using Microsoft.AspNetCore.Mvc;
using ModelAppLib;
using NLog.Extensions.Logging;


namespace ApiREST.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class DiceSidesController : Controller
    {

        private readonly IDataManager _service;
        private readonly ILogger<DicesController> _logger;

        public DiceSidesController(IDataManager service, ILogger<DicesController> logger)
        {
            _service = service;
            _logger = logger;
        }

        // GET: api/v1/dicesides
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DiceSideDTO>>> Get()
        {
            _logger.LogTrace("Method GET from DiceSidesController to get list of DiceSideDTO");
            try
            {
                var result = await _service.GetAllSides();
                if (result == null)
                {
                    _logger.LogTrace("Method GET from DiceSidesController, not found list of DiceSideDTO");
                    return NotFound();
                }
                _logger.LogTrace("Method GET from DiceSidesController return something");
                return Ok(result.ToDTO());
            }
            catch (Exception)
            {
                _logger.LogTrace("Method GET from DiceSidesController, return Exception");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        // GET api/v1/dicesides/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DiceSideDTO>> Get(int id)
        {
            _logger.LogTrace("GET DiceSide with Id");
            try
            {
                var result = await _service.GetDiceSideWithId(id);
                if (result == null)
                {
                    _logger.LogTrace("Method GET by ID from DiceSidesController, not found the DiceSide with the Id");
                    return NotFound();
                }
                _logger.LogTrace("Method GET by ID from DiceSidesController, return something");
                return Ok(result.ToDTO());
            }
            catch (Exception)
            {
                _logger.LogTrace("Method GET by ID from DiceSidesController, return Exception");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<DiceSideDTO>> Create(InputDiceSideDTO diceSide)
        {
            try
            {
                if (diceSide == null)
                    return BadRequest();
                DiceSideDTO final = new DiceSideDTO();
                final.image = diceSide.image;
                var model = final.ToModel();
                var createDiceSide = await _service.AddSide(model);
                if (!createDiceSide)
                {
                    _logger.LogError("Methode Post, impossible to add the DiceSide");
                    return BadRequest();
                }
                final.ID = model.Id;
                _logger.LogTrace("Methode Post, the diceSide was added correctly");
                return final;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<ActionResult<DiceSideDTO>> Put(int id, [FromBody]InputDiceSideDTO diceSide)
        {
            try
            {
                if (id == null)
                {
                    _logger.LogError("Methode Put, the id was null");
                    return BadRequest();
                }
                DiceSideDTO final = new DiceSideDTO();
                final.ID = id;
                final.image = diceSide.image;
                await _service.UpdateSide(final.ToModel());
                return CreatedAtAction(nameof(Get),
                    new { id = final.ID }, final); ;
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error updating data");
            }
        }


        // DELETE api/v1/values/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<DiceSideDTO>> Delete(int id)
        {
            try
            {
                var diceSideToDelete = await _service.GetDiceSideWithId(id);

                if (diceSideToDelete == null)
                {
                    _logger.LogError("Methode Delete, impossible to find the Dice to delete");
                    return NotFound($"Dice with the id = {id} was not found");
                }

                var resultDelete = await _service.DeleteSide(diceSideToDelete);
                _logger.LogTrace("Methode Delete, the Dice was Deleted");
                if (resultDelete.Equals(false))
                {
                    _logger.LogError("Methode Delete, the deleting was not successfull");
                    return NotFound("The delete was not correct ");
                }
                _logger.LogTrace("Methode Delete, returned the DTO of the deleted Dice");
                return diceSideToDelete.ToDTO();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error deleting data");
            }
        }

    }
}
