using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shogendar.Karikari.Models;

namespace Shogendar.Karikari.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetUsersController : ControllerBase
    {
        [HttpGet]
        ///<summary>
        /// GetUsersAPIは、自分が 借り/貸し している人のLoanListが出ます
        /// ture: 自分が貸した側  false:自分が借りた側
        ///</summary>
        public IActionResult GetUsers(int id, bool userType)
        {
            Db db = new();
            var query = from loan in db.Loans
                        where (userType ? loan.PayerId : loan.RepayerId) == id
                        select userType ? loan.Repayer : loan.Payer;
            if (!query.Any())
                return NotFound();
            return Ok(JsonSerializer.Serialize(query.ToList()));
        }

    }
}
