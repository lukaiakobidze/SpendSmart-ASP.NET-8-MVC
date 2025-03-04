using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpendSmart.Models
{
    public class Expense
    {
        [Key]
        public int Id { set; get; }
        [Required]
        public decimal Value { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public string UserId { get; set; }
        public IdentityUser User { get; set; }

    }
}
