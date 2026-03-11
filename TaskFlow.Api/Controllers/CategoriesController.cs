using Microsoft.AspNetCore.Mvc;
using TaskFlow.Api.DTOs;
using TaskFlow.Api.Models;
using TaskFlow.Api.Services;

namespace TaskFlow.Api.Controllers {
    [ApiController]
    [Route("api/categorias")]
    public class CategoriesController : ControllerBase {
        private readonly ITaskService _taskService;

        public CategoriesController(ITaskService taskService) {
            _taskService = taskService;
        }

        // GET: api/tasks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryReadDto>>> GetCategories() { // Cambiado TaskItem por CategoryReadDto
            var categories = await _taskService.GetAllCategoriesAsync();
            return Ok(categories);
        }

        [HttpPost]
        public async Task<ActionResult<Category>> CreateCategory(CategoryCreateDto categoryDto) {
            var category = await _taskService.CreateCategoryAsync(categoryDto);
            return Ok(category);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategoryAsync(int id) {
            var eliminado = await _taskService.DeleteCategoryAsync(id);
            
            if (!eliminado) {
                return NotFound($"No he encontrado la categoria con ID {id}");
            }

            return NoContent();
        }
    }
}
