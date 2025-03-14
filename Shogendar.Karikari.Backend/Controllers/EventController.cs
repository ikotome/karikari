using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shogendar.Karikari.Models;

namespace Shogendar.Karikari.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        [HttpGet]
        public IActionResult ShowEvents(int id){
            Db db = new();
            var query = from Event in db.Events
                        where Event.Id == id
                        select Event;
            if(!query.Any())
                return NotFound();
            return Ok(query.ToList());

        }

        [HttpPut]
        public IActionResult CreateOrUpdateEvent(int eventId, int groupId, string description, DateTime repaydate, bool settled){
            Db db = new();
            Event result;
            if (eventId == 0){
                result = new Models.Event { Group = groupId,  Description = description, RepayDate = repaydate, Settled = settled};
                db.Events.Add(result);
            }else{
                var query = from Event in db.Events
                            where Event.Id == eventId
                            select Event;
                result = query.FirstOrDefault();
                if(result is null)
                    result = new Event { Id = eventId, Description = description };
                result.Group = groupId;
                result.Description = description;
                result.RepayDate = repaydate;
                result.Settled = settled;

            }
            
            db.SaveChanges();
            return Created("", result);
        }

        [HttpDelete]
        public IActionResult DeleteEvent(int id){
            Db db = new();
            var query = from Event in db.Events
                        where Event.Id == id
                        select Event;
            Models.Event result = query.FirstOrDefault();
            db.Events.Remove(result);
            db.SaveChanges();
            return NoContent();
        }
    }
}
