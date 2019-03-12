using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetUserViewModel>>> GetAllUser()
        {
            var user = await _context.Users.Take(2).ToListAsync();
            List<GetUserViewModel> list = new List<GetUserViewModel>();
            foreach (var item in user)
            {
                var todo = await _context.TodoItems.Where(x => x.UserId == item.Id).ToListAsync();
                GetUserViewModel model = new GetUserViewModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    Birthday = (item.Birthday.Ticks - 621355968000000000) / 10000,
                    Gender = item.Gender,
                    Email = item.Email,
                    NumberPhone = item.NumberPhone,
                    Address = item.Address,
                    ListTodo = todo
                };
                list.Add(model);
            }

            return list;
        }

        [HttpGet("{id}")]
        public ActionResult<GetUserViewModel> GetUser(string id)
        {
            var item = _context.Users.Find(id);
            var todo = _context.TodoItems.Where(x => x.UserId == item.Id).ToList();
            if (item != null)
            {
                GetUserViewModel model = new GetUserViewModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    Birthday = (item.Birthday.Ticks - 621355968000000000) / 10000,
                    Gender = item.Gender,
                    Email = item.Email,
                    NumberPhone = item.NumberPhone,
                    Address = item.Address,
                    ListTodo = todo
                };
                return model;
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult Create(AddUserViewModel user)
        {
            if (ModelState.IsValid)
            {
                user.Id = Guid.NewGuid().ToString();

                DateTime dtime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                User model = new User
                {
                    Id = user.Id,
                    Name = user.Name,
                    Birthday = dtime.AddMilliseconds(user.Birthday),
                    Gender = user.Gender,
                    Email = user.Email,
                    NumberPhone = user.NumberPhone,
                    Address = user.Address,
                };

                _context.Users.Add(model);
                _context.SaveChanges();

                return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
            }
            return BadRequest(ModelState);
        }

        private static DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddMilliseconds(unixTimeStamp);
            return dtDateTime;
        }

        [HttpPut("{id}")]
        public IActionResult Edit(string id, EditUserViewModel user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var users = _context.Users.Any(x => x.Id == id);

            if (id != user.Id && !users)
            {
                return BadRequest();
            }
            DateTime dtime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

            User model = new User
            {
                Id = user.Id,
                Name = user.Name,
                Birthday = dtime.AddMilliseconds(user.Birthday),
                Gender = user.Gender,
                Email = user.Email,
                NumberPhone = user.NumberPhone,
                Address = user.Address
            };

            _context.Entry(model).State = EntityState.Modified;
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

        [HttpGet("CheckBirthday")]
        public ActionResult CheckBirthday(long birthday)
        {
            long dtimeNow = (DateTime.Now.Ticks - 621355968000000000) / 10000;
            if (birthday <= dtimeNow)
                return Ok();
            return NotFound();
        }
        [HttpGet("CheckEmail")]
        public ActionResult CheckEmail(string email)
        {
            var item = _context.Users.Where(x => x.Email == email).FirstOrDefault(); ;
            if (item == null)
                return Ok();
            return NotFound();
        }
    }
}