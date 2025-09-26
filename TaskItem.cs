using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList
{
    public class TaskItem
    {
        public bool IsCompleted { get; set; } 
        public string Title { get; set; }
        public string Category { get; set; } 
        public DateTime DueDate { get; set; }
        public string Description { get; set; } 
        public int Priority { get; set; }

        public TaskItem() 
        {
            DueDate = DateTime.Now;
        }
    }
}
