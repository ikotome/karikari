using System;
using System.ComponentModel.DataAnnotations;

namespace Shogendar.Karikari.Models
{
    public class Loan
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public required int PayerId { get; set; }
        public User? Payer { get; set; }
        public required int RepayerId { get; set; }
        public User? Repayer { get; set; }
        public required decimal Amount { get; set; }
        public DateTime PayDate { get; set; }
        public DateTime RepayDate { get; set; }
        public required LoanType Type{ get; set; }
        public PaymentMethod Method{ get; set; }
        
    }
    public enum PaymentMethod { 
        /// <summary>
        /// 現金
        /// </summary>
        Cash, 
        /// <summary>
        /// 送金
        /// </summary>
        Transfer, 
        /// <summary>
        /// 商品
        /// </summary>
        Goods
    }
    public enum LoanType { 
        /// <summary>
        /// 貸す
        /// </summary>
        Lent, 
        /// <summary>
        /// 返す
        /// </summary>
        Return
    }
}
