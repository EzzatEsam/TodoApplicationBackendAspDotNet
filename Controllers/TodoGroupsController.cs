using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Humanizer;
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
    public class TodoGroupsController : ControllerBase
    {
        private readonly TodoAppContext _context;
        private readonly UserManager<CustomUser> _userManager;

        public TodoGroupsController(TodoAppContext context, UserManager<CustomUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        // GET: api/TodoGroups
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoGroupDTO>>> GetTodoGroups()
        {

            if (_userManager.GetUserId(User) == null)
            {
                return Unauthorized();
            }
            return await _context.TodoGroups.Where(x => x.UserId == _userManager.GetUserId(User)).Include(x => x.Items).Select(x => TodoGroupDTO.ToDTO(x)).ToListAsync();
        }

        // GET: api/TodoGroups/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoGroupDTO>> GetTodoGroup(long id)
        {

            var userId = _userManager.GetUserId(User);
            var todoGroup = await _context.TodoGroups.Where(x => x.UserId == userId).Include(x => x.Items).FirstOrDefaultAsync(x => x.Id == id);

            if (todoGroup == null)
            {
                return NotFound();
            }

            return TodoGroupDTO.ToDTO(todoGroup);
        }

        // PUT: api/TodoGroups/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoGroup(long id, TodoGroupDTO todoGroup)
        {

            var UserId = _userManager.GetUserId(User);
            var group = await _context.TodoGroups.Where(x => x.UserId == UserId).FirstOrDefaultAsync(x => x.Id == id);
            if (group == null)
            {
                return NotFound();
            }

            group.Name = todoGroup.Name;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!TodoGroupExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/TodoGroups
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TodoGroupDTO>> PostTodoGroup(TodoGroupDTO todoGroup)
        {

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Unauthorized();
            }
            var group = new TodoGroup { Name = todoGroup.Name, UserId = user.Id , User = user};
            _context.TodoGroups.Add(group);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTodoGroup), new { id = group.Id }, TodoGroupDTO.ToDTO(group));
        }

        // DELETE: api/TodoGroups/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoGroup(long id)
        {
            var todoGroup = await _context.TodoGroups.Where(x => x.UserId == _userManager.GetUserId(User)).FirstOrDefaultAsync(x => x.Id == id);
            if (todoGroup == null)
            {
                return NotFound();
            }

            _context.TodoGroups.Remove(todoGroup);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TodoGroupExists(long id)
        {
            return _context.TodoGroups.Any(e => e.Id == id);
        }

    }
}
