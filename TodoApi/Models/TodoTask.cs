using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TodoApi.Models
{
    [Table("Tasks")]
    public class TodoTask
    {
        [Key]
        public int Id {get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Task Name")]
        public string TaskName {get; set;}
        public DateTime DueDate{get;set;}
        public bool IsComplete{get;set;}
        #nullable enable
        public string? Description{get;set;}
#nullable disable
        [Required]
        public AppUser AppUser { get;set;}
    }
}