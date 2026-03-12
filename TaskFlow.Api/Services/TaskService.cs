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
            // Usamos Include para cargar la categoría asociada
            var tasks = await _context.Tasks.Include(t => t.Category).ToListAsync();

            return tasks.Select(t => new TaskReadDto {
                Id = t.Id,
                Title = t.Title,
                IsCompleted = t.IsCompleted,
                CategoryName = t.Category?.Name ?? "Sin categoría",
                Priority = t.Priority.ToString()
            });
        }

        public async Task<IEnumerable<TaskReadDto>> GetTasksByCategoryAsync(int categoryId) {
            bool existeCategoria = await _context.Categories.AnyAsync(c => c.Id == categoryId);
            if (!existeCategoria)
                return null;

            var tasks = await _context.Tasks
                .Include(t => t.Category)
                .Where(t => t.CategoryId == categoryId)
                .ToListAsync();

            return tasks.Select(t => new TaskReadDto {
                Id = t.Id,
                Title = t.Title,
                IsCompleted = t.IsCompleted,
                CategoryName = t.Category?.Name ?? "Sin categoría",
                Priority = t.Priority.ToString()
            });
        }

        public async Task<TaskReadDto> GetTaskById(int id) {
            var t = await _context.Tasks.FindAsync(id);
            if (t == null) return null;
            return new TaskReadDto {
                Id = t.Id,
                Title = t.Title,
                IsCompleted = t.IsCompleted,
                Priority = t.Priority.ToString()
            };
        }        

        public async Task<TaskReadDto> CreateTaskAsync(TaskCreateDto taskDto) {
            if (string.IsNullOrEmpty(taskDto.Title)) {
                throw new Exception("¡Oye! No puedes crear una tarea sin título.");
            }

            var taskParaDb = new TaskItem {
                Title = taskDto.Title,
                CategoryId = taskDto.CategoryId,
                IsCompleted = false,
                CreatedAt = DateTime.UtcNow,
                Priority = Priority.Medio
            };

            _context.Tasks.Add(taskParaDb);

            await _context.SaveChangesAsync();

            // pero pidiéndole a SQL que haga el JOIN con las categorías.
            var taskConInfo = await _context.Tasks
                .Include(t => t.Category) // <--- Aquí cargamos la relación
                .FirstOrDefaultAsync(t => t.Id == taskParaDb.Id);

            // 4. "Mapeo" de salida actualizado
            return new TaskReadDto {
                Id = taskConInfo.Id,
                Title = taskConInfo.Title,
                IsCompleted = taskConInfo.IsCompleted,
                CategoryName = taskConInfo.Category?.Name ?? "Sin categoría",
                Priority = taskConInfo.Priority.ToString()
            };
        }

        public async Task<bool> DeleteTaskAsync(int id) {
            // 1. Buscamos la tarea en la DB por su ID
            var task = await _context.Tasks.FindAsync(id);

            // 2. Si existe, la eliminamos del contexto
            if (task != null) {
                _context.Tasks.Remove(task);
                // 3. Guardamos cambios para que SQL se entere
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }        

        public async Task<string> UpdateTaskAsync(int id, TaskUpdateDto updateDto) {
            // 1. ¿Existe la tarea?
            var task = await _context.Tasks.FindAsync(id);
            if (task == null) return "TASK_NOT_FOUND";

            // 2. ¿Existe la nueva categoría?
            var categoryExists = await _context.Categories.AnyAsync(c => c.Id == updateDto.CategoryId);
            if (!categoryExists) return "CATEGORY_NOT_FOUND";

            // 3. Si todo ok, actualizamos
            task.Title = updateDto.Title;
            task.IsCompleted = updateDto.IsCompleted;
            task.CategoryId = updateDto.CategoryId;

            await _context.SaveChangesAsync();
            return "SUCCESS";
        }

        public async Task<bool> DeleteTaskasync(int id) {
            var task = await _context.Tasks.FindAsync(id);

            if (task == null) return false;

            _context.Remove(task);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}