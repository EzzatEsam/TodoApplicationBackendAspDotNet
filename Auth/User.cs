using Microsoft.AspNetCore.Identity;

namespace TodoProj.Auth;


public class CustomUser : IdentityUser
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
}