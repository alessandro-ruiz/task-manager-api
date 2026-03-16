using Microsoft.EntityFrameworkCore;
using TaskManagerApi.Data;
using TaskManagerApi.DTOs;
using TaskManagerApi.Interfaces;
using TaskManagerApi.Models;

namespace TaskManagerApi.Services
{
    public class TaskService : ITaskService
    {
        private readonly AppDbContext _context;

        public TaskService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResultDto<TaskResponseDto>> GetAllAsync(TaskQueryParametersDto query)
        {
            var tasksQuery = _context.Tasks.AsQueryable();

            if (query.IsCompleted.HasValue)
            {
                tasksQuery = tasksQuery.Where(t => t.IsCompleted == query.IsCompleted.Value);
            }

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                var search = query.Search.Trim().ToLower();

                tasksQuery = tasksQuery.Where(t =>
                    t.Title.ToLower().Contains(search) ||
                    (t.Description != null && t.Description.ToLower().Contains(search)));
            }

            var totalCount = await tasksQuery.CountAsync();

            var items = await tasksQuery
                .OrderByDescending(t => t.CreatedAt)
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(t => new TaskResponseDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    IsCompleted = t.IsCompleted,
                    CreatedAt = t.CreatedAt
                })
                .ToListAsync();

            return new PagedResultDto<TaskResponseDto>
            {
                Items = items,
                TotalCount = totalCount,
                Page = query.Page,
                PageSize = query.PageSize,
                TotalPages = (int)Math.Ceiling(totalCount / (double)query.PageSize)
            };
        }

        public async Task<TaskResponseDto?> GetByIdAsync(int id)
        {
            return await _context.Tasks
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
        }

        public async Task<TaskResponseDto> CreateAsync(CreateTaskDto dto)
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

            return new TaskResponseDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                IsCompleted = task.IsCompleted,
                CreatedAt = task.CreatedAt
            };
        }

        public async Task<bool> UpdateAsync(int id, UpdateTaskDto dto)
        {
            var existingTask = await _context.Tasks.FindAsync(id);

            if (existingTask == null)
                return false;

            existingTask.Title = dto.Title;
            existingTask.Description = dto.Description;
            existingTask.IsCompleted = dto.IsCompleted;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existingTask = await _context.Tasks.FindAsync(id);

            if (existingTask == null)
                return false;

            _context.Tasks.Remove(existingTask);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}