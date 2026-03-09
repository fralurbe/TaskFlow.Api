using Microsoft.EntityFrameworkCore;
using TaskFlow.Api.Data;
using TaskFlow.Api.DTOs;
using TaskFlow.Api.Models;

namespace TaskFlow.Api.Services {
    public class TaskService : ITaskService {
        ApplicationDbContext _context;
        public TaskService(ApplicationDbContext context) {
            _context = context;

        }
        public async Task<IEnumerable<TaskReadDto>> GetAllTasksAsync() {
            var tasks = await _context.Tasks.ToListAsync();

            return tasks.Select(t => new TaskReadDto {
                Id = t.Id,
                Title = t.Title,
                IsCompleted = t.IsCompleted
            });
        }

        public async Task<TaskReadDto> GetTaskById(int id) {
            var t = await _context.Tasks.FindAsync(id);
            if (t == null) return null;
            return new TaskReadDto {
                Id = t.Id,
                Title = t.Title,
                IsCompleted = t.IsCompleted
            };
        }

        public async Task<TaskReadDto> CreateTaskAsync(TaskCreateDto taskDto) {
            if (string.IsNullOrEmpty(taskDto.Title)) {
                throw new Exception("¡Oye! No puedes crear una tarea sin título.");
            }

            var taskParaDb = new TaskItem {
                Title = "IMPORTANTE " + taskDto.Title,
                IsCompleted = false
            };

            _context.Tasks.Add(taskParaDb);

            await _context.SaveChangesAsync();

            // 4. "Mapeo" de salida: Devolvemos el DTO de lectura.
            // Ahora 'taskParaDb.Id' ya tiene el número que generó la base de datos.
            return new TaskReadDto {
                Id = taskParaDb.Id,
                Title = taskParaDb.Title,
                IsCompleted = taskParaDb.IsCompleted
            };
        }

        public async Task DeleteTaskAsync(int id) {
            // 1. Buscamos la tarea en la DB por su ID
            var task = await _context.Tasks.FindAsync(id);

            // 2. Si existe, la eliminamos del contexto
            if (task != null) {
                _context.Tasks.Remove(task);
                // 3. Guardamos cambios para que SQL se entere
                await _context.SaveChangesAsync();
            }
        }
    }
}