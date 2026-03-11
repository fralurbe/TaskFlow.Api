using Microsoft.AspNetCore.Mvc;
using TaskFlow.Api.DTOs;
using TaskFlow.Api.Models;
using TaskFlow.Api.Services;

namespace TaskFlow.Api.Controllers {
    [ApiController]
    //[Route("api/[controller]")] // La URL será: api/tasks
    [Route("api/mis-tareas")]
    public class TasksController : ControllerBase {
        private readonly ITaskService _taskService;

        // Le pedimos al sistema que nos traiga el servicio que registramos en Program.cs
        public TasksController(ITaskService taskService) {
            _taskService = taskService;
        }

        // GET: api/tasks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetTasks() {
            var tasks = await _taskService.GetAllTasksAsync();
            return Ok(tasks);
        }

        [HttpGet("categoria/{categoryId}")]
        public async Task<ActionResult<IEnumerable<TaskReadDto>>> GetTasksByCategory (int categoryId) {
            if (categoryId <= 0)
                return BadRequest("No se admiten ids negativos");
            var tasks = await _taskService.GetTasksByCategoryAsync(categoryId);
            return Ok(tasks);
        }

        [HttpPost]
        public async Task<ActionResult<TaskReadDto>> CreateTask(TaskCreateDto taskDto) {            
            // Ahora le pasamos el DTO al servicio, que es lo que él espera
            var newTask = await _taskService.CreateTaskAsync(taskDto);

            // El servicio nos devuelve un TaskReadDto (la versión de lectura)
            return CreatedAtAction(nameof(GetTasks), new { id = newTask.Id }, newTask);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id) {
            var eliminado = await _taskService.DeleteTaskAsync(id);

            if (!eliminado)
                return NotFound($"No existe la tarea {id}"); //404
            return NoContent();//204
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, TaskUpdateDto updateDto) {
            var resultado = await _taskService.UpdateTaskAsync(id, updateDto);

            return resultado switch {
                "SUCCESS" => NoContent(),
                "TASK_NOT_FOUND" => NotFound($"No se encontró la tarea con ID {id}"),
                "CATEGORY_NOT_FOUND" => BadRequest("La categoría especificada no existe."),
                _ => StatusCode(500, "Error interno del servidor")
            };
        }
    }
}