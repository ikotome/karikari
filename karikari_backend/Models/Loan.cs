using System;
using System.ComponentModel.DataAnnotations;

namespace karikari_backend.Models
{
    public class Loan
    {
        public int Id { get; set; }
        public required int Event { get; set; }
        public int Payer { get; set; }
        public int Repayer { get; set; }
        public decimal Amount { get; set; }
    }
}
