using System.ComponentModel.DataAnnotations;

namespace timviec.Models
{
    public class User
    {
        [Key]
        public string? Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public DateTime? Birthday { get; set; }
        public string? Title { get; set; }
        public string? Location { get; set; }
        public string? Phone { get; set; }
        public string? Description { get; set; }
        public string? Skills { get; set; }
        public string Avatar { get; set; } = "/uploads/user.png";
    }
}
