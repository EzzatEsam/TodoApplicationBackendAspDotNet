using TodoProj.Auth;

namespace TodoProj.Models;


public class TodoGroup
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public required string UserId { get; set; }
    public required CustomUser User { get; set; } 
    public ICollection<TodoItem> Items { get; set; } = [];
}

