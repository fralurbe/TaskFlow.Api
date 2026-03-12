using System.ComponentModel.DataAnnotations;

namespace TaskFlow.Api.Models {
    public class TaskItem {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? AttachmentUrl { get; set; }        

        //Clave foránea para sql server
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public Priority Priority { get; set; } = Priority.Medio;
    }
}