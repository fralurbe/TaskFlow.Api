using System.ComponentModel.DataAnnotations;

namespace TaskFlow.Api.Models {
    public class Category {
        public int Id { get; set; }

        [Required(ErrorMessage =  "El nombre de la categoría es obligatorio")]
        public string Name { get; set; }

        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }
}
