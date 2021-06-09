using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace project8.Models.TaskViewModels
{
    public class TaskCreateViewModel
    {
        [Required]
        [MinLength(2, ErrorMessage = "The minimum length of the task name is 2 symbols.")]
        [Display(Name = "Task Name")]
        public string Name { get; set; }
    }
}