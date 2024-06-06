using System.ComponentModel.DataAnnotations;

namespace ClientTodo.Models
{
    public class TodoTask
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Task Name")]
        public string TaskName { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsComplete { get; set; }
#nullable enable
        public string? Description { get; set; }
#nullable disable
        [Required]
        public AppUser AppUser { get; set; }
    }
}
