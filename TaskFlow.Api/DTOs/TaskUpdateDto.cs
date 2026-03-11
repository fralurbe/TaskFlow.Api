namespace TaskFlow.Api.DTOs {
    public class TaskUpdateDto {
        public string Title { get; set; }
        public bool IsCompleted { get; set; }
        public int CategoryId { get; set; }
    }
}
