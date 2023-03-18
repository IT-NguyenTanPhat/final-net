using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace timviec.Models
{
    public class Job
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        [ForeignKey("Company")]
        public int CompanyId { get; set; }
        public virtual Company Company { get;set; }
        [Required]
        public DateTime PostedAt { get; set; } = DateTime.Now;

    }
}
