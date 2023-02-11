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

        private static ILogger<DiceSidesController> logger = LoggerFactory.Create(builder => builder.AddNLog()).CreateLogger<DiceSidesController>();


        public DiceSidesController(IDataManager service)
        {
            _service = service;
        }

        // GET: api/v1/dicesides
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DiceSideDTO>>> Get()
        {
            logger.LogTrace("Method GET from DiceSidesController to get list of DiceSideDTO");
            try
            {
                var result = await _service.GetAllSides();
                if (result == null)
                {
                    logger.LogTrace("Method GET from DiceSidesController, not found list of DiceSideDTO");
                    return NotFound();
                }
                logger.LogTrace("Method GET from DiceSidesController return something");
                return Ok(result.ToDTO());
            }
            catch (Exception)
            {
                logger.LogTrace("Method GET from DiceSidesController, return Exception");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        // GET api/v1/dicesides/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DiceSideDTO>> Get(int id)
        {
            logger.LogTrace("GET DiceSide with Id");
            try
            {
                var result = await _service.GetDiceSideWithId(id);
                if (result == null)
                {
                    logger.LogTrace("Method GET by ID from DiceSidesController, not found the DiceSide with the Id");
                    return NotFound();
                }
                logger.LogTrace("Method GET by ID from DiceSidesController, return something");
                return Ok(result.ToDTO());
            }
            catch (Exception)
            {
                logger.LogTrace("Method GET by ID from DiceSidesController, return Exception");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<DiceSideDTO>> Create(DiceSideDTO diceSide)
        {
            try
            {
                if (diceSide == null)
                    return BadRequest();
                var createDiceSide = await _service.AddSide(diceSide.ToModel());
                if (!createDiceSide)
                {
                    logger.LogError("Methode Post, impossible to add the DiceSide");
                    return BadRequest();
                }

                // CreatedAtAction va retourne l'objet créé, et sur lequelle on va ensuite le convertire avec le DTO pour ensuite le return
                logger.LogTrace("Methode Post, the diceSide was added correctly");
                return CreatedAtAction(nameof(Get),
                    new { id = diceSide.ID }, diceSide);
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
                    logger.LogError("Methode Put, the id was null");
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
                    logger.LogError("Methode Delete, impossible to find the Dice to delete");
                    return NotFound($"Dice with the id = {id} was not found");
                }

                var resultDelete = await _service.DeleteSide(diceSideToDelete);
                logger.LogTrace("Methode Delete, the Dice was Deleted");
                if (resultDelete.Equals(false))
                {
                    logger.LogError("Methode Delete, the deleting was not successfull");
                    return NotFound("The delete was not correct ");
                }
                logger.LogTrace("Methode Delete, returned the DTO of the deleted Dice");
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
