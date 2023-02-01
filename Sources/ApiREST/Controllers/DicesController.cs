using DTO;
using Microsoft.AspNetCore.Mvc;
using ModelAppLib;
using NLog.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiREST.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class DicesController : Controller
    {

        private readonly IDataManager _service;

        private static ILogger<DicesController> logger = LoggerFactory.Create(builder => builder.AddNLog()).CreateLogger<DicesController>();

        public DicesController(IDataManager service)
        {
            _service = service;
        }

        // GET: api/v1/dices
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DiceDTO>>> Get()
        {
            logger.LogTrace("Method GET from DiceControler to get list of DiceDTO");
            try
            {
                var result = await _service.GetAllDices();
                if (result == null)
                {
                    logger.LogTrace("Method GET from DiceControler, not found list of DiceDTO");
                    return NotFound();
                }
                logger.LogTrace("Method GET from DiceControler return something");
                return Ok(result.ToDTO());
            }
            catch (Exception)
            {
                logger.LogTrace("Method GET from DiceControler, return Exception");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        // GET api/v1/dices/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DiceDTO>> Get(int id)
        {
            logger.LogTrace("GET Dice with Id");
            try
            {
                var result = await _service.GetDiceWithId(id);
                if( result == null)
                {
                    logger.LogTrace("Method GET by ID from DiceControler, not found the Dice with the Id");
                    return NotFound();
                }
                logger.LogTrace("Method GET by ID from DiceControler, return something");
                return Ok(result.ToDTO());
            } catch(Exception)
            {
                logger.LogTrace("Method GET by ID from DiceControler, return Exception");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<DiceDTO>> Create(DiceDTO dice)
        {
            try
            {
                if (dice == null)
                    return BadRequest();
                var createDice = await _service.AddDice(dice.ToModel());
                if( !createDice)
                {
                    logger.LogError("Methode Post, impossible to add the Dice");
                    return BadRequest();
                }

                // CreatedAtAction va retourne l'objet créé, et sur lequelle on va ensuite le convertire avec le DTO pour ensuite le return
                logger.LogTrace("Methode Post, the dice was added correctly");
                return CreatedAtAction(nameof(Get),
                    new { id = dice.ID }, dice);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/v1/values/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<DiceDTO>> Delete(int id)
        {
            try
            {
                if (id == null)
                {
                    logger.LogError("Methode Delete, the id was null");
                    return BadRequest();
                }
                var diceToDelete = await _service.GetDiceWithId(id);

                if (diceToDelete == null)
                {
                    logger.LogError("Methode Delete, impossible to find the Dice to delete");
                    return NotFound($"Dice with the id = {id} was not found");
                }

                var resultDelete = await _service.DeleteDice(diceToDelete);
                logger.LogTrace("Methode Delete, the Dice was Deleted");
                if (resultDelete.Equals(false) )
                {
                    logger.LogError("Methode Delete, the deleting was not successfull");
                    return NotFound("The delete was not correct ");
                }
                logger.LogTrace("Methode Delete, returned the DTO of the deleted Dice");
                return diceToDelete.ToDTO();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error deleting data");
            }
        }
    }
}

