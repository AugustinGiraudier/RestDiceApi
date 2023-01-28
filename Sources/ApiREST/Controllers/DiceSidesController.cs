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



    }
}
