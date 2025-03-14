using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shogendar.Karikari.Models;

namespace Shogendar.Karikari.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
         [HttpGet]
        public IActionResult ShowGroup(int id){
            Db db = new();
            var query = from Group in db.Groups
                            where Group.Id == id
                            select Group;
            if(!query.Any())
                return NotFound();
            return Ok(query.ToList());
        }

        [HttpPut]
        public IActionResult CreateOrUpdateGroup(int groupId, string name, List<User> users){
            Db db = new();
            Models.Group result;
            if (groupId == 0){
                result = new Models.Group{ Name = name, };
                db.Groups.Add(result);
            }else{
                var query = from Group in db.Groups
                            where Group.Id == groupId
                            select Group;
                result = query.FirstOrDefault();
                if(result is null)
                    result = new Models.Group { Id = groupId };
                result.Name = name;
                result.Users = users;
            }
            db.SaveChanges();
            return Created("",result);
        }

        [HttpDelete]
        public IActionResult DeleateGroup(int groupId){
            Db db = new();
            var query = from Group in db.Groups
                            where Group.Id == groupId
                            select Group;
            if(!query.Any())
                return NotFound();
            Models.Group result = query.FirstOrDefault();
            db.Groups.Remove(result);
            db.SaveChanges();
            return Ok(query.ToList());
        }
    }
}
