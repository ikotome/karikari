using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Shogendar.Karikari.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetLoansController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetLoans(int myId, int otherId ,bool userType){
            Db db = new();
            var query = from Loan in db.Loans
                        where userType ? Loan.PayerId == myId && Loan.RepayerId == otherId : Loan.RepayerId == myId && Loan.PayerId == otherId
                        select Loan;
            if(!query.Any())
                return NotFound();
            return Ok(query.ToList());
        }
    }
}
