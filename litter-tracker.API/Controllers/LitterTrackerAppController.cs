using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using litter_tracker.CloudDatastore.DAL.Interfaces;
using litter_tracker.Objects.Helpers;
using litter_tracker.Objects.StoreObjects;
using Microsoft.Extensions.Logging;
using store_api.AuthHelpers;
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

        [HttpGet("pins")]
        [SwaggerResponse(200, "Success", typeof(List<LitterPin>))]
        [SwaggerResponse(401, "Unauthorized Request")]
        [SwaggerResponse(500, "Server Error")]
        public async Task<ActionResult<List<LitterPin>>> GetLitterPins()
        {
            try
            {
                //var requestUid = await HttpContext.Request.AuthorizeWithFirebase();

                //if (requestUid == null)
                //    return Unauthorized();

                return Ok(await _litterTrackerRepository.GetLitterPins());
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
                //var requestUid = await HttpContext.Request.AuthorizeWithFirebase();

                //if (requestUid == null)
                //    return Unauthorized();

                await _litterTrackerRepository.CreateNewLitterPin(request);

                return Ok();

            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error creating new pin");
                throw;
            }
        }

        [HttpPost("pins")]
        [SwaggerResponse(200, "Success", typeof(ActionResult))]
        [SwaggerResponse(401, "Unauthorized Request")]
        [SwaggerResponse(500, "Server Error")]
        public async Task<ActionResult> CreateNewLitterPins([FromBody] List<LitterPin> request)
        {
            try
            {
                //var requestUid = await HttpContext.Request.AuthorizeWithFirebase();

                //if (requestUid == null)
                //    return Unauthorized();

                await _litterTrackerRepository.CreateNewLitterPins(request);

                return Ok();

            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error creating new pins");
                throw;
            }
        }

        [HttpPost("update-pin")]
        [SwaggerResponse(200, "Success", typeof(ActionResult))]
        [SwaggerResponse(401, "Unauthorized Request")]
        [SwaggerResponse(403, "Not Pin Owner")]
        [SwaggerResponse(500, "Server Error")]
        public async Task<ActionResult<LitterPin>> UpdateLitterPin([FromBody] LitterPin request)
        {
            try
            {
                //var requestUid = await HttpContext.Request.AuthorizeWithFirebase();

                //if (requestUid == null)
                //    return Unauthorized();

                //if(requestUid != request.CreatedByUid)
                //    return Forbid();

                var result = await _litterTrackerRepository.UpdateLitterPin(request);

                return result == null ? Problem(detail: "Failed to update and return requested pin") : Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error updating pin");
                throw;
            }
        }

        [HttpDelete("delete-pin")]
        [SwaggerResponse(200, "Success", typeof(ActionResult))]
        [SwaggerResponse(401, "Unauthorized Request")]
        [SwaggerResponse(403, "Not Pin Owner")]
        [SwaggerResponse(500, "Server Error")]
        public async Task<ActionResult<LitterPin>> DeleteLitterPin([FromBody] LitterPin request)
        {
            try
            {
                //var requestUid = await HttpContext.Request.AuthorizeWithFirebase();

                //if (requestUid == null)
                //    return Unauthorized();

                //if (requestUid != request.CreatedByUid)
                //    return Forbid();

                await _litterTrackerRepository.DeleteLitterPin(request.DataStoreId);

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error updating pin");
                throw;
            }
        }
    }
}
