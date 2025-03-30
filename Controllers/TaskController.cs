using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SmartTaskApi.Data;  // Import your DbContext
using System;
using System.Collections.Generic;
using System.Linq;
using Task = SmartTaskApi.Models.Task;// Import Task Model
using SmartTaskApi.DTO;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;

namespace SmartTaskApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")] // Base route: /api/tasks
    [ApiController] // Marks this as a Web API controller
    public class TasksController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        // Constructor - Inject DbContext
        public TasksController(ApplicationDbContext context,IMapper
            mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        //[HttpGet]
        //public ActionResult<IEnumerable<TaskDto>> GetAllTasks(int pageNumber = 1, int pageSize = 10)
        //{
        //    var tasks = _context.Tasks
        //                        .Skip((pageNumber - 1) * pageSize)
        //                        .Take(pageSize)
        //                        .ToList();
        //    var taskDtos = _mapper.Map<IEnumerable<TaskDto>>(tasks); // Convert TaskModel -> TaskDto

        //    return Ok(taskDtos);
        //}

        //cursor based pagination
        //[HttpGet]
        //public ActionResult<object> GetAllTasks(int? lastTaskId = null, int pageSize = 10)
        //{
        //    var query = _context.Tasks.AsQueryable();

        //    // If lastTaskId is provided, fetch only records after that ID
        //    if (lastTaskId.HasValue)
        //    {
        //        query = query.Where(t => t.Id > lastTaskId.Value);
        //    }

        //    // Fetch paginated data
        //    var tasks = query.OrderBy(t => t.Id)
        //                     .Take(pageSize)
        //                     .ToList();

        //    // Get next cursor (last task ID in current result)
        //    int? nextCursor = tasks.Count > 0 ? tasks.Last().Id : null;

        //    var response = new
        //    {
        //        nextCursor, // Cursor for next page
        //        pageSize,
        //        tasks
        //    };

        //    return Ok(response);
        //}

        // POST: api/tasks - Create a new task

        //performance optimsation
        //Efficient Query Mapping(ProjectTo<T>()) → Maps directly from DB, avoiding extra memory usage.
        //[HttpGet]
        //public ActionResult<IEnumerable<TaskDto>> GetAllTasks(int pageNumber = 1, int pageSize = 10)
        //{
        //    var taskDtos = _context.Tasks
        //                           .ProjectTo<TaskDto>(_mapper.ConfigurationProvider)  // 🚀 Directly maps in SQL
        //                           .Skip((pageNumber - 1) * pageSize)
        //                           .Take(pageSize)
        //                           .ToList();

        //    return Ok(taskDtos);
        //}
        [HttpGet]
        public ActionResult<IEnumerable<TaskDto>> GetAllTasks(
    int pageNumber = 1,
    int pageSize = 10,
    string? status = null,
    DateTime? dueDate = null,
    int? userId = null)
        {
            var query = _context.Tasks.AsQueryable(); // Start with IQueryable

            // Apply filters dynamically
            if (!string.IsNullOrEmpty(status))
                query = query.Where(t => t.Status == status);

            if (dueDate.HasValue)
                query = query.Where(t => t.DueDate == dueDate.Value);

            if (userId.HasValue)
                query = query.Where(t => t.UserId == userId.Value);

            // Apply mapping and pagination efficiently using ProjectTo<T>()
            var taskDtos = query
                           .ProjectTo<TaskDto>(_mapper.ConfigurationProvider) // Maps in DB
                           .Skip((pageNumber - 1) * pageSize)
                           .Take(pageSize)
                           .ToList();

            return Ok(taskDtos);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
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
