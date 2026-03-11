using TaskFlow.Api.DTOs;
using TaskFlow.Api.Models;

namespace TaskFlow.Api.Services {
    public interface ITaskService {       
        // Usamos Task (asíncrono) porque en Azure la red puede tener latencia
        // Task<IEnumerable<TaskReadDto>> GetAllTasksAsync();
        // Task<TaskReadDto?> GetTaskByIdAsync(int id);
        // Task<TaskReadDto> CreateTaskAsync(TaskCreateDto taskDto);

        public Task<IEnumerable<TaskReadDto>> GetAllTasksAsync();

        public Task<TaskReadDto?> GetTaskById(int id);

        public Task<TaskReadDto> CreateTaskAsync(TaskCreateDto taskDto);

        public Task DeleteTaskAsync(int id);

        public Task<Category> CreateCategoryAsync(CategoryCreateDto categoryDto);

        public Task<IEnumerable<CategoryReadDto>> GetAllCategoriesAsync();
        
        public Task<bool> DeleteCategoryAsync(int id);

        public Task<IEnumerable<TaskReadDto>> GetTasksByCategoryAsync(int categoryId);

        public Task<bool> UpdateTaskAsync(int id, TaskUpdateDto updateDto);
    }
}