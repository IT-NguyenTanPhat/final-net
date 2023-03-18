using System.ComponentModel.DataAnnotations;

namespace timviec.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Slug { get; set; }
        public virtual ICollection<Job> Jobs { get; set;}
    }
}
