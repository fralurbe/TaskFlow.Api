using TaskFlow.Api.DTOs;

namespace TaskFlow.Api.Services {
    public interface ITaskService {
        // Usamos Task (asíncrono) porque en Azure la red puede tener latencia
        Task<IEnumerable<TaskReadDto>> GetAllTasksAsync();
        Task<TaskReadDto?> GetTaskByIdAsync(int id);
        Task<TaskReadDto> CreateTaskAsync(TaskCreateDto taskDto);
    }
}