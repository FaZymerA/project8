using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace project8.Entities
{
    public class Task
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public long Duration { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ResumedAt { get; set; }
        public DateTime FinishedAt { get; set; }
    }
}