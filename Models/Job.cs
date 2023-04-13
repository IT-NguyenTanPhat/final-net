using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace timviec.Models
{
    public class Job
    {
        [Key]
        public string? Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int? Salary { get; set; }
        public int? Experience { get; set; }
        public string Status { get; set; } = "pending";
        public string Description { get; set; }
        [ForeignKey("Category")]
        public string? CategoryId { get; set; }
        public virtual Category? Category { get; set; }
        [ForeignKey("Company")]
        public string? CompanyId { get; set; }
        public virtual Company? Company { get;set; }
        public DateTime PostedAt { get; set; } = DateTime.Now;
    }
}
