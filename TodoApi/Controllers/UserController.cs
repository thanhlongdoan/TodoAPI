using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public readonly TodoContext _context;
        public UserController(TodoContext context)
        {
            _context = context;
        }
        [HttpGet("{id}")]
        public ActionResult<GetUserViewModel> GetUser(string id)
        {
            var item = _context.Users.Find(id);
            if (item != null)
            {
                GetUserViewModel model = new GetUserViewModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    Birthday = item.Birthday.Ticks,
                    Gender = item.Gender,
                    Email = item.Email,
                    NumberPhone = item.NumberPhone,
                    Address = item.Address
                };
                return model;
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            if (ModelState.IsValid)
            {
                user.Id = Guid.NewGuid().ToString();
                await _context.Users.AddAsync(user);
                _context.SaveChanges();
                return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
            }
            return BadRequest(ModelState);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(string id, User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var users = await _context.Users.AnyAsync(x => x.Id == id);
            if (id != user.Id && !users)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var item = await _context.Users.FindAsync(id);
            if (item != null)
            {
                _context.Users.Remove(item);
                _context.SaveChanges();
                return Ok();
            }
            return NotFound();
        }
    }
}