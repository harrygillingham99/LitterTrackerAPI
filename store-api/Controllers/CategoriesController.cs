using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using store_api.CloudDatastore.DAL.Interfaces;
using store_api.Objects;
using store_api.Objects.StoreObjects;
using Swashbuckle.AspNetCore.Annotations;

namespace store_api.Controllers
{
    [Route("categories")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ILogger<CategoriesController> _logger;
        private readonly ICategoriesRepository _categoriesRepository;

        public CategoriesController(ILogger<CategoriesController> logger, ICategoriesRepository categoriesRepository)
        {
            _logger = logger;
            _categoriesRepository = categoriesRepository;
        }

        [HttpGet("")]
        [SwaggerResponse(200, "Success", typeof(List<Categories>))]
        [SwaggerResponse(500, "Server Error")]
        public async Task<ActionResult<List<Categories>>> GetCategories()
        {
            try
            {
                return Ok((await _categoriesRepository.GetCategories()).ToList());
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception getting categories");
                throw;
            }
        }

        [HttpPost("")]
        [SwaggerResponse(200, "Success", typeof(bool))]
        [SwaggerResponse(500, "Server Error")]
        public async Task<ActionResult<bool>> AddCategory([FromBody] Categories categoryToAdd)
        {
            try
            {
                return Ok((await _categoriesRepository.AddCategories(categoryToAdd)));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception adding category");
                throw;
            }
        }

        [HttpPut("")]
        [SwaggerResponse(200, "Success", typeof(bool))]
        [SwaggerResponse(500, "Server Error")]
        public async Task<ActionResult<bool>> UpdateCategory([FromBody] Categories updatedCategory)
        {
            try
            {
                return Ok(await _categoriesRepository.UpdateCategory(updatedCategory));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception adding category");
                throw;
            }
        }

        [HttpDelete("")]
        [SwaggerResponse(200, "Success", typeof(ActionResult))]
        [SwaggerResponse(500, "Server Error")]
        public async Task<ActionResult> DeleteCategory([FromBody] long key)
        {
            try
            {
                 await _categoriesRepository.DeleteCategory(key);
                 return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception adding category");
                throw;
            }
        }
    }
}
