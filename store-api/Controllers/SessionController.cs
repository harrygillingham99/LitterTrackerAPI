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
    public class SessionController : ControllerBase
    {
        private readonly ILogger<SessionController> _logger;
        private readonly ISessionRepository _sessionRepository;

        public SessionController(ILogger<SessionController> logger, ISessionRepository sessionRepository)
        {
            _logger = logger;
            _sessionRepository = sessionRepository;
        }

        [HttpGet("all-pins")]
        [SwaggerResponse(200, "Success", typeof(List<LitterPin>))]
        [SwaggerResponse(401, "Unauthorized Request")]
        [SwaggerResponse(500, "Server Error")]
        public async Task<ActionResult<List<LitterPin>>> GetLitterPins()
        {
            try
            {
                var requestUid = await HttpContext.Request.Headers["JWT"].FirstOrDefault().Verify();

                if (requestUid == null)
                    return Unauthorized();

                return Ok((await _sessionRepository.GetCurrentBasket(requestUid)));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception getting categories");
                throw;
            }
        }
    }
}
