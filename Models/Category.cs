using System.ComponentModel.DataAnnotations;

namespace timviec.Models
{
    public class Category
    {
        [Key]
        public string? Id { get; set; }
        [Required]
        public string Name { get; set; }
        public virtual ICollection<Job>? Jobs { get; set; }

    }
}
