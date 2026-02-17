using Microsoft.EntityFrameworkCore;
using TaskFlow.Api.Data;
using TaskFlow.Api.DTOs;
using TaskFlow.Api.Models;

namespace TaskFlow.Api.Services {
    public class TaskService : ITaskService {
        private readonly ApplicationDbContext _context;

        public TaskService(ApplicationDbContext context) {
            _context = context;
        }

        public async Task<IEnumerable<TaskReadDto>> GetAllTasksAsync() {
            var tasks = await _context.Tasks.ToListAsync();
            // Mapeo manual (luego podrías usar AutoMapper, pero para aprender es mejor así)
            return tasks.Select(t => new TaskReadDto {
                Id = t.Id,
                Title = t.Title,
                IsCompleted = t.IsCompleted
            });
        }

        public async Task<TaskReadDto> CreateTaskAsync(TaskCreateDto taskDto) {
            var task = new TaskItem { Title = taskDto.Title };
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync(); // Aquí se guarda en Azure SQL

            return new TaskReadDto { Id = task.Id, Title = task.Title };
        }

        public async Task<TaskReadDto?> GetTaskByIdAsync(int id) {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null) return null;
            return new TaskReadDto { Id = task.Id, Title = task.Title };
        }
    }
}