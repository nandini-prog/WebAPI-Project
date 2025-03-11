using System;

namespace SmartTaskApi.Models
{
    public class Task
    {
        public int Id { get; set; } // Unique identifier (Primary Key)
        public string Title { get; set; } = string.Empty; // Required short name
        public string? Description { get; set; } // Optional detailed explanation
        public DateTime? DueDate { get; set; } // Nullable deadline
        public string Priority { get; set; } = "Medium"; // Default priority
        public string Status { get; set; } = "Pending"; // Default task state
        public int UserId { get; set; } // Reference to the User who owns the task
    }
}
