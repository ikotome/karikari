using System.Text.Json;
using System.Xml.XPath;
using Microsoft.AspNetCore.Components.Web;
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
        public IActionResult ShowLoans(int id)
        {
            Db db = new();
            var query = from loan in db.Loans
                        where loan.Id == id
                        select loan;
            if (!query.Any())
                return NotFound();
            return Ok(JsonSerializer.Serialize(query.FirstOrDefault()));
        }

        [HttpPut]
        public IActionResult CreateLoans(int loanId, string title, string description, int payerId, int repayerId, decimal amount, DateTime paydate, DateTime repaydate, LoanType type, PaymentMethod method)
        {
            Db db = new();
            Loan result;
            if (loanId == 0)
            {
                result = new Models.Loan { Title = title, Description = description, PayerId = payerId, RepayerId = repayerId, Amount = amount, PayDate = paydate, RepayDate = repaydate, Type = type, Method = method };
                db.Loans.Add(result);
            }
            else
            {
                // Loanから loanIdを元に
                var query = from Loan in db.Loans
                            where Loan.Id == loanId
                            select Loan;
                result = query.FirstOrDefault();
                if (result is null)
                    result = new Loan { Id = loanId, Title = title, PayerId = payerId, RepayerId = repayerId, Amount = amount, Type = type };
                db.Loans.Add(result);
                result.Description = description;
                result.PayDate = paydate;
                result.RepayDate = repaydate;
                result.Method = method;
            }
            db.SaveChanges();
            return Created("", JsonSerializer.Serialize(result));
        }
    }
}
