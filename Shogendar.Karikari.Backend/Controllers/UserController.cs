using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Shogendar.Karikari.Models;

namespace Shogendar.Karikari.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet]
        public IActionResult ShowUser(int? id = null, string? name = null)
        {
            if (id is null && name is null)
                return BadRequest($"{nameof(id)}と{nameof(name)}のいずれかのパラメータを指定する必要があります。");
            Db db = new();
            var query = from User in db.Users
                        where id == null ? User.Name == name : User.Id == id
                        select User;
            if (!query.Any())
                return NotFound();
            return Ok(JsonSerializer.Serialize(query.FirstOrDefault()));
        }

        [HttpPut]
        public IActionResult CreateOrUpdateUser(int userId, string name, string email, string password)
        {
            Db db = new();
            User result;
            if (userId == 0)
            {
                result = new Models.User { Name = name, Email = email, Password = password };
                db.Users.Add(result);
            }
            else
            {
                var query = from User in db.Users
                            where User.Id == userId
                            select User;
                result = query.FirstOrDefault();
                if (result is null)
                    result = new User { Id = userId };
                db.Users.Add(result);
                result.Name = name;
                result.Email = email;
                result.Password = password;
            }
            db.SaveChanges();
            return Created("", JsonSerializer.Serialize(result));
        }

        [HttpDelete]
        public IActionResult DeleateUser(int userId)
        {
            Db db = new();
            var query = from User in db.Users
                        where User.Id == userId
                        select User;
            if (!query.Any())
                return NotFound();
            Models.User result = query.FirstOrDefault();
            db.Users.Remove(result);
            db.SaveChanges();
            return Ok(JsonSerializer.Serialize(query.ToList()));
        }
    }
}
