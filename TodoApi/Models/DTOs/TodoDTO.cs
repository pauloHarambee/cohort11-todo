using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TodoApi.Models.DTOs
{
    public class TodoDTO
    {
        [Required]
        [StringLength(50)]
        [Display(Name = "Task Name")]
        public string TaskName {get; set;}
        public DateTime DueDate{get;set;}
        public bool IsComplete{get;set;}
        [Required]
        public string AppUserId { get; set;}
#nullable enable
        public string? Description{get;set;}


        public TodoTask ToTodoTask() => new TodoTask {
            TaskName = this.TaskName,
            DueDate = DueDate,
            IsComplete = IsComplete,
            Description = Description,
            
        };
    }
}