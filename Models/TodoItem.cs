using System.Text.RegularExpressions;
using TodoProj.Auth;

namespace TodoProj.Models;

public class TodoItem
{
    public  long Id { get; set; }
    public DateTime DueDate { get; set; } = DateTime.Now;
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public required string Name { get; set; }
    public required string UserID {get; set; }
    public required CustomUser User { get; set; } 
    public string Description { get; set; } = string.Empty;
    public bool IsDone { get; set; } = false;
    public required long GroupId { get; set; }
    public  required TodoGroup Group { get; set; } 

}