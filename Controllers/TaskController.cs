using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagerApi.Data;
using TaskManagerApi.DTOs;
using TaskManagerApi.Models;
using TaskManagerApi.Interfaces;
using TaskManagerApi.Services;

namespace TaskManagerApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResultDto<TaskResponseDto>>> GetTasks([FromQuery] TaskQueryParametersDto query)
        {
            if (query.Page <= 0) query.Page = 1;
            if (query.PageSize <= 0) query.PageSize = 10;
            if (query.PageSize > 50) query.PageSize = 50;

            var result = await _taskService.GetAllAsync(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TaskResponseDto>> GetTaskById(int id)
        {
            var task = await _taskService.GetByIdAsync(id);

            if (task == null)
            {
                return NotFound(new { message = $"Task whit id {id} was not found" });
            }
            return Ok(task);
        }

        [HttpPost]
        public async Task<ActionResult<TaskResponseDto>> CreateTask(CreateTaskDto dto)
        {
            var creadtedTask = await _taskService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetTaskById), new { id = creadtedTask.Id }, creadtedTask);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTask(int id, UpdateTaskDto dto)
        {
            var update = await _taskService.UpdateAsync(id, dto);

            if (!update)
                return NotFound(new { message = $"Task with id {id} was not found." });

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTask(int id)
        {
            var deleted = await _taskService.DeleteAsync(id);

            if (!deleted)
                return NotFound(new { message = $"Task with id {id} was not found." });

            return NoContent();
        }
    }
}