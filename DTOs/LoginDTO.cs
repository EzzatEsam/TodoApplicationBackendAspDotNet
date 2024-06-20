using System.ComponentModel.DataAnnotations;

public record LoginDTO
{
    [EmailAddress]
    public required string Email { get; set; }
    public required string Password { get; set; }
}