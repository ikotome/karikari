using System;
using System.ComponentModel.DataAnnotations;

namespace Shogendar.Karikari.Models
{
    public class Loan
    {
        public int Id { get; set; }
        public required int Event { get; set; }
        public int PayerId { get; set; }
        public User Payer { get; set; }
        public int RepayerId { get; set; }
        public User Repayer { get; set; }
        public decimal Amount { get; set; }
    }
}
