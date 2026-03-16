using TaskManagerApi.DTOs;

namespace TaskManagerApi.Interfaces
{
    public interface ITaskService
    {
        Task<PagedResultDto<TaskResponseDto>> GetAllAsync(TaskQueryParametersDto query);
        Task<TaskResponseDto?> GetByIdAsync(int id);
        Task<TaskResponseDto> CreateAsync(CreateTaskDto dto);
        Task<bool> UpdateAsync(int id, UpdateTaskDto dto);
        Task<bool> DeleteAsync(int id);
    }
}