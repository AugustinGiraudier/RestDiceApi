using DTO;
using Microsoft.AspNetCore.Mvc;
using ModelAppLib;
using NLog.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiREST.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class DiceController : Controller
    {

        private readonly IDataManager _service;
        private static ILogger<DiceController> logger = LoggerFactory.Create(builder => builder.AddNLog()).CreateLogger<DiceController>();

        public DiceController(IDataManager service)
        {
            _service = service;
        }

        // GET: api/dices
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

        // GET api/dices/5
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

        /*
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Dice>> Create(Dice dice) // Prendre pas un DIce mais un DTO
        {
            try
            {
                var createDice = await _service.AddDice(dice);
                if( !createDice)
                {
                    return BadRequest();
                }

                // CreatedAtAction va retourne l'objet créé, et sur lequelle on va ensuite le convertire avec le DTO pour ensuite le return
                return CreatedAtAction(nameof(GetAllDices),
                    new { id = createDice.id }, createDice);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }*/
        

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

