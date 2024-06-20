using System.Text.Json.Serialization;
using TodoProj.Models;

namespace TodoProj.DTOs;

public record TodoGroupDTO
{
    public required string Name { get; set; }
    public required long Id { get; set; }

    [JsonIgnore]
    public List<TodoItemDTO> TodoItems { get; set; } = [];


    public static TodoGroupDTO ToDTO(TodoGroup group)
    {

        Console.WriteLine("Converting Group ", group.Name);
        group.Items.ToList().ForEach(x => Console.WriteLine(x.Name));
        return new TodoGroupDTO
        {
            Id = group.Id,
            Name = group.Name,
            TodoItems = group.Items.Select(TodoItemDTO.ToDTO).ToList(),
        };
    }
}