using System.ComponentModel.DataAnnotations;

namespace ClientTodo.Models.ViewModel
{
    public class TodoViewModel
    {
        [Required]
        [StringLength(50)]
        [Display(Name = "Task Name")]
        public string TaskName { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsComplete { get; set; }
        [Required]
        public string AppUserId { get; set; }
        [Required]
        public int TaskTypeId { get; set; }
        #nullable enable
        public string? Description { get; set; }

    }
}
