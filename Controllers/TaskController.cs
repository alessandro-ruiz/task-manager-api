using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagerApi.Data;
using TaskManagerApi.DTOs;
using TaskManagerApi.Models;

namespace TaskManagerApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TaskController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskResponseDto>>> GetTasks()
        {
            var tasks = await _context.Tasks
                .OrderByDescending(t => t.CreatedAt)
                .Select(t => new TaskResponseDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    IsCompleted = t.IsCompleted,
                    CreatedAt = t.CreatedAt
                })
                .ToListAsync();

            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TaskResponseDto>> GetTaskById(int id)
        {
            var task = await _context.Tasks
                .Where(t => t.Id == id)
                .Select(t => new TaskResponseDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    IsCompleted = t.IsCompleted,
                    CreatedAt = t.CreatedAt
                })
                .FirstOrDefaultAsync();

            if (task == null)
            {
                return NotFound(new { message = $"Task whit id {id} was not found" });
            }
            return Ok(task);
        }

        [HttpPost]
        public async Task<ActionResult<TaskResponseDto>> CreateTask(CreateTaskDto dto)
        {
            var task = new TaskItem
            {
                Title = dto.Title,
                Description = dto.Description,
                IsCompleted = false,
                CreatedAt = DateTime.UtcNow
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            var response = new TaskResponseDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                IsCompleted = task.IsCompleted,
                CreatedAt = task.CreatedAt
            };

            return CreatedAtAction(nameof(GetTaskById), new { id = task.Id }, response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTask(int id, UpdateTaskDto dto)
        {
            var existingTask = await _context.Tasks.FindAsync(id);

            if (existingTask == null)
                return NotFound(new { message = $"Task with id {id} was not found." });

            existingTask.Title = dto.Title;
            existingTask.Description = dto.Description;
            existingTask.IsCompleted = dto.IsCompleted;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTask(int id)
        {
            var existingTask = await _context.Tasks.FindAsync(id);

            if (existingTask == null)
                return NotFound(new { message = $"Task with id {id} was not found." });

            _context.Tasks.Remove(existingTask);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}