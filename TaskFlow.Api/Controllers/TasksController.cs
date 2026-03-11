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
        public async Task<ActionResult> DeleteTask(int id) {
            await _taskService.DeleteTaskAsync(id);
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutTask(int id, TaskUpdateDto taskUpdateDto) {
            if (string.IsNullOrEmpty(taskUpdateDto.Title)) {
                return BadRequest("El título es obligatorio."); //400
            }

            var task = await _taskService.UpdateTaskAsync(id, taskUpdateDto);
            if (task == false)
                return NotFound($"No se pudo encontrar la tarea con ID {id}"); //404
            return NoContent(); // 204

        }
    }
}