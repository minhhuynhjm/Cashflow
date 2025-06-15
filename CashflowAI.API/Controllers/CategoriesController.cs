using CashflowAI.Domain.DTOs;
using CashflowAI.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CashflowAI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDto>> GetById(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            return Ok(category);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetByUserId(int userId)
        {
            var categories = await _categoryService.GetByUserIdAsync(userId);
            return Ok(categories);
        }

        [HttpPost]
        public async Task<ActionResult<CategoryDto>> Create(CategoryDto createDto)
        {
            var result = await _categoryService.CreateAsync(createDto);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CategoryDto categoryDto)
        {
            await _categoryService.UpdateAsync(id, categoryDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _categoryService.DeleteAsync(id);
            return NoContent();
        }
    }
} 