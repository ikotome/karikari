using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Shogendar.Karikari.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        [HttpGet]
        

        [HttpPost]
        public IActionResult CreateEvent(int groupId, string description, DateTime repaydate, bool settled){
            Db db = new();
            db.Event.Add(new Models.Event { GroupId = groupId,  Description = description, Repaydate = repaydate, Settled = sette})
        }
    }
}
