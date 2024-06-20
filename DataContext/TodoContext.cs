using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TodoProj.Auth;
using TodoProj.Models;

namespace TodoProj.Context;

public class TodoAppContext : IdentityDbContext<CustomUser>

{
    public DbSet<TodoGroup> TodoGroups { get; set; }
    public DbSet<TodoItem> TodoItems { get; set; }

    public TodoAppContext(DbContextOptions<TodoAppContext> options)
        : base(options)
    {
    }

}