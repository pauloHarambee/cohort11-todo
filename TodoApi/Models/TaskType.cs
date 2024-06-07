using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models
{
    public class TaskType
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
