using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Api.DTOs;
using TaskFlow.Api.Models;
using TaskFlow.Api.Services;

namespace TaskFlow.Api.Controllers {    
    [ApiController]
    [Route("api/categorias")]
    public class CategoriesController : ControllerBase {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService) {
            _categoryService = categoryService;
        }

        // GET: api/tasks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryReadDto>>> GetCategories() { // Cambiado TaskItem por CategoryReadDto
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(categories);
        }

        [HttpPost]
        public async Task<ActionResult<Category>> CreateCategory(CategoryCreateDto categoryDto) {
            var category = await _categoryService.CreateCategoryAsync(categoryDto);
            return Ok(category);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id) {
            var resultado = await _categoryService.DeleteCategoryAsync(id);

            return resultado switch {
                "SUCCESS" => NoContent(),           // 204: Borrado con éxito
                "NOT_FOUND" => NotFound($"No existe la categoría {id}"), // 404
                "HAS_TASKS" => Conflict("No se puede borrar: esta categoría tiene tareas asociadas."), // 409: Conflicto de datos
                _ => StatusCode(500, "Error inesperado")
            };
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, CategoryUpdateDto updateDto) {
            var resultado = await _categoryService.UpdateCategoryAsync(id, updateDto);

            return resultado switch {
                "SUCCESS" => NoContent(),           // 204: modificado con éxito
                "NOT_FOUND" => NotFound($"No existe la categoría {id}"), // 404              
                _ => StatusCode(500, "Error inesperado")
            };
        }
    }
}
