using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using store_api.CloudDatastore.DAL.Interfaces;
using store_api.Objects;
using store_api.Objects.StoreObjects;
using Swashbuckle.AspNetCore.Annotations;

namespace store_api.Controllers
{
    [Route("admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly ILogger<AdminController> _logger;
        private readonly IAdminRepository _adminRepository;

        public AdminController(ILogger<AdminController> logger, IAdminRepository adminRepository)
        {
            _logger = logger;
            _adminRepository = adminRepository;
        }

        [HttpPost("is-admin")]
        [SwaggerResponse(200, "Success", typeof(bool))]
        [SwaggerResponse(401, "Unauthorized Request")]
        [SwaggerResponse(500, "Server Error")]
        public async Task<ActionResult<bool>> IsAdminUser([FromBody]AuthedRequest token)
        {
            try
            {
                var requestUid = await token.Verify();

                if (requestUid == null)
                    return Unauthorized();

                return Ok((await _adminRepository.IsAdminUser(requestUid)));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception getting categories");
                throw;
            }
        }
    }
}
