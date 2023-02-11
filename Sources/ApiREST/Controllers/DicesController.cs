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
        private readonly ILogger<DicesController> _logger;
        public DicesController(IDataManager service, ILogger<DicesController> logger)
        {
            _service = service;
            _logger = logger;
        }

        // GET: api/v1/dices
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DiceDTO>>> Get()
        {
            _logger.LogTrace("Method GET from DiceControler to get list of DiceDTO");
            try
            {
                var result = await _service.GetAllDices();
                if (result == null)
                {
                    _logger.LogTrace("Method GET from DiceControler, not found list of DiceDTO");
                    return NotFound();
                }
                _logger.LogTrace("Method GET from DiceControler return something");
                return Ok(result.ToDTO());
            }
            catch (Exception)
            {
                _logger.LogTrace("Method GET from DiceControler, return Exception");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        // GET api/v1/dices/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DiceDTO>> Get(int id)
        {
            _logger.LogTrace("GET Dice with Id");
            try
            {
                var result = await _service.GetDiceWithId(id);
                if( result == null)
                {
                    _logger.LogTrace("Method GET by ID from DiceControler, not found the Dice with the Id");
                    return NotFound();
                }
                _logger.LogTrace("Method GET by ID from DiceControler, return something");
                return Ok(result.ToDTO());
            } catch(Exception)
            {
                _logger.LogTrace("Method GET by ID from DiceControler, return Exception");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<DiceDTO>> Create([FromBody] InputDiceDTO dice)
        {
            try
            {
                if (dice == null)
                    return BadRequest();
                var model = dice.ToModel(_service);
                var createDice = await _service.AddDice(model);
                if( !createDice)
                {
                    _logger.LogError("Methode Post, impossible to add the Dice");
                    return BadRequest();
                }

                // CreatedAtAction va retourne l'objet créé, et sur lequelle on va ensuite le convertire avec le DTO pour ensuite le return
                _logger.LogTrace("Methode Post, the dice was added correctly");
                return CreatedAtAction(nameof(Get),
                    new { id = model.Id }, dice);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async void Put(int id, [FromBody] InputDiceDTO dice)
        {
            try
            {
                var res = await _service.UpdateDice(dice.ToModel(_service));
                if (!res)
                {
                    _logger.LogError("Methode Put, unable to find Dice");
                    BadRequest();
                }
            }
            catch (Exception e)
            {
                StatusCode(StatusCodes.Status500InternalServerError,
                    "Error deleting data");
            }
        }

        // DELETE api/v1/values/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<DiceDTO>> Delete(int id)
        {
            try
            {
                if (id == null)
                {
                    _logger.LogError("Methode Delete, the id was null");
                    return BadRequest();
                }
                var diceToDelete = await _service.GetDiceWithId(id);

                if (diceToDelete == null)
                {
                    _logger.LogError("Methode Delete, impossible to find the Dice to delete");
                    return NotFound($"Dice with the id = {id} was not found");
                }

                var resultDelete = await _service.DeleteDice(diceToDelete);
                _logger.LogTrace("Methode Delete, the Dice was Deleted");
                if (resultDelete.Equals(false) )
                {
                    _logger.LogError("Methode Delete, the deleting was not successfull");
                    return NotFound("The delete was not correct ");
                }
                _logger.LogTrace("Methode Delete, returned the DTO of the deleted Dice");
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

