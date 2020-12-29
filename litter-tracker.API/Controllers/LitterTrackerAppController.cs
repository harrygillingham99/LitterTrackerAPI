using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using store_api.CloudDatastore.DAL.Interfaces;
using store_api.Objects.StoreObjects;
using Swashbuckle.AspNetCore.Annotations;

namespace store_api.Controllers
{
    [Route("app")]
    [ApiController]
    public class LitterTrackerAppController : ControllerBase
    {
        private readonly ILogger<LitterTrackerAppController> _logger;
        private readonly ILitterTrackerRepository _litterTrackerRepository;

        public LitterTrackerAppController(ILogger<LitterTrackerAppController> logger, ILitterTrackerRepository litterTrackerRepository)
        {
            _logger = logger;
            _litterTrackerRepository = litterTrackerRepository;
        }

        [HttpGet("all-pins")]
        [SwaggerResponse(200, "Success", typeof(List<LitterPin>))]
        [SwaggerResponse(401, "Unauthorized Request")]
        [SwaggerResponse(500, "Server Error")]
        public async Task<ActionResult<List<LitterPin>>> GetLitterPins()
        {
            try
            {
                var requestUid = await HttpContext.Request.AuthorizeWithFirebase();

                if (requestUid == null)
                    return Unauthorized();

                return Ok(new List<LitterPin>(){new LitterPin()});
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception getting categories");
                throw;
            }
        }

        [HttpPost("pin")]
        [SwaggerResponse(200, "Success", typeof(ActionResult))]
        [SwaggerResponse(401, "Unauthorized Request")]
        [SwaggerResponse(500, "Server Error")]
        public async Task<ActionResult> CreateNewLitterPin([FromBody] LitterPin request)
        {
            try
            {
                var requestUid = await HttpContext.Request.AuthorizeWithFirebase();

                if (requestUid == null)
                    return Unauthorized();

                return Ok();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
