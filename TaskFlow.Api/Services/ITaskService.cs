using TaskFlow.Api.DTOs;
using TaskFlow.Api.Models;

namespace TaskFlow.Api.Services {
    public interface ITaskService {       
        // Usamos Task (asíncrono) porque en Azure la red puede tener latencia
        public Task<IEnumerable<TaskReadDto>> GetAllTasksAsync();

        public Task<TaskReadDto?> GetTaskById(int id);

        public Task<TaskReadDto> CreateTaskAsync(TaskCreateDto taskDto);
        
        public Task<bool> DeleteTaskAsync(int id);              

        public Task<IEnumerable<TaskReadDto>> GetTasksByCategoryAsync(int categoryId);

        public Task<string> UpdateTaskAsync(int id, TaskUpdateDto updateDto);
    }
}