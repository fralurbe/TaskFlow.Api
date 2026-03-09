using Microsoft.AspNetCore.Mvc;
using TaskFlow.Api.DTOs;
using TaskFlow.Api.Models;
using TaskFlow.Api.Services;

namespace TaskFlow.Api.Controllers {
    [ApiController]
    [Route("api/[controller]")] // La URL será: api/tasks
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
    }
}