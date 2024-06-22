using TodoProj.Models;

namespace TodoProj.DTOs;

public record TodoItemDTO
{

    public required string Name { get; set; }
    public long GroupId { get; set; }

    public bool IsDone { get; set; } = false;
    public string Description { get; set; } = string.Empty;

    public long Id { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime CreatedDate { get; set; }

    public static TodoItemDTO ToDTO(TodoItem item)
    {
        return new TodoItemDTO
        {
            GroupId = item.GroupId,
            Id = item.Id,
            Name = item.Name,
            Description = item.Description,
            DueDate = item.DueDate,
            CreatedDate = item.CreatedDate,
            IsDone = item.IsDone,
        };
    }

}