using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using litter_tracker.CloudDatastore.DAL.Interfaces;
using litter_tracker.Objects.ApiObjects;
using litter_tracker.Services.GoogleCloudStorage;
using litter_tracker.Services.OpenWeatherApi;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using store_api.Helpers;
using Swashbuckle.AspNetCore.Annotations;

namespace store_api.Controllers
{
    [Route("app")]
    [ApiController]
    public class LitterTrackerAppController : ControllerBase
    {
        private readonly ILogger<LitterTrackerAppController> _logger;
        private readonly ILitterTrackerRepository _litterTrackerRepository;
        private readonly IOpenWeatherServiceAgent _openWeatherServiceAgent;
        private readonly IGoogleCloudStorage _googleCloudStorage;
        public LitterTrackerAppController(ILogger<LitterTrackerAppController> logger, ILitterTrackerRepository litterTrackerRepository, IOpenWeatherServiceAgent openWeatherServiceAgent, IGoogleCloudStorage googleCloudStorage)
        {
            _logger = logger;
            _litterTrackerRepository = litterTrackerRepository;
            _openWeatherServiceAgent = openWeatherServiceAgent;
            _googleCloudStorage = googleCloudStorage;
        }

        [HttpGet("pins")]
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

                return Ok(await _litterTrackerRepository.GetLitterPins());
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception getting categories");
                throw;
            }
        }

        [HttpPost("pin")]
        [SwaggerResponse(200, "Success", typeof(LitterPin))]
        [SwaggerResponse(401, "Unauthorized Request")]
        [SwaggerResponse(500, "Server Error")]
        public async Task<ActionResult<LitterPin>> CreateNewLitterPin([FromBody] LitterPin request)
        {
            try
            {
                var requestUid = await HttpContext.Request.AuthorizeWithFirebase();

                if (requestUid == null)
                    return Unauthorized();

                request = await request.EnsureWeatherData(_openWeatherServiceAgent);

                await _litterTrackerRepository.CreateNewLitterPin(request);

                return Ok(request);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error creating new pin");
                throw;
            }
        }

        [HttpPost("pins")]
        [SwaggerResponse(200, "Success", typeof(List<LitterPin>))]
        [SwaggerResponse(401, "Unauthorized Request")]
        [SwaggerResponse(500, "Server Error")]
        public async Task<ActionResult<List<LitterPin>>> CreateNewLitterPins([FromBody] List<LitterPin> request)
        {
            try
            {
                var requestUid = await HttpContext.Request.AuthorizeWithFirebase();

                if (requestUid == null)
                    return Unauthorized();

                request = await request.EnsureWeatherData(_openWeatherServiceAgent);

                await _litterTrackerRepository.CreateNewLitterPins(request);

                return Ok(request);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error creating new pins");
                throw;
            }
        }

        [HttpPost("update-pin")]
        [SwaggerResponse(200, "Success", typeof(LitterPin))]
        [SwaggerResponse(401, "Unauthorized Request")]
        [SwaggerResponse(403, "Not Pin Owner")]
        [SwaggerResponse(500, "Server Error")]
        public async Task<ActionResult<LitterPin>> UpdateLitterPin([FromBody] LitterPin request)
        {
            try
            {
                var requestUid = await HttpContext.Request.AuthorizeWithFirebase();

                if (requestUid == null)
                    return Unauthorized();

                if (requestUid != request.CreatedByUid)
                    return Forbid();

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
        public async Task<ActionResult> DeleteLitterPin([FromBody] LitterPin request)
        {
            try
            {
                var requestUid = await HttpContext.Request.AuthorizeWithFirebase();

                if (requestUid == null)
                    return Unauthorized();

                if (requestUid != request.CreatedByUid)
                    return Forbid();

                await _litterTrackerRepository.DeleteLitterPin(request.DataStoreId);

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error updating pin");
                throw;
            }
        }

        [HttpPost("upload-image/{uploadedByUid}/{markerDatastoreId}")]
        [SwaggerResponse(200, "Success", typeof(ActionResult))]
        [SwaggerResponse(401, "Unauthorized Request")]
        [SwaggerResponse(500, "Server Error")]
        public async Task<ActionResult> UploadImage(IFormFile file, [FromRoute] string uploadedByUid, [FromRoute] long markerDatastoreId )
        {
            try
            {
                var requestUid = await HttpContext.Request.AuthorizeWithFirebase();

                if (requestUid == null)
                    return Unauthorized();

                await _googleCloudStorage.UploadFile(uploadedByUid, markerDatastoreId, file);

                return Ok();
            }
            catch (NullReferenceException e)
            {
                _logger.LogError(e, $"Null ref uploading file");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error uploading file");
                throw;
            }
        }
    }
}
