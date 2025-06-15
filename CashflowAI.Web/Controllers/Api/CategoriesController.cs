using Microsoft.AspNetCore.Mvc;
using CashflowAI.Domain.Interfaces;
using CashflowAI.Domain.DTOs;
using CashflowAI.Domain.Exceptions;

namespace CashflowAI.Web.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(
            ICategoryService categoryService,
            ILogger<CategoriesController> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            try
            {
                var userId = 1; // For demo purposes
                var categories = await _categoryService.GetByUserIdAsync(userId);
                return Ok(new { success = true, data = categories });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting categories");
                return StatusCode(500, new { success = false, message = "Error getting categories" });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            try
            {
                var category = await _categoryService.GetByIdAsync(id);
                return Ok(new { success = true, data = category });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting category {CategoryId}", id);
                return StatusCode(500, new { success = false, message = "Error getting category" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryDto dto)
        {
            try
            {
                var category = await _categoryService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, 
                    new { success = true, data = category });
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating category");
                return StatusCode(500, new { success = false, message = "Error creating category" });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryDto dto)
        {
            try
            {
                await _categoryService.UpdateAsync(id, dto);
                return Ok(new { success = true, message = "Category updated successfully" });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating category {CategoryId}", id);
                return StatusCode(500, new { success = false, message = "Error updating category" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                await _categoryService.DeleteAsync(id);
                return Ok(new { success = true, message = "Category deleted successfully" });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting category {CategoryId}", id);
                return StatusCode(500, new { success = false, message = "Error deleting category" });
            }
        }

        [HttpGet("default")]
        public async Task<IActionResult> GetDefaultCategories()
        {
            try
            {
                var categories = await _categoryService.GetListAsync();
                return Ok(new { success = true, data = categories });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting default categories");
                return StatusCode(500, new { success = false, message = "Error getting default categories" });
            }
        }
    }
} 