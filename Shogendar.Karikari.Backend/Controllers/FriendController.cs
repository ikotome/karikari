using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shogendar.Karikari.Models;

namespace Shogendar.Karikari.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FriendController : ControllerBase
    {
        [HttpGet]
        ///<summary>
        /// GetUsersAPIは、自分が 借り/貸し している人のLoanListが出ます
        ///</summary>
        public IActionResult GetUsers(int id, bool userType){
            Db db = new();
            var query = from loan in db.Loans
                            where (userType ? loan.PayerId : loan.RepayerId) == id
                            select userType ? loan.Repayer : loan.Payer;
            if(!query.Any())
                return NotFound();
            return Ok(query);
        }
    }
}
