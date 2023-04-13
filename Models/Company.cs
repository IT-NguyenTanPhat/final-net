using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace timviec.Models
{
    public class Company
    {
        [Key]
        public string? Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string? Locaiton { get; set; }    
        public string? Phone { get; set; }
        public string Avatar { get; set; } = "/uploads/user.png";
        public string? Website { get; set; }
        public string? Description { get; set; }
        public virtual ICollection<Job>? Jobs { get; set; }

    }
}
