using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace timviec.Models
{
    public class Apply
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual User? User { get; set; }
        [Required]
        [ForeignKey("Job")]
        public string JobId { get; set; }
        public virtual Job? Job { get; set; }
        public string? CoverLetter { get; set; }
        public string Status { get; set; } = "pending";
        public DateTime Time { get; set; } = DateTime.Now;
    }
}
