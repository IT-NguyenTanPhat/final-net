using System.ComponentModel.DataAnnotations;

namespace timviec.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string Avatar { get; set; }
        [Required]
        public string Role { get; set; }

    }
}
