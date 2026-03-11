using System.ComponentModel.DataAnnotations;

namespace TaskFlow.Api.DTOs {
    public class TaskCreateDto {
        [Required(ErrorMessage = "¡Eh! El título no puede estar vacío.")]
        public string Title { get; set; } = string.Empty;
        public int CategoryId { get; set; }
    }
}