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
    [Route("session")]
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

        [HttpPost("current-basket")]
        [SwaggerResponse(200, "Success", typeof(Basket))]
        [SwaggerResponse(401, "Unauthorized Request")]
        [SwaggerResponse(500, "Server Error")]
        public async Task<ActionResult<Basket>> GetCurrentBasket([FromBody]AuthedRequest token)
        {
            try
            {
                var requestUid = await token.Verify();

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

        [HttpPost("update-basket")]
        [SwaggerResponse(200, "Success", typeof(bool))]
        [SwaggerResponse(401, "Unauthorized Request")]
        [SwaggerResponse(500, "Server Error")]
        public async Task<ActionResult<bool>> UpdateBasket([FromBody] AuthedBasketRequestWrapper basketToUpdate)
        {
            try
            {
                var requestUid = await basketToUpdate.Token.Verify();

                if (requestUid == null)
                    return Unauthorized();

                basketToUpdate.Basket.UserUid = requestUid;

                return Ok(await _sessionRepository.UpdateBasket(basketToUpdate.Basket));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception adding category");
                throw;
            }
        }

        [HttpPost("place-order")]
        [SwaggerResponse(200, "Success", typeof(bool))]
        [SwaggerResponse(401, "Unauthorized Request")]
        [SwaggerResponse(500, "Server Error")]
        public async Task<ActionResult<bool>> OrderItems([FromBody] AuthedBasketRequestWrapper basketToOrder)
        {
            try
            {
                var requestUid = await basketToOrder.Token.Verify();

                if (requestUid == null)
                    return Unauthorized();

                basketToOrder.Basket.UserUid = requestUid;

                return Ok(await _sessionRepository.OrderItems(basketToOrder.Basket));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception adding category");
                throw;
            }
        }

        [HttpPost("historic-orders")]
        [SwaggerResponse(200, "Success", typeof(List<Basket>))]
        [SwaggerResponse(401, "Unauthorized Request")]
        [SwaggerResponse(500, "Server Error")]
        public async Task<ActionResult<List<Basket>>> GetHistoricOrders([FromBody]AuthedRequest token)
        {
            try
            {
                var requestUid = await token.Verify();

                if (requestUid == null)
                    return Unauthorized();

                return Ok(await _sessionRepository.GetHistoricOrders(requestUid));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception getting historic orders");
                throw;
            }
        }

    }
}
