namespace TaskFlow.Api.DTOs {
    public class TaskReadDto {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
        public string CategoryName { get; set; }
    }
}