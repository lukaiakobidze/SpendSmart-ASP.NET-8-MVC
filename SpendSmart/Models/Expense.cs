using System.ComponentModel.DataAnnotations;

namespace SpendSmart.Models
{
    public class Expense
    {
        public int Id { set; get; }
        public decimal Value { get; set; }

        [Required]
        public string? Description { get; set; }
    }
}
