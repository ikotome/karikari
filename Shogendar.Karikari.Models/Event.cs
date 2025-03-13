using System;
using System.ComponentModel.DataAnnotations;

namespace Shogendar.Karikari.Models
{
    public class Event
    {
        public int Id { get; set; }
        public int Group { get; set; }
        public required string Description { get; set; }
        public DateTime PayDate { get; set; }
        public DateTime RepayDate { get; set; }
        /// <summary>
        /// Settled ＝ 解決ずみ(精算済みかどうか)
        /// </summary>
        public bool Settled { get; set; }
        /// <summary>
        /// このイベントにかかる金額の一覧
        /// </summary>
        public List<Loan> Loans { get; set; }
    }
}
