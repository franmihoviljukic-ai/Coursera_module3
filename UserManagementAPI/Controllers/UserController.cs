using Microsoft.AspNetCore.Mvc;
using UserManagementAPI.Models;

namespace UserManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private static readonly List<User> users = new()
        {
            new User { Id = 1, Name = "John Doe", Email = "john@example.com" },
            new User { Id = 2, Name = "Jane Doe", Email = "jane@example.com" }
        };

        [HttpGet("{id}")]
        public ActionResult<User> GetUser(int id)
        {
            var user = users.FirstOrDefault(x => x.Id == id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpGet]
        public ActionResult<List<User>> GetUsers()
        {
            return Ok(users);
        }

        [HttpPost]
        public ActionResult<User> CreateUser(User user)
        {
            if (string.IsNullOrWhiteSpace(user.Name))
                return BadRequest("Name is required.");

            if (string.IsNullOrWhiteSpace(user.Email) ||
                !user.Email.Contains("@"))
                return BadRequest("Valid email is required.");

            user.Id = users.Count == 0
                ? 1
                : users.Max(x => x.Id) + 1;

            users.Add(user);

            return CreatedAtAction(
                nameof(GetUser),
                new { id = user.Id },
                user
            );
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, User updatedUser)
        {

            if (string.IsNullOrWhiteSpace(updatedUser.Name))
                return BadRequest("Name is required.");

            if (string.IsNullOrWhiteSpace(updatedUser.Email) ||
                !updatedUser.Email.Contains("@"))
                return BadRequest("Valid email is required.");

            var user = users.FirstOrDefault(x => x.Id == id);

            if (user == null)
                return NotFound();

            user.Name = updatedUser.Name;
            user.Email = updatedUser.Email;

            return NoContent();

        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = users.FirstOrDefault(x => x.Id == id);

            if (user == null)
                return NotFound();

            users.Remove(user);

            return NoContent();
        }
    }
}