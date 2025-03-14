using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Shogendar.Karikari.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetLoansController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetLoans(int myId, int otherId, bool userType)
        {
            Db db = new();
            var query = from Loan in db.Loans
                        where userType ? Loan.PayerId == myId && (otherId == 0 || Loan.RepayerId == otherId) : Loan.RepayerId == myId && (otherId == 0 || Loan.PayerId == otherId)
                        select Loan;
            if (!query.Any())
                return NotFound();
            return Ok(JsonSerializer.Serialize(query.ToList()));
        }
    }
}
