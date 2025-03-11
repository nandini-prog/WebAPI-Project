using Microsoft.AspNetCore.Mvc;
using SmartTaskApi.Data;  // Import your DbContext
using System;
using System.Collections.Generic;
using System.Linq;
using Task = SmartTaskApi.Models.Task;// Import Task Model

namespace SmartTaskApi.Controllers
{
    [Route("api/[controller]")] // Base route: /api/tasks
    [ApiController] // Marks this as a Web API controller
    public class TasksController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        // Constructor - Inject DbContext
        public TasksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/tasks - Fetch all tasks
        [HttpGet]
        public ActionResult<IEnumerable<Task>> GetAllTasks()
        {
            var tasks = _context.Tasks.ToList(); // Fetch all tasks from DB
            return Ok(tasks); // Return as HTTP 200 OK response
        }

        // POST: api/tasks - Create a new task
        [HttpPost]
        public ActionResult<Task> CreateTask([FromBody] Task task)
        {
            if (task == null)
            {
                return BadRequest("Task data is invalid.");
            }

            _context.Tasks.Add(task); // Add task to DB context
            _context.SaveChanges(); // Save to the database

            return CreatedAtAction(nameof(GetAllTasks), new { id = task.Id }, task);
        }

        [HttpPut("{id}")]  // Route: PUT /api/tasks/{id}
        public IActionResult UpdateTask(int id, [FromBody] Task updatedTask)
        {
            // 1. Check if the task exists in the database
            var task = _context.Tasks.Find(id);
            if (task == null)
            {
                return NotFound(); // Return 404 if the task is not found
            }

            // 2. Update the task properties
            task.Title = updatedTask.Title;
            task.Description = updatedTask.Description;
            task.DueDate = updatedTask.DueDate;
            task.Status = updatedTask.Status;

            // 3. Save changes to the database
            _context.SaveChanges();

            return NoContent(); // Return 204 No Content on successful update
        }

        // DELETE: api/tasks/{id} - Delete a task
        [HttpDelete("{id}")]
        public IActionResult DeleteTask(int id)
        {
            // 1. Check if the task exists in the database
            var task = _context.Tasks.Find(id);
            if (task == null)
            {
                return NotFound(); // 404 Not Found if task doesn't exist
            }

            // 2. Remove the task from the database
            _context.Tasks.Remove(task);
            _context.SaveChanges(); // Save changes to DB

            return NoContent(); // Return 204 No Content on success
        }

    }
}
