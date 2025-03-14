using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Shogendar.Karikari.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FriendController : ControllerBase
    {
        [HttpGet]
        ///
        public IActionResult borrower(int id){

            return Ok();
        }
    }
}
