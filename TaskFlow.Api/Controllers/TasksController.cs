using Microsoft.AspNetCore.Mvc;
using TaskFlow.Api.DTOs;
using TaskFlow.Api.Services;

namespace TaskFlow.Api.Controllers {
    [ApiController] // Indica que esto es una API y no una web con HTML
    [Route("api/[controller]")] // La ruta será: api/tasks
    public class TasksController : ControllerBase {
        private readonly ITaskService _service;

        // "Profesor": Aquí pedimos el contrato (Interfaz), no la clase real.
        // .NET se encarga de darnos el TaskService automáticamente.
        public TasksController(ITaskService service) {
            _service = service;
        }

        [HttpGet] // Para leer todas las tareas
        public async Task<ActionResult<IEnumerable<TaskReadDto>>> GetTasks() {
            var tasks = await _service.GetAllTasksAsync();
            return Ok(tasks); // Devuelve un código 200 OK con la lista
        }

        [HttpPost] // Para crear una nueva tarea
        public async Task<ActionResult<TaskReadDto>> CreateTask(TaskCreateDto taskCreateDto) {
            var taskReadDto = await _service.CreateTaskAsync(taskCreateDto);

            // Devuelve un código 201 (Created) y la ubicación del nuevo recurso
            return CreatedAtAction(nameof(GetTasks), new { id = taskReadDto.Id }, taskReadDto);
        }
    }
}