using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tripsafe.Users.Data;
using Tripsafe.Users.Data.Models;
using Tripsafe.Users.Service.Core.Users;

namespace Tripsafe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserHelper userHelper;

        public UsersController(IUserHelper UserHelper)
        {
            userHelper = UserHelper;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return Ok( await userHelper.GetUsersAsync());
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<User>> GetUser(Guid id)
        {
            var user = await userHelper.GetUserAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutUser(Guid id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            try
            {
                await userHelper.UpdateUserAsync(user);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!userHelper.UserExists(id))
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

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            await userHelper.CreateUserAsync(user);

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var user = await userHelper.GetUserAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            await userHelper.DeleteUserAsync(user);

            return NoContent();
        }

        private bool UserExists(Guid id)
        {
            return userHelper.UserExists(id);
        }
    }
}
