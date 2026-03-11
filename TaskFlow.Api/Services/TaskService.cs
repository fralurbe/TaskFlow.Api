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
                CategoryName = t.Category?.Name ?? "Sin categoría"
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
                CategoryName = t.Category?.Name ?? "Sin categoría"
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
                CategoryId = taskDto.CategoryId,
                IsCompleted = false,
                CreatedAt = DateTime.UtcNow
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
                CategoryName = taskConInfo.Category?.Name ?? "Sin categoría"
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

        public async Task<Category> CreateCategoryAsync(CategoryCreateDto categoryDto) {
            var nuevaCategoria = new Category {
                Name = categoryDto.Name
            };

            _context.Categories.Add(nuevaCategoria);
            await _context.SaveChangesAsync();
            return nuevaCategoria;
        }

        public async  Task<IEnumerable<CategoryReadDto>> GetAllCategoriesAsync() {
            var categories = await _context.Categories.ToListAsync();

            return categories.Select(c => new CategoryReadDto {
                Id = c.Id,
                Name = c.Name
            });
        }

        public async Task<bool> DeleteCategoryAsync(int id) {
            var categoria = await _context.Categories.FindAsync(id);

            if (categoria == null) return false;

            _context.Categories.Remove(categoria);
            await _context.SaveChangesAsync();
            
            return true;
        }        
    }
}