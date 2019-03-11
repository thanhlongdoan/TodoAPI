using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly TodoContext _context;
        public TodoController(TodoContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<PageModel>> GetListTodo(string searchString, int? pageNumber = 1)
        {
            int PageSize = 2;
            PageModel model = new PageModel();

            if (!String.IsNullOrEmpty(searchString))
            {
                model = new PageModel
                {
                    PageSize = PageSize,
                    CurrentPage = (int)pageNumber,
                    TotalPages = (int)Math.Ceiling(_context.TodoItems.Where(x => x.Name.Contains(searchString)).Count() / (double)PageSize),
                    items = await _context.TodoItems.Where(x => x.Name.Contains(searchString)).Skip(((int)pageNumber - 1) * PageSize).Take(PageSize).ToListAsync(),
                };
            }
            else
            {
                model = new PageModel
                {
                    PageSize = PageSize,
                    CurrentPage = (int)pageNumber,
                    TotalPages = (int)Math.Ceiling(_context.TodoItems.Count() / (double)PageSize),
                    items = await _context.TodoItems.Skip(((int)pageNumber - 1) * PageSize).Take(PageSize).ToListAsync(),
                };
            }

            return model;
        }

        [HttpGet("GetTodoItem")]
        public async Task<ActionResult<GetTodoViewModel>> GetTodoItem(long id, string userId)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }
            else
            {
                var query = (from todo in _context.TodoItems
                             join user in _context.Users
                             on todo.UserId equals user.Id
                             where userId == todo.UserId
                             select new
                             {
                                 user.Id,
                                 user.Name,
                                 user.Birthday,
                                 user.Address,
                                 user.NumberPhone
                             }).FirstOrDefault();
                GetTodoViewModel model = new GetTodoViewModel
                {
                    Id = query.Id,
                    Name = query.Name,
                    Birthday = query.Birthday,
                    Address = query.Address,
                    NumberPhone = query.NumberPhone,
                    TodoItems = todoItem
                };
                return model;
            }
        }

        [HttpPost("{userId}")]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem item, string userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                item.UserId = userId;
                _context.TodoItems.Add(item);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetTodoItem), new { id = item.Id }, item);
            }
            return NotFound();
        }

        [HttpPut("PutTodoItem")]
        public async Task<IActionResult> PutTodoItem(long id, string userId, TodoItem item)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                if (id != item.Id)
                    return BadRequest();
                _context.Entry(item).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok();
            }
            return NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var item = await _context.TodoItems.FindAsync(id);
            if (item == null)
                return NotFound();
            _context.TodoItems.Remove(item);
            _context.SaveChanges();
            return Ok();
        }
    }
}
