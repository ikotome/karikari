using System.Xml.XPath;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Shogendar.Karikari.Models;

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
            if(!query.Any())
                return NotFound();
            return Ok(query.ToList());
        }

        [HttpPut]
        public IActionResult CreateLoans(int loanId, Event eventId, int payerId, int repayerId, decimal amount){
            Db db = new();
            Loan result;
            if (loanId == 0){
                result = new Models.Loan { PayerId = payerId, RepayerId = repayerId, Amount = amount};
                db.Loans.Add(result);
            }else{
                // Loanから loanIdを元に
                var query = from Loan in db.Loans
                            where Loan.Id == loanId
                            select Loan;
                result = query.FirstOrDefault();
                if(result is null)
                    result = new Loan { Id = loanId, Event = eventId };
                result.PayerId = payerId;
                result.RepayerId = repayerId;
                result.Amount = amount;
            }
            db.SaveChanges();
            return Created("", result);
        }
    }
}
