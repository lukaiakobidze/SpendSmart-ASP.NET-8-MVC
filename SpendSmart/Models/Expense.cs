using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace SpendSmart.Models
{
    public class Expense
    {
        public int Id { set; get; }
        public decimal Value { get; set; }
        
        public int CategoryId { get; set; }
        [Required]
        public string? Description { get; set; }
        
        public DateTime Date { get; set; }

    }
}
