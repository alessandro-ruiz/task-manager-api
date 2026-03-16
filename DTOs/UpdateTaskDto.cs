using System.ComponentModel.DataAnnotations;

namespace TaskManagerApi.DTOs
{
    public class UpdateTaskDto
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        public bool IsCompleted { get; set; }
    }
}