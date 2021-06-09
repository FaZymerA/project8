using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace project8.Models.TaskViewModels
{
    public class TaskDetailsViewModel
    {
        public string Name { get; set; }
        public bool IsCompleted { get; set; }
        public string Comment { get; set; }
        public long Duration { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? FinishedAt { get; set; }
    }
}