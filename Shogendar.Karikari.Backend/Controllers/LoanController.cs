using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Shogendar.Karikari.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanController : ControllerBase
    {
        [HttpGet]
        public IActionResult ShowLoans(int uid){
            Db db = new();
            var query = from loan in db.Loans
                        where loan.PayerId == uid
                        select loan;
            return Ok(query.ToList());
        }


    }
}
