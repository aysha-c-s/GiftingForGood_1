using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace GFG1.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Phone { get; set; }
        [AllowNull]
        public string OptionalPhone { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        public List<Post>? Posts { get; set; }

    }
}