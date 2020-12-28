using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using store_api.CloudDatastore.DAL.Interfaces;
using store_api.Objects;
using store_api.Objects.StoreObjects;
using Swashbuckle.AspNetCore.Annotations;

namespace store_api.Controllers
{
    [ApiController]
    [Route("products")]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IProductRepository _storeRepository;

        public ProductsController(ILogger<ProductsController> logger, IProductRepository storeRepository)
        {
            _logger = logger;
            _storeRepository = storeRepository;
        }

        [HttpGet("")]
        [SwaggerResponse(200, "Success", typeof(List<Product>))]
        [SwaggerResponse(500, "Server Error")]
        public async Task<List<Product>> GetProducts()
        {
            try
            {
                return (await _storeRepository.GetProducts()).ToList();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception getting products");
                throw;
            }
            
        }

        [HttpPut("")]
        [SwaggerResponse(200, "Success", typeof(bool))]
        [SwaggerResponse(500, "Server Error")]
        public async Task<bool> UpdateProduct([FromBody] Product product)
        {
            try
            {
                return await _storeRepository.UpdateProduct(product);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception updating products");
                throw;
            }
        }

        [HttpPost("")]
        [SwaggerResponse(200, "Success", typeof(bool))]
        [SwaggerResponse(500, "Server Error")]
        public async Task<bool> InsertProduct([FromBody] Product product)
        {
            try
            {
                return await _storeRepository.InsertProduct(product);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception inserting products");
                throw;
            }
        }

        [HttpDelete("")]
        [SwaggerResponse(200, "Success", typeof(ActionResult))]
        [SwaggerResponse(500, "Server Error")]
        public async Task<ActionResult> DeleteProduct([FromBody] long key)
        {
            try
            {
                await _storeRepository.DeleteProduct(key);
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception expiring products");
                throw;
            }
        }
    }
}
