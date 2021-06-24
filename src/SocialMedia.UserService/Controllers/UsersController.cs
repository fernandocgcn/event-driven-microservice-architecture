using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialMedia.UserService.Data;
using SocialMedia.UserService.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialMedia.UserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserServiceContext _context;

        public UsersController(UserServiceContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {
            return await _context.User.ToListAsync();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.User.Add(user);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetUser", new { id = user.ID }, user);
        }
    }
}
