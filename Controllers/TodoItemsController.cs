using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoProj.Auth;
using TodoProj.Context;
using TodoProj.DTOs;
using TodoProj.Models;

namespace TodoProj.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly TodoAppContext _context;
        private readonly UserManager<CustomUser> _userManager;

        public TodoItemsController(TodoAppContext context, UserManager<CustomUser> manager)
        {
            _context = context;
            _userManager = manager;

        }

        // GET: api/TodoItems
        [HttpGet("ParentGroup/{groupId}")]
        public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetTodoItems(long groupId)
        {
            return await _context.TodoItems.Where(x => x.GroupId == groupId && x.UserID == _userManager.GetUserId(User)).Select(x => TodoItemDTO.ToDTO(x)).ToListAsync();
        }

        // GET: api/TodoItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItemDTO>> GetTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.Where(x => x.UserID == _userManager.GetUserId(User) && x.Id == id).FirstOrDefaultAsync();

            if (todoItem == null)
            {
                return NotFound();
            }

            return TodoItemDTO.ToDTO(todoItem);
        }

        // PUT: api/TodoItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(long id, TodoItemDTO dto)
        {
            var item = await _context.TodoItems.Where(x => x.UserID == _userManager.GetUserId(User)).FirstOrDefaultAsync(x => x.Id == id);
            var group = await _context.TodoGroups.Include(x => x.Items).FirstOrDefaultAsync(x => x.Id == dto.GroupId);
            if (item == null || group == null)
            {
                return NotFound();
            }

            var names = group.Items.Select(x => x.Name).ToList();
            if (names.Contains(dto.Name))
            {
                return Conflict("Name already exists");
            }

            item.Name = dto.Name;
            item.IsDone = dto.IsDone;
            item.DueDate = dto.DueDate;
            item.CreatedDate = dto.CreatedDate;
            item.Description = dto.Description;
            item.GroupId = dto.GroupId;
            item.Group = group;


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/TodoItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TodoItemDTO>> PostTodoItem(TodoItemDTO dto)
        {
            var group = await _context.TodoGroups.Where(x => x.UserId == _userManager
                .GetUserId(User) && x.Id == dto.GroupId)
                .Include(x => x.Items)
                .SingleOrDefaultAsync();

            if (group == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return BadRequest("");
            }

            var names = group.Items.Select(x => x.Name).ToList();
            if (names.Contains(dto.Name))
            {
                return Conflict("Name already exists");
            }

            var item = new TodoItem
            {
                Name = dto.Name,
                IsDone = dto.IsDone,
                Description = dto.Description,
                DueDate = dto.DueDate,
                CreatedDate = dto.CreatedDate,
                GroupId = dto.GroupId,
                UserID = user.Id,
                User = user,
                Group = group
            };

            _context.TodoItems.Add(item);
            Console.WriteLine(group.Name);
            group.Items.ToList().ForEach(x => Console.WriteLine(x.Name));
            await _context.SaveChangesAsync();
            Console.WriteLine("after");
            Console.WriteLine(group.Name);
            group.Items.ToList().ForEach(x => Console.WriteLine(x.Name));
            return CreatedAtAction("GetTodoItem", new { id = item.Id }, TodoItemDTO.ToDTO(item));
        }

        // DELETE: api/TodoItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.Where(x => x.UserID == _userManager.GetUserId(User))
            .FirstOrDefaultAsync(x => x.Id == id);
            if (todoItem == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TodoItemExists(long id)
        {
            return _context.TodoItems.Any(e => e.Id == id);
        }
    }
}
