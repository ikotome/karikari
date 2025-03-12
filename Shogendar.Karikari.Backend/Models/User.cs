using System;
using System.ComponentModel.DataAnnotations;

namespace Shogendar.Karikari.Backend.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
